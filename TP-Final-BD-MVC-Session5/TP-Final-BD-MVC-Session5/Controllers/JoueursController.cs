using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace TP_Final_BD_MVC_Session5.Controllers
{
    public class JoueursController : InterfaceController
    {
        //
        // GET: /Joueurs/
        public ActionResult Index(long? idToDelete) //List
        {
            ViewBag.Message = "Your contact page.";

            if (idToDelete.HasValue)
            {
                Models.CompositionsEquipes BDCompo = new Models.CompositionsEquipes();
                BDCompo.DeleteAllRecordByFieldName("IdJoueur", idToDelete);

                Models.Joueurs BDJoueur = new Models.Joueurs();
                BDJoueur.DeleteRecordByID((long)idToDelete);
            }

            Models.Joueurs allPlayers = new Models.Joueurs();
            ViewBag.hasRow = allPlayers.SelectAll();

            return View(allPlayers);
        }

        [HttpGet]
        public ActionResult Insert()
        {
            ViewBag.Message = "Your application description page.";
            return View();
        }

        [HttpPost]
        public ActionResult Insert(TP_Final_BD_MVC_Session5.Models.Joueurs joueur)
        {
            if (ModelState.IsValid)
            {
                WebImage logo = WebImage.GetImageFromRequest();
                if (logo != null)
                    joueur.Photo = saveImageToServer(logo);

                joueur.Insert();

                return RedirectToAction("Index");
            }
            else
                return View();
        }

        [HttpGet]
        public ActionResult UpdateJoueur(long? idToUpdate)
        {
            if (idToUpdate.HasValue)
            {
                Models.Joueurs BDJoueur = new Models.Joueurs();
                BDJoueur.SelectByID(idToUpdate.ToString());
                return View(BDJoueur);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public ActionResult UpdateJoueur(Models.Joueurs joueur)
        {
            if (ModelState.IsValid)
            {
                WebImage logo = WebImage.GetImageFromRequest();

                if (logo != null)
                    joueur.Photo = saveImageToServer(logo);

                joueur.Update();

                return RedirectToAction("Index");
            }
            else
                return View(joueur);
        }
	}
}