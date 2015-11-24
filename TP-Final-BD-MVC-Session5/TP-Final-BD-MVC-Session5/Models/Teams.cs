using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TP_Final_BD_MVC_Session5.Models
{
    public class Teams : SqlExpressUtilities.SqlExpressWrapper
    {
         public long Id { get; set; }
        [Required(ErrorMessage="Un non doit être entré"), MaxLength(150), Display(Name = "Nom:\u00a0")] //\u00a0 est l'unicode pour un &nbsp
        public String NomEquipe { get; set; }
        [Required(ErrorMessage = "Un classement doit être entré"), Range(1, 9999), Display(Name = "Classement:\u00a0")] //\u00a0 est l'unicode pour un &nbsp

        public long Classement { get; set; }
        [Required(ErrorMessage = "Un logo doit être émis"), Display(Name = "Logo:\u00a0")] //\u00a0 est l'unicode pour un &nbsp
        public String LogoEquipe { get; set; }

        [Required(ErrorMessage="Un esport doit être attribué"), Range(1, int.MaxValue), Display(Name="Nom du esport:\u00a0")]
        public long IdSport { get; set; }
        public Teams()
            : base(TP_Final_BD_MVC_Session5.Constants.ConnectionString)
        {
        }
    }
}