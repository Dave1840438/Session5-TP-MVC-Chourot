using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TP_Final_BD_MVC_Session5.Models
{
    public class Esports : SqlExpressUtilities.SqlExpressWrapper
    {
        public long Id { get; set; }
        public String Nom { get; set; }
        public DateTime DateCreation { get; set; }
        public String Logo { get; set; }
        public Esports(String cs)
            : base(cs)
        {
        }

    }
}