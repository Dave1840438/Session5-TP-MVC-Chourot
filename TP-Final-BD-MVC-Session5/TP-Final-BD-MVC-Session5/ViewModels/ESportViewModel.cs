using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TP_Final_BD_MVC_Session5.ViewModels
{
    public class ESportViewModel
    {
        public long Id { get; set; }
        public String Nom { get; set; }
        public DateTime DateCreation { get; set; }

        [DataType(DataType.Upload)]
        public HttpPostedFileBase Logo { get; set; }
    }
}