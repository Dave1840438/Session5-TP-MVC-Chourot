using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TP_Final_BD_MVC_Session5.ViewModels
{
    public class TeamsViewModel : SqlExpressUtilities.SqlExpressWrapper
    {
         public long Id { get; set; }
        public String NomEquipe { get; set; }

        public long Classement { get; set; }
        public String LogoEquipe { get; set; }

        public String Nom { get; set; } //Du esport

        public DateTime DateCreation { get; set; }
        public String Logo { get; set; }

        public TeamsViewModel()
            : base(Constants.ConnectionString)
        {
        }

        public override bool SelectAll(string orderBy = "")
        {
            string sql = "SELECT " +
                            "Teams.Id, " +
                            "Teams.NomEquipe, " +
                            "Teams.Classement, " +
                            "Teams.LogoEquipe, " +
                            "ESports.Nom, " +
                            "ESports.DateCreation, " +
                            "ESports.Logo " +
                            "FROM Teams INNER JOIN ESports " +
                            "ON Teams.IdSport = ESports.Id";
            if (orderBy != "")
                sql += " ORDER BY " + orderBy;

            QuerySQL(sql);

            bool hadRow = reader.HasRows;

            if (hadRow)
                Next();
            else
                EndQuerySQL();

            return hadRow;
        }

        public override bool SelectByFieldName(String FieldName, object value, String orderBy = "")
        {
            string sql = "SELECT " +
                            "Teams.Id, " +
                            "Teams.NomEquipe, " +
                            "Teams.Classement, " +
                            "Teams.LogoEquipe, " +
                            "ESports.Nom, " +
                            "ESports.DateCreation, " +
                            "ESports.Logo " +
                            "FROM Teams INNER JOIN ESports " +
                            "ON Teams.IdSport = ESports.Id" +
                            " WHERE " + FieldName + " = " + SqlExpressUtilities.SQLHelper.ConvertValueFromMemberToSQL(value);

            if (orderBy != "")
                sql += " ORDER BY " + orderBy;

            QuerySQL(sql);

            bool hadRow = reader.HasRows;

            if (hadRow)
                Next();
            else
                EndQuerySQL();

            return hadRow;
        }  

    }
}