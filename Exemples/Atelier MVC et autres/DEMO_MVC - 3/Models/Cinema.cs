using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinema
{
    public enum Genre { action, comédie, drame, horreur, sentimentale, fiction };

    public class Film
    {
        public long Id { get; set; }

        [Display(Name = "Titre")]
        [StringLength(50), Required]
        [RegularExpression(@"^((?!^Name$)[-a-zA-Z0-9 àâäçèêëéìîïòôöùûüÿñÀÂÄÇÈÊËÉÌÎÏÒÔÖÙÛÜ_'])+$", ErrorMessage = "Caractères illégaux.")]
        public String Titre { get; set; }


        [Display(Name = "Date de parution")]
        [DataType(DataType.Date)]
        public DateTime Parution { get; set; }

        public Genre Genre { get; set; }

        public Film() 
        {
            Titre = "";
            Parution = DateTime.Now;
            Genre = Cinema.Genre.action;
        }

    }
    public class Films : SqlExpressUtilities.SqlExpressWrapper
    {      
        public Film film { get; set; }
        public Films(object cs)
            : base(cs)
        {
           film = new Film();
        }
        public Films() { film = new Film(); }

        public List<Film> ToList()
        {

            List<object> list = this.RecordsList();
            List<Cinema.Film> films_list = new List<Film>();
            foreach (Film film in list)
            {
                films_list.Add(film);
            }
            return films_list;
        }
    }

    public class Acteur
    {
        public long Id { get; set; }
        public String Nom { get; set; }
        public DateTime Naissance { get; set; }
        public String Nationalite { get; set; }

        public Acteur() { }

    }

    public class Acteurs : SqlExpressUtilities.SqlExpressWrapper
    {
        public Acteur acteur { get; set; }
        public Acteurs(object cs)
            : base(cs)
        {
            acteur = new Acteur();
        }
        public Acteurs() { acteur = new Acteur(); }
    }

    public class Parution
    {
        public long Id { get; set; }
        public long Film_Id { get; set; }
        public long Acteur_Id { get; set; }

        public Parution() { }
    }
    public class Parutions : SqlExpressUtilities.SqlExpressWrapper
    {
        public Parution parution { get; set; }
        public Parutions(object cs)
            : base(cs)
        {
            parution = new Parution();
        }
        public Parutions() { parution = new Parution(); }
    }


    public class Acteurs_Par_Film : SqlExpressUtilities.SqlExpressWrapper
    {
        public Acteur acteur { get; set; }

        private String film_Titre = "";

        public Acteurs_Par_Film(String film_Titre, object cs)
            : base(cs)
        {
            this.film_Titre = film_Titre;
            acteur = new Acteur();
        }

        public Acteurs_Par_Film() { acteur = new Acteur(); }

        public override void SelectAll(string orderBy = "")
        {
            string sql =    "SELECT " +
                            "Acteurs.Nom, " +
                            "Acteurs.Naissance, " +
                            "Acteurs.Nationalite " +
                            "FROM Films INNER JOIN Parutions " +
                            "ON Films.Id = Parutions.Film_Id INNER JOIN Acteurs " +
                            "ON Parutions.Acteur_Id = Acteurs.Id " +
                            "WHERE Films.Titre = " + SqlExpressUtilities.SQLHelper.ConvertValueFromMemberToSQL(film_Titre);

            if (orderBy != "")
                sql += " ORDER BY " + orderBy;

            QuerySQL(sql);
        }
    }
}
