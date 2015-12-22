using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace TP_Final_BD_MVC_Session5.Controllers
{
    public class TeamsController : InterfaceController
    {
        //
        // GET: /Teams/
        public ActionResult Index(long? idToDelete, long? idSports) //List
        {
            ViewBag.Message = "Your contact page.";

            if (idToDelete.HasValue)
            {
                Models.CompositionsEquipes BDCompo = new Models.CompositionsEquipes();
                BDCompo.DeleteAllRecordByFieldName("IdTeam", idToDelete);

                Models.Teams BDTeam = new Models.Teams();
                BDTeam.DeleteRecordByID((long)idToDelete);
            }

            ViewModels.TeamsViewModel allTeams = new ViewModels.TeamsViewModel();
            if (idSports.HasValue)
                ViewBag.hasRow = allTeams.SelectByFieldName("IdSport", idSports);
            else
                ViewBag.hasRow = allTeams.SelectAll();

            return View(allTeams);
        }

        [HttpGet]
        public ActionResult Insert()
        {
            ViewBag.Message = "Your application description page.";
            return View();
        }

        [HttpPost]
        public ActionResult Insert(Models.Teams team)
        {
            if (ModelState.IsValid)
            {
                WebImage logo = WebImage.GetImageFromRequest();
                if (logo != null)
                    team.LogoEquipe = saveImageToServer(logo);

                team.Insert();

                return RedirectToAction("Index");
            }
            else
            {
                return View();
            }
        }

        [HttpGet]
        public ActionResult UpdateTeam(long? idToUpdate)
        {
            if (idToUpdate.HasValue)
            {
                Models.Teams BDTeam = new Models.Teams();
                BDTeam.SelectByID(idToUpdate.ToString());
                return View(BDTeam);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public ActionResult UpdateTeam(Models.Teams team)
        {
            if (ModelState.IsValid)
            {
                WebImage logo = WebImage.GetImageFromRequest();

                if (logo != null)
                    team.LogoEquipe = saveImageToServer(logo);

                team.Update();

                return RedirectToAction("Index");
            }
            else
                return View(team);
        }
    }
}