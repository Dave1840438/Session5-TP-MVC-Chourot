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

        public ActionResult Sort(String sortBy)
        {
            if (Session["Film_SortBy"] == null)
            {
                Session["Film_SortBy"] = sortBy;
                Session["Film_SortOrder"] = "ASC";
            }
            else
            {
                if ((String)Session["Film_SortBy"] == sortBy)
                {
                    if ((String)Session["Film_sortOrder"] == "ASC")
                        Session["Film_SortOrder"] = "DESC";
                    else
                        Session["Film_SortOrder"] = "ASC";
                }
                else
                {
                    Session["Film_SortBy"] = sortBy;
                    Session["Film_SortOrder"] = "ASC";
                }
            }
            return RedirectToAction("Lister", "Films");
        }
        public ActionResult Lister()
        {
            Films films = new Films(Session["DB_CINEMA"]);

            String orderBy = "";
            if (Session["Film_SortBy"] != null)
                orderBy = (String)Session["Film_SortBy"] + " " + (String)Session["Film_SortOrder"];

            films.SelectAll(orderBy);
            return View(films.ToList());
        }
        public ActionResult Ajouter()
        {
            return View(new Film());
        }

        [HttpPost]
        public ActionResult Ajouter(Cinema.Film film)
        {
            if (ModelState.IsValid)
            {
                Films films = new Films(Session["DB_CINEMA"]);
                films.film = film;
                films.film.Genre = (Cinema.Genre)int.Parse(Request["Genre"]);
                films.film.UpLoadPoster(Request);               
                films.Insert();
                return RedirectToAction("Lister", "Films");
            }
            return View(film);
        }

        public ActionResult Details(String Id)
        {
            Films films = new Films(Session["DB_CINEMA"]);
            if (films.SelectByID(Id))
                return View(films.film);
            else
                return RedirectToAction("Lister", "Films");
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
                    films.film.UpLoadPoster(Request);
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