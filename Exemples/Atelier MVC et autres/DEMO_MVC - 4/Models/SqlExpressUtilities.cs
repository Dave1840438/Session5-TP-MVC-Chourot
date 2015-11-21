using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Reflection;


namespace SqlExpressUtilities
{
    ////////////////////////////////////////////////////////////////////////////////////////////////////
    // SqlExpressWrapper version beta
    ////////////////////////////////////////////////////////////////////////////////////////////////////
    // Cette classe offre une interface conviviale au programmeur utilisateur pour des transactions SQL
    // avec une table d'une base de données SQL Express
    //
    // Note importante concernant les tables:
    // Afin de profiter des toutes les fonctionnalités de cette classe
    // assurez-vous que le premier champ soit Id de type BigInt INDETITY(1,1) dans la structure des 
    // tables visées
    //
    // Note importante concernant les sous-classes directes:
    //
    //  Le premier attribut de la sous-classe doit être du format suivant
    //
    //
    //  public class Enregistrement
    //  {
    //      public long Id {get; set;}
    //      public Type Nom_Champ_1 {get; set;}
    //      public Type Nom_Champ_2 {get; set;}
    //      ...
    //      public Type Nom_Champ_n {get; set;}
    //
    //      public Enregistrement() {}
    //  }
    //
    //  public class Nom_de_la_table : SQLExpressWrapper
    //  {
    //      public Enregistrement Enregistrement {get; set;}
    //
    //      public Nom_de_la_Table(String chaine_de_connection) : base( chaine_de_connection)
    //      {
    //          Enregistrement = new Enregistrement();
    //      }    
    //
    //      public Nom_de_la_Table()
    //      {
    //          Enregistrement = new Enregistrement();
    //      }    
    //
    ////////////////////////////////////////////////////////////////////////////////////////////////////
    // Auteur : Nicolas Chourot (Tous droits réservés)
    // Départment d'informatique
    // Collège Lionel-Groulx
    ////////////////////////////////////////////////////////////////////////////////////////////////////
    public class SqlExpressWrapper
    {
        // objet de connection
        SqlConnection connection;
        // chaine de connection
        public string connexionString;
        // Objet de lecture issue de la dernière requête SQL
        public SqlDataReader reader;
        // Nom de la table
        public String SQLTableName = "";

        // contructeur obligatoire auquel il faut fournir la chaine de connection
        //
        //      Idéalement utiliser l'objet Session initiliasé dans Session_Start() du fichier source Global.axax.cs
        //
        //      string DB_Path = Server.MapPath(@"~\App_Data\MaBaseDeDonnees.mdf");
        //      Session["DB_Con"] = @"Data Source=(LocalDB)\v11.0;AttachDbFilename='" + DB_Path + "'; Integrated Security=true;Max Pool Size=1024;Pooling=true;";
        //
        public SqlExpressWrapper(Object connexionString)
        {
            this.connexionString = connexionString.ToString();
            SQLTableName = this.GetType().Name;
        }
        public SqlExpressWrapper()
        {
        }

        public void SetTableName(String tableName)
        {
            SQLTableName = tableName;

        }

        public int ReaderColumnIndex(string columnName)
        {
            for (int i = 0; i < reader.FieldCount; i++)
            {
                if (reader.GetName(i) == columnName)
                {
                    return i;
                }
            }
            return -1;
        }

        public Type ReaderColumnType(string columnName)
        {
            int columnIndex = ReaderColumnIndex(columnName);
            if (columnIndex > -1)
                return reader.GetFieldType(columnIndex);
            else
                return null;
        }

        public Object ReaderColumnValue(string columnName)
        {
            int columnIndex = ReaderColumnIndex(columnName);
            if (columnIndex > -1)
                return SQLHelper.ConvertValueFromSQLToMember(reader.GetValue(columnIndex));
            else
                return null;
        }

        // Cette méthode utilise "Reflection" pour extraire les membres de type attribut du premier attribut de la sous-classe
        //  directe de la classe SqlExpressWrapper et ainsi affecter leur valeur avec celles du datareader
        // 
        public void GetReaderValues()
        {
            // obtenir le type de la sous-classe
            Type type = this.GetType();
            // Extraire la liste des membres
            PropertyInfo first_attribute_properties = type.GetProperties()[0];
            object first_attribute = first_attribute_properties.GetValue(this, null);
            PropertyInfo[] properties = first_attribute.GetType().GetProperties();
            // Parcourrir la liste des membres
            for (int i = 0; i < properties.Length; i++)
            {
                // vérifier que le membre d'index i est un attribut
                if (properties[i].GetIndexParameters().GetLength(0) == 0)
                {
                    String memberName = properties[i].Name;

                    // vérifier qu'il y a un champ de la table qui porte le même nom que l'attribut
                    // avant de lui attribuer sa valeur
                    if (ReaderColumnIndex(memberName) > -1)
                    {
                        var value = ReaderColumnValue(memberName);
                        String typeValue = value.GetType().ToString();
                        if (typeValue != "System.DBNull")
                            properties[i].SetValue(first_attribute, value, null);
                        else
                            properties[i].SetValue(first_attribute, null, null);
                    }
                }
            }
        }


        // retourne le clone du premier attribut de la sous-classe directe
        public object Clone()
        {
            Type type = this.GetType();
            PropertyInfo first_attribute_properties = type.GetProperties()[0];
            object first_attribute = first_attribute_properties.GetValue(this, null);
            Object copy = Activator.CreateInstance(first_attribute.GetType());

            PropertyInfo[] source_properties = first_attribute.GetType().GetProperties();
            PropertyInfo[] copy_properties = copy.GetType().GetProperties();

            for (int i = 0; i < source_properties.Length; i++)
            {
                // vérifier que le membre d'index i est un attribut
                if (source_properties[i].GetIndexParameters().GetLength(0) == 0)
                {
                    copy_properties[i].SetValue(copy, source_properties[i].GetValue(first_attribute, null), null);
                }
            }
            return copy;
        }


        // Retourne la liste de tous les enregistrements du datareader "reader"
        public List<object> RecordsList()
        {
            List<object> list = new List<object>();
            while (Next())
            {
                list.Add(Clone());
            }
            return list;
        }

        // Extraire les valeurs des champs de l'enregistrement suivant du lecteur Reader
        bool GetfieldsValues()
        {
            bool NotEndOfReader = false;

            // si il reste des enregistrements à lire
            if (NotEndOfReader = reader.Read())
            {
                GetReaderValues();
            }
            return NotEndOfReader;
        }

        // Passer à l'enregistrement suivant du lecteur de requête
        public bool NextRecord()
        {
            // Mettre à jour la collection de valeur des champs
            return GetfieldsValues();
        }

        // Saisir les valeurs du prochain enregistrement du Reader
        public bool Next()
        {
            if (reader != null)
            {
                bool more = NextRecord();
                if (!more)
                    EndQuerySQL();
                return more;
            }
            return false;
        }

        // Exécuter une commande SQL
        public void QuerySQL(string sqlCommand)
        {
            // instancier l'objet de collection
            if (SQLTableName != "")
            {
                try
                {
                    connection = new SqlConnection(connexionString);
                    try
                    {
                        // bâtir l'objet de requête
                        SqlCommand sqlcmd = new SqlCommand(sqlCommand);
                        // affecter l'objet de connection à l'objet de requête
                        sqlcmd.Connection = connection;
                        // ouvrir la connection avec la bd
                        connection.Open();
                        // éxécuter la requête SQL et récupérer les enregistrements qui en découlent dans l'objet Reader
                        reader = sqlcmd.ExecuteReader();
                        if (!reader.HasRows)
                            EndQuerySQL();
                    }
                    catch (Exception ex)
                    {
                        // Todo - rapport d'erreur
                        EndQuerySQL();
                    }
                }
                catch (Exception ex)
                {
                    // Todo - rapport d'erreur
                    EndQuerySQL();
                }
            }
        }

        // Conclure la dernière requête
        public void EndQuerySQL()
        {
            // Fermer la connection
            if (connection != null)
            {
                if (connection.State != System.Data.ConnectionState.Closed)
                {
                    connection.Close();
                    connection.Dispose();
                    reader.Dispose();
                    connection = null;
                    reader = null;
                }
            }
        }

        // Éxécuter une requête SQL qui ne génère pas d'enregistrement
        public int NonQuerySQL(string sqlCommand)
        {
            // instancier l'objet de collection
            using (connection = new SqlConnection(connexionString))
            {
                // bâtir l'objet de requête
                using (SqlCommand sqlcmd = new SqlCommand(sqlCommand))
                {
                    // affecter l'objet de connection à l'objet de requête
                    sqlcmd.Connection = connection;
                    // ouvrir la connection avec la bd
                    connection.Open();
                    // éxécuter la requête SQL et récupérer les enregistrements qui en découlent dans l'objet Reader
                    reader = sqlcmd.ExecuteReader();
                    int recordsAffected = reader.RecordsAffected;
                    EndQuerySQL();
                    return recordsAffected;
                }
            }
        }

        // Extraire tous les enregistrements
        public virtual void SelectAll(string orderBy = "")
        {
            string sql = "SELECT * FROM " + SQLTableName;
            if (orderBy != "")
                sql += " ORDER BY " + orderBy;
            QuerySQL(sql);
        }
        public bool SelectByID(long ID)
        {
            return SelectByID(ID.ToString());
        }
        // Extraire l'enregistrement d'id ID
        public bool SelectByID(String ID)
        {
            string sql = "SELECT * FROM " + SQLTableName + " WHERE ID = " + ID;
            QuerySQL(sql);
            if (reader != null)
            {
                Next();
                return true;
            }
            return false;
        }

        public bool SelectLast()
        {
            string sql = "SELECT TOP 1 * FROM " + SQLTableName + " ORDER BY ID DESC";
            //SELECT TOP 1 column_name FROM table_name ORDER BY column_name DESC;
            QuerySQL(sql);
            if (reader != null)
            {
                Next();
                return true;
            }
            return false;
        }

        public void SelectByFieldName(String FieldName, object value, String orderBy = "")
        {
            string SQL = "SELECT * FROM " + SQLTableName + " WHERE " + FieldName + " = " + SQLHelper.ConvertValueFromMemberToSQL(value);

            if (orderBy != "") SQL += " ORDER BY " + orderBy;

            QuerySQL(SQL);
        }

        public void SelectPeriod(String DateTimeFieldName, DateTime min, DateTime max, String orderBy = "")
        {
            String start = SQLHelper.DateSQLFormat((DateTime)min);
            String End = SQLHelper.DateSQLFormat((DateTime)max);
            string SQL = "SELECT * FROM " + SQLTableName + " WHERE " + DateTimeFieldName + " >= '" + start + "' AND " + DateTimeFieldName + " <= '" + End + "'";

            if (orderBy != "")
                SQL += " ORDER BY " + orderBy;
            QuerySQL(SQL);
        }

        // Insérer un nouvel enregistrement
        public virtual void Insert()
        {
            InsertRecord();
        }

        // insérer un nouvel enregistrement en utilisant les valeurs stockées dans FieldValues
        public void InsertRecord()
        {
            string SQL = "INSERT INTO " + SQLTableName + "(";
            Type type = this.GetType();
            // Extraire la liste des membres du premier attribut de la sous-classe directe
            PropertyInfo first_attribute_properties = type.GetProperties()[0];
            object first_attribute = first_attribute_properties.GetValue(this, null);
            PropertyInfo[] properties = first_attribute.GetType().GetProperties();

            // Parcourrir la liste des membres
            int i;
            for (i = 1; i < properties.Length; i++)
            {
                // vérifier que le membre d'index i est un attribut
                if (properties[i].GetIndexParameters().GetLength(0) == 0)
                {
                    SQL += properties[i].Name + ", ";
                }
            }

            SQL = SQL.Remove(SQL.LastIndexOf(", "), 2);
            SQL += ") VALUES (";

            for (i = 1; i < properties.Length; i++)
            {
                // vérifier que le membre d'index i est un attribut
                if (properties[i].GetIndexParameters().GetLength(0) == 0)
                {
                    SQL += SQLHelper.ConvertValueFromMemberToSQL(properties[i].GetValue(first_attribute, null)) + ", ";
                }
            }
            SQL = SQL.Remove(SQL.LastIndexOf(", "), 2);
            SQL += ")";
            NonQuerySQL(SQL);
        }

        // Mise à jour de l'enregistrement
        public virtual void Update()
        {
            UpdateRecord();
        }

        // Met à jour de l'enregistrement courant par le biais des valeurs inscrites dans la liste
        // FieldsValues
        // IMPORTANT: le premier membre du premier attribut de la sous-classe directe doit s'appeler Id et être de type long
        public int UpdateRecord()
        {
            String SQL = "UPDATE " + SQLTableName + " ";
            SQL += "SET ";
            // obtenir le type de la sous-classe
            Type type = this.GetType();
            // Extraire la liste des membres du premier attribut de la sous-classe directe
            PropertyInfo first_attribute_properties = type.GetProperties()[0];
            object first_attribute = first_attribute_properties.GetValue(this, null);
            PropertyInfo[] properties = first_attribute.GetType().GetProperties();
            // parcourrir la liste après le champ Id
            for (int i = 1; i < properties.Length; i++)
            {
                if (properties[i].GetIndexParameters().GetLength(0) == 0)
                {
                    SQL += "[" + properties[i].Name + "] = " + SQLHelper.ConvertValueFromMemberToSQL(properties[i].GetValue(first_attribute, null)) + ", ";
                }
            }
            SQL = SQL.Remove(SQL.LastIndexOf(", "), 2);
            // Id
            SQL += " WHERE [" + properties[0].Name + "] = " + SQLHelper.ConvertValueFromMemberToSQL(properties[0].GetValue(first_attribute, null));

            return NonQuerySQL(SQL);
        }

        // Effacer l'enregistrement d'id ID
        public virtual void DeleteRecordByID(String ID)
        {
            String sql = "DELETE FROM " + SQLTableName + " WHERE ID = " + ID;
            NonQuerySQL(sql);
        }
        public virtual void DeleteRecordByID(long ID)
        {
            DeleteRecordByID(ID.ToString());
        }

        public void DeleteAllRecordByFieldName(String fieldName, object value)
        {
            String SQL = "DELETE FROM " + SQLTableName + " WHERE " + fieldName + "= " + SQLHelper.ConvertValueFromMemberToSQL(value);

            NonQuerySQL(SQL);
        }
    }

    public class SQLHelper
    {

        // pour éviter des erreurs de syntaxe dans les requêtes sql
        static public string PrepareForSql(string text)
        {
            return text.Replace("'", "&c&");
        }

        static public string FromSql(string text)
        {
            return text.Replace("&c&", "'");
        }

        static string TwoDigit(int n)
        {
            string s = n.ToString();
            if (n < 10)
                s = "0" + s;
            return s;
        }

        public static string DateSQLFormat(DateTime date)
        {
            return date.Year + "-" + TwoDigit(date.Month) + "-" + TwoDigit(date.Day) + " " + TwoDigit(date.Hour) + ":" + TwoDigit(date.Minute) + ":" + TwoDigit(date.Second) + ".000";
        }

        public static bool IsNumericType(Type type)
        {
            if (type == null)
            {
                return false;
            }

            switch (Type.GetTypeCode(type))
            {
                case TypeCode.Byte:
                case TypeCode.Decimal:
                case TypeCode.Double:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.SByte:
                case TypeCode.Single:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                    return true;
                case TypeCode.Object:
                    if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
                    {
                        return IsNumericType(Nullable.GetUnderlyingType(type));
                    }
                    return false;
            }
            return false;
        }
        public static object ConvertValueFromSQLToMember(Object memberValue)
        {
            if (memberValue.GetType() == typeof(String))
                return SQLHelper.FromSql(memberValue.ToString()).Trim();
            else
                return memberValue;
        }
        public static String ConvertValueFromMemberToSQL(Object memberValue)
        {
            String Sql_value = "";
            if (memberValue != null)
            {

                if (SQLHelper.IsNumericType(memberValue.GetType()))
                {
                    if (memberValue.GetType().IsEnum)
                        Sql_value = ((int)memberValue).ToString();
                    else
                        Sql_value = memberValue.ToString().Replace(',', '.');
                }
                else
                {
                    if (memberValue.GetType() == typeof(DateTime))
                        Sql_value = "'" + SQLHelper.DateSQLFormat((DateTime)memberValue) + "'";
                    else
                        if (memberValue.GetType() == typeof(System.Boolean))
                            Sql_value = ((System.Boolean)memberValue ? "1" : "0");
                        else
                            Sql_value = "'" + SQLHelper.PrepareForSql((String)memberValue) + "'";
                }
            }
            else
            {
                Sql_value = " NULL ";
            }
            return Sql_value;
        }
    }
}