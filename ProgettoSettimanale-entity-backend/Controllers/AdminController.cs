using ProgettoSettimanale_entity_backend.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProgettoSettimanale_entity_backend.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private ModelDbContext db = new ModelDbContext();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CreateProdotti()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CreateProdotti(Pizze p, HttpPostedFileBase Foto)
        {
            if (Foto.ContentLength > 0)
            {
                string nameFile = Foto.FileName;
                string path = Path.Combine(Server.MapPath("~/Content/Img"), nameFile);
                Foto.SaveAs(path);
            }
            p.Foto = Foto.FileName;
            Pizze ordini = db.Pizze.Add(p);
            db.SaveChanges();
            return RedirectToAction("Index", "Home");

        }

        public ActionResult EditPizza(int id)
        {

            Pizze p = db.Pizze.Find(id);
            Session["IdPizz"] = p.IdPizza;
            return View(p);
        }
        [HttpPost]
        public ActionResult EditPizza(Pizze p, HttpPostedFileBase Foto)
        {
            if (ModelState.IsValid)
            {
                if (Foto.ContentLength > 0)
                {
                    string nameFile = Foto.FileName;
                    string path = Path.Combine(Server.MapPath("~/Content/Img"), nameFile);
                    Foto.SaveAs(path);
                }
                p.Foto = Foto.FileName;
                p.IdPizza = (int)Session["IdPizz"];
                db.Entry(p).State = EntityState.Modified;

                db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        public ActionResult EditOrdine(int id)
        {

            Ordini o = db.Ordini.Find(id);
            if (o == null)
            {
                return HttpNotFound();
            }
            Session["IdOrdine"] = id;
            Session["IdCl"] = o.IdClienti;
            return View(o);
        }
        [HttpPost]
        public ActionResult EditOrdine(Ordini o)
        {

            o.IdClienti = (int)Session["IdCl"];
            o.IdOrdine = (int)Session["Idordine"];
            db.Entry(o).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("SelectOrdini", "Admin");

        }

        public ActionResult DeletePizza(int id)
        {
            DB.EliminaPizza();
            Pizze p = db.Pizze.Find(id);
            db.Pizze.Remove(p);
            db.SaveChanges();
            return RedirectToAction("Index", "Home");
        }
        public ActionResult DeleteOrdine(int id)
        {
            DB.Elimina();
            Ordini p = db.Ordini.Find(id);
            db.Ordini.Remove(p);
            db.SaveChanges();
            return RedirectToAction("Index", "Home");
        }

        public ActionResult DettagliPizza(int Id)
        {
            if (Id == 0)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            Pizze p = db.Pizze.Find(Id);
            return View(p);
        }
        [AllowAnonymous]
        public ActionResult SelectProdotti()
        {
            if (User.IsInRole("Admin"))
            {
                return View(db.Pizze.ToList());
            }
            else if (User.IsInRole("User"))
            {
                return View(db.Pizze.ToList());
            }
            return RedirectToAction("Login", "Home");
        }
        public ActionResult SelectOrdini()
        {

            return View(db.Ordini.ToList());


        }

        public ActionResult Query()
        {

            return View();
        }
        public JsonResult TotOrdinIEvasi()
        {
            var totale = db.Ordini.Count(m => m.Evaso == true);
            return Json(totale, JsonRequestBehavior.AllowGet);
        }
        public JsonResult TotIncassoGiornata(DateTime DataConsegna)
        {
            var Ordini = db.Ordini.Where(m => m.DataConsegna == DataConsegna).ToList();
            var Pizze = db.Pizze.ToList();
            DB.ListIncasso.Clear();
            DB.CercaPrenotazione(DataConsegna);
            return Json(DB.Totale, JsonRequestBehavior.AllowGet);
        }

    }
}