using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TP_Final_BD_MVC_Session5.Models
{
    public class Esports : SqlExpressUtilities.SqlExpressWrapper
    {
        public long Id { get; set; }
        [Required(ErrorMessage="Un non doit être entré"), MaxLength(50), Display(Name = "Nom:\u00a0")] //\u00a0 est l'unicode pour un &nbsp
        public String Nom { get; set; }
        [Required(ErrorMessage = "Une date doit être entrée"), Display(Name = "Date de création:\u00a0")] //\u00a0 est l'unicode pour un &nbsp

        public DateTime DateCreation { get; set; }
        [Required(ErrorMessage = "Un logo doit être émis"), Display(Name = "Logo:\u00a0")] //\u00a0 est l'unicode pour un &nbsp
        public String Logo { get; set; }
        public Esports()
            : base(TP_Final_BD_MVC_Session5.Constants.ConnectionString)
        {
        }

    }
}