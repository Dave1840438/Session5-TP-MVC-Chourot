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
         public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            Models.Esports lel = new Models.Esports();
            return View();
        }

        [HttpPost]
        public ActionResult About(TP_Final_BD_MVC_Session5.Models.Esports esport)
        {
            if (ModelState.IsValid)
            {
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
                    esport.Logo = newFileName;
                }

                esport.Insert();
            }

            return View();
        }

        public ActionResult Contact(long? idToDelete)
        {
            ViewBag.Message = "Your contact page.";

            if (idToDelete.HasValue)
            {
                Models.Esports BDEsport = new Models.Esports();
                BDEsport.DeleteRecordByID((long)idToDelete);
            }

            Models.Esports allEsports = new Models.Esports();
            ViewBag.hasRow = allEsports.SelectAll();

            return View(allEsports);
        }



        [HttpGet]
        public ActionResult UpdateESport(long? idToUpdate)
        {
            if (idToUpdate.HasValue)
            {
                Models.Esports BDEsport = new Models.Esports();
                BDEsport.SelectByID(idToUpdate.ToString());
                return View(BDEsport);
            }
            else
            {
                return RedirectToAction("Contact");
            }
        }

        [HttpPost]
        public ActionResult UpdateESport(Models.Esports esport)
        {
            if (ModelState.IsValid)
            {
                WebImage logo = WebImage.GetImageFromRequest();
                if (logo != null)
                {
                    String newFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(logo.FileName);
                    String fullPath = @"Images/" + newFileName;
                    string folderPath = Server.MapPath(@"~/Images");
                    if (!Directory.Exists(folderPath))
                        Directory.CreateDirectory(folderPath);

                    logo.Save(@"~/" + fullPath);
                    esport.Logo = newFileName;
                }

                esport.Update();

                return RedirectToAction("Contact");
            }
            else
                return View(esport);
        }
    }
}