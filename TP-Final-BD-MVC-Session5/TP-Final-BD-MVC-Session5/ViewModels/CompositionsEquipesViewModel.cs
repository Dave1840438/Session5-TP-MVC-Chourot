using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TP_Final_BD_MVC_Session5.ViewModels
{
    public class CompositionsEquipesViewModel : SqlExpressUtilities.SqlExpressWrapper
    {
        public long Id { get; set; }
        public string NomComplet { get; set; }
        public string Photo { get; set; }

        public string NomEquipe { get; set; }
        public string LogoEquipe { get; set; }
        public string Nom { get; set; } //ESport
        public string Logo { get; set; } //ESport
        public long Score { get; set; }

        public CompositionsEquipesViewModel()
            : base(Constants.ConnectionString)
        {
        }

        public override bool SelectAll(string orderBy = "")
        {
            string sql = "SELECT " +
                            "CompositionsEquipes.Id, " +
                            "Joueurs.NomComplet, " +
                            "Joueurs.Photo, " +
                            "Teams.NomEquipe, " +
                            "Teams.LogoEquipe, " +
                            "ESports.Nom, " +
                            "ESports.Logo, " +
                            "CompositionsEquipes.Score " +
                            "FROM CompositionsEquipes INNER JOIN Joueurs " +
                            "ON CompositionsEquipes.IdJoueur = Joueurs.Id " +
                            "INNER JOIN Teams ON CompositionsEquipes.IdTeam = Teams.Id " +
                            "INNER JOIN ESports " +
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
                            "CompositionsEquipes.Id, " +
                            "Joueurs.NomComplet, " +
                            "Joueurs.Photo, " +
                            "Teams.NomEquipe, " +
                            "Teams.LogoEquipe, " +
                            "ESports.Nom, " +
                            "ESports.Logo, " +
                            "CompositionsEquipes.Score " +
                            "FROM CompositionsEquipes INNER JOIN Joueurs " +
                            "ON CompositionsEquipes.IdJoueur = Joueurs.Id " +
                            "INNER JOIN Teams ON CompositionsEquipes.IdTeam = Teams.Id " +
                            "INNER JOIN ESports " +
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