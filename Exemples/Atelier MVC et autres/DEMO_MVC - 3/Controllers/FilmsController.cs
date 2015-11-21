using Cinema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DEMO_MVC.Controllers
{
    public class FilmsController : Controller
    {
        public ActionResult Lister()
        {
            Films films = new Films(Session["DB_CINEMA"]);
            films.SelectAll();
            return View(films.ToList());
        }
        public ActionResult Ajouter()
        {
            return View();
        }

        [HttpPost]
        public   ActionResult Ajouter(Cinema.Film film)
        {
            if (ModelState.IsValid)
            {
                Films films = new Films(Session["DB_CINEMA"]);
                films.film = film;
                films.film.Genre = (Cinema.Genre)int.Parse(Request["Genre"]);
                films.Insert();
                return RedirectToAction("Lister", "Films");
            }
            return View(film);
        }

        public ActionResult Editer(String Id)
        {
            Films films = new Films(Session["DB_CINEMA"]);
            if (films.SelectByID(Id))
                return View(films.film);
            else
                return RedirectToAction("Lister", "Films");
        }
        [HttpPost]
        public ActionResult Editer(Cinema.Film film)
        {
            Films films = new Films(Session["DB_CINEMA"]);
            if (ModelState.IsValid)
            {
                if (films.SelectByID(film.Id))
                {
                    films.film = film;
                    films.film.Genre = (Cinema.Genre)int.Parse(Request["Genre"]);

                    films.Update();
                    return RedirectToAction("Lister", "Films");
                }
            }
            return View(film);
         }

        public ActionResult Effacer(String Id)
        {
            Films films = new Films(Session["DB_CINEMA"]);
            films.DeleteRecordByID(Id);
            return RedirectToAction("Lister", "Films");
        }

 	}
}