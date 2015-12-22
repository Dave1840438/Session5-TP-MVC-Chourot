using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace TP_Final_BD_MVC_Session5.Controllers
{
    public class CompositionsEquipesController : InterfaceController
    {
        //
        // GET: /CompositionsEquipes/
        public ActionResult Index(long? idToDelete, long? idTeam, string fieldName = "Score") //List
        {
            ViewBag.idTeam = idTeam;
            ViewBag.FieldName = fieldName;

            ViewBag.Message = "Your contact page.";

            if (idToDelete.HasValue)
            {
                Models.CompositionsEquipes BDCompo = new Models.CompositionsEquipes();
                BDCompo.DeleteRecordByID((long)idToDelete);
            }

            ViewModels.CompositionsEquipesViewModel allCompo = new ViewModels.CompositionsEquipesViewModel();
            if (idTeam.HasValue && idTeam != -1)
            {
                ViewBag.hasRow = allCompo.SelectByFieldName("IdTeam", idTeam, fieldName);
            }
            else
                ViewBag.hasRow = allCompo.SelectAll(fieldName);

            return View(allCompo);
        }

        [HttpGet]
        public ActionResult Insert()
        {
            ViewBag.Message = "Your application description page.";
            return View();
        }

        [HttpPost]
        public ActionResult Insert(TP_Final_BD_MVC_Session5.Models.CompositionsEquipes compo)
        {
            if (ModelState.IsValid)
            {
                compo.Insert();
                return RedirectToAction("Index");
            }
            else
                return View();
        }

        [HttpGet]
        public ActionResult UpdateCompositionsEquipes(long? idToUpdate)
        {
            if (idToUpdate.HasValue)
            {
                Models.CompositionsEquipes BDCompo = new Models.CompositionsEquipes();
                BDCompo.SelectByID(idToUpdate.ToString());
                return View(BDCompo);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public ActionResult UpdateCompositionsEquipes(Models.CompositionsEquipes compo)
        {
            if (ModelState.IsValid)
            {
                compo.Update();
                return RedirectToAction("Index");
            }
            else
                return View(compo);
        }
    }
}