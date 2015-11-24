using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace TP_Final_BD_MVC_Session5.Controllers
{
    public class ESportsController : InterfaceController
    {
        //
        // GET: /ESports/
        public ActionResult Index(long? idToDelete) //List
        {
            ViewBag.Message = "Your contact page.";

            if (idToDelete.HasValue)
            {
                Models.CompositionsEquipes BDCompo = new Models.CompositionsEquipes();

                Models.Teams BDTeam = new Models.Teams();
                if (BDTeam.SelectByFieldName("IdSport", idToDelete))
                {
                    do
                    {
                        BDCompo.DeleteAllRecordByFieldName("IdTeam", BDTeam.Id);
                    } while (BDTeam.Next());
                }

                BDTeam.DeleteAllRecordByFieldName("IdSport", idToDelete);

                Models.Esports BDEsport = new Models.Esports();
                BDEsport.DeleteRecordByID((long)idToDelete);
            }

            Models.Esports allEsports = new Models.Esports();
            ViewBag.hasRow = allEsports.SelectAll();

            return View(allEsports);
        }

        [HttpGet]
        public ActionResult Insert()
        {
            ViewBag.Message = "Your application description page.";
            return View();
        }

        [HttpPost]
        public ActionResult Insert(TP_Final_BD_MVC_Session5.Models.Esports esport)
        {
            if (ModelState.IsValid)
            {
                WebImage logo = WebImage.GetImageFromRequest();
                if (logo != null)
                    esport.Logo = saveImageToServer(logo);

                esport.Insert();

                return RedirectToAction("Index");
            }
            else
                return View();
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
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public ActionResult UpdateESport(Models.Esports esport)
        {
            if (ModelState.IsValid)
            {
                WebImage logo = WebImage.GetImageFromRequest();

                if (logo != null)
                    esport.Logo = saveImageToServer(logo);

                esport.Update();

                return RedirectToAction("Index");
            }
            else
                return View(esport);
        }

        public ActionResult Teams()
        {
            return View();
        }
    }
}