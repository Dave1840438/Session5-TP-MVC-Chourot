using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Cinema
{

    public class ImageGUIDReference
    {
        public String DefaultImage { get; set; }
        public String BasePath { get; set; }
        public String GUID { get; set; }
        public ImageGUIDReference(String basePath, String defautImage)
        {
            this.BasePath = basePath;
            this.DefaultImage = defautImage;
            GUID = "";
        }

        public String GetURL()
        {
            String url;
            if (String.IsNullOrEmpty(GUID))
                url = BasePath + DefaultImage;
            else
                url = BasePath + GUID + ".png";
            return url;
        }

        public String GetURI()
        {
            String uri;
            if (String.IsNullOrEmpty(GUID))
                uri = "";
            else
                uri = BasePath + GUID + ".png";
            return uri;
        }

        public String GetImageURL(String GUID)
        {
            String url;
            if (String.IsNullOrEmpty(GUID))
                url = BasePath + DefaultImage;
            else
                url = BasePath + GUID + ".png";
            return url;
        }

        public String UpLoadImage(HttpRequestBase Request, String PreviousGUID)
        {
            if (Request.Files.Count > 0)
            {
                var file = Request.Files[0];
                if (file != null && file.ContentLength > 0)
                {
                    GUID = PreviousGUID;
                    if (!String.IsNullOrEmpty(GUID))
                    {
                        System.IO.File.Delete(HttpContext.Current.Server.MapPath(GetURI()));
                    }
                    GUID = Guid.NewGuid().ToString();
                    file.SaveAs(HttpContext.Current.Server.MapPath(GetURI()));
                    return GUID;
                }
            }
            return PreviousGUID;
        }

        public void Remove(String GUID)
        {
            if (!String.IsNullOrEmpty(GUID))
            {
                this.GUID = GUID; 
                System.IO.File.Delete(HttpContext.Current.Server.MapPath(GetURI()));
            }
        }
    }
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

        [Display(Name = "Description")]
        [StringLength(1024), Required]
        public String Description { get; set; }

        [Display(Name = "Affiche")]
        public String Poster_Id { get; set; }

        private ImageGUIDReference ImageReference;

        public Film() 
        {
            Titre = "";
            Parution = DateTime.Now;
            Genre = Cinema.Genre.action;
            Description = "";
            Poster_Id = "";
            ImageReference = new ImageGUIDReference(@"/Images/Films/", @"UnknownPoster.png");
        }

        public String GetPosterURL()
        {
            return ImageReference.GetImageURL(Poster_Id);
        }

        public void UpLoadPoster(HttpRequestBase Request)
        {
            Poster_Id = ImageReference.UpLoadImage(Request, Poster_Id);
        }

        public void RemovePoster()
        {
            ImageReference.Remove(Poster_Id);
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

        public override void DeleteRecordByID(String ID)
        {
            if (this.SelectByID(ID))
            {
                this.film.RemovePoster();
                base.DeleteRecordByID(ID);
            }
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
