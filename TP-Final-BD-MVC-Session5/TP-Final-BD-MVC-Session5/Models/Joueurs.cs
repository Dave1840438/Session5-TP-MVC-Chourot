using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TP_Final_BD_MVC_Session5.Models
{
    public class Joueurs : SqlExpressUtilities.SqlExpressWrapper
    {
        public long Id { get; set; }
        [Required(ErrorMessage="Un non doit être entré"), MaxLength(100), Display(Name = "Nom Complet:\u00a0")] //\u00a0 est l'unicode pour un &nbsp
        public String NomComplet { get; set; }
        [Required(ErrorMessage = "Une date doit être entrée"), Display(Name = "Date de naissance:\u00a0")] //\u00a0 est l'unicode pour un &nbsp

        public DateTime DateNaissance { get; set; }
        [Required(ErrorMessage = "Un photo doit être émise"), Display(Name = "Logo:\u00a0")] //\u00a0 est l'unicode pour un &nbsp
        public String Photo { get; set; }
        public Joueurs()
            : base(TP_Final_BD_MVC_Session5.Constants.ConnectionString)
        {
        }
    }
}

