using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TP_Final_BD_MVC_Session5.Models
{
    public class CompositionsEquipes : SqlExpressUtilities.SqlExpressWrapper
    {

        public long Id { get; set; }
        [Required(ErrorMessage = "Un joueur doit être sélectionné"),  Display(Name = "Joueur:\u00a0")] //\u00a0 est l'unicode pour un &nbsp
        public long IdJoueur { get; set; }
        [Required(ErrorMessage = "Une équipe doit être sélectionnée"), Display(Name = "Équipe:\u00a0")] //\u00a0 est l'unicode pour un &nbsp

        public long IdTeam { get; set; }
        [Required(ErrorMessage = "Un score doit être entré"), Range(0, int.MaxValue), Display(Name = "Score:\u00a0")] //\u00a0 est l'unicode pour un &nbsp
        public long Score { get; set; }
        public CompositionsEquipes()
            : base(TP_Final_BD_MVC_Session5.Constants.ConnectionString)
        {
        }
    }
}