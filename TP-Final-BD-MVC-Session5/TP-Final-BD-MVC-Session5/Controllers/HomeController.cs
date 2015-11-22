using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace TP_Final_BD_MVC_Session5.Controllers
{
    public class HomeController : Controller
    {
        public const string ConnectionString = "Data Source=(LocalDB)\\v11.0;AttachDbFilename=C:\\Users\\Dominic\\Desktop\\Session5-TP-MVC-Chourot\\TP-Final-BD-MVC-Session5\\App_Data\\MainDB.mdf;Integrated Security=True;Connect Timeout=10";

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        [HttpPost]
        public ActionResult About(TP_Final_BD_MVC_Session5.ViewModels.ESportViewModel esport)
        {
            Models.Esports BDEsport = new Models.Esports(ConnectionString);

            BDEsport.DateCreation = esport.DateCreation;
            BDEsport.Nom = esport.Nom;
            


            ViewBag.Message = "Inserted!";
            WebImage logo = WebImage.GetImageFromRequest();
            if (logo != null)
            {
                String newFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(logo.FileName);
                String fullPath = @"Images/" + newFileName;
                string folderPath = Server.MapPath(@"~/Images");
                if (!Directory.Exists(folderPath))
                    Directory.CreateDirectory(folderPath);

                logo.Save(@"~/" + fullPath);
                BDEsport.Logo = newFileName;
            }

            BDEsport.Insert();

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            Models.Esports allEsports = new Models.Esports(ConnectionString);
            allEsports.SelectAll();


            return View(allEsports);
        }
    }
}