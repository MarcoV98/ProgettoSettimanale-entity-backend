using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using ProgettoSettimanale_entity_backend.Models;

namespace ProgettoSettimanale_entity_backend.Controllers
{
    public class HomeController : Controller
    {
        private ModelDbContext db = new ModelDbContext();
        public List<Prodotto> prodottos = new List<Prodotto>();

        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }

        public ActionResult Register()
        {

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register([Bind(Include = "Username, Password")] Utenti u)
        {
            u.Ruolo = "User";
            Utenti user = db.Utenti.Add(u);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult Login()
        {

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login([Bind(Include = "IdUtente,Username, Password")] Utenti u)
        {
            Utenti users = db.Utenti.FirstOrDefault(m => m.Username == u.Username & m.Password == u.Password);
            if (users != null)
            {
                FormsAuthentication.SetAuthCookie(u.Username, false);
                return RedirectToAction("Index");
            }

            return View();

        }
        public ActionResult Logout()
        {
            Session["Cliente"] = null;
            FormsAuthentication.SignOut();

            return RedirectToAction("Index", "Home");
        }

        public ActionResult CreateCliente()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateCliente(Clienti c)
        {

            Clienti cliente = db.Clienti.Add(c);
            Session["Cliente"] = cliente.Nome;
            Session["Cognome"] = cliente.Cognome;
            Session["Indirizzo"] = cliente.Indirizzo;
            Session["IdCliente"] = cliente.IdCliente;
            db.SaveChanges();
            return RedirectToAction("Index");

        }

        public ActionResult CreateOrdine()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CreateOrdine([Bind(Include = "IdOrdine, Allergie,IdCliente,Evaso,DataConsegna,NoteAggiunzione")] Ordini o)
        {
            if (Session["Cliente"] == null)
            {
                return RedirectToAction("CreateCliente", "Home");
            }
            else
            {
                Ordini ordini = new Ordini();
                List<Prodotto> prodotto = new List<Prodotto>();
                prodotto = (List<Prodotto>)Session["Carello"];
                Session["Cart1"] = Session["Carello"];

                foreach (Prodotto p in prodotto)
                {
                    o.PizzeScelte.Add(new PizzeScelte
                    {
                        Quantità = p.Quantità,
                        PizzaScelta = p.IdPizza,
                    });
                };
                Clienti clienti = new Clienti();
                clienti.Nome = (string)Session["Cliente"];

                Clienti c = db.Clienti.FirstOrDefault(m => m.Nome == clienti.Nome);

                o.IdClienti = c.IdCliente;
                Session["IdOrdine"] = o.IdOrdine;
                o.Evaso = false;
                ordini = db.Ordini.Add(o);

                db.SaveChanges();
                Session["OrdineEfettuato"] = Session["Carello"];
                Session["carello"] = null;


                //  Prodotto.ListPizze.Clear();
                return RedirectToAction("Ordini");
            }


        }
        public ActionResult AggiungiOrdine(int quantità, int idPizza)
        {

            Pizze pizze = db.Pizze.FirstOrDefault(m => m.IdPizza == idPizza);

            Prodotto prod = new Prodotto();
            prod.IdPizza = idPizza;
            prod.Quantità = quantità;
            prod.Nome = pizze.Nome;
            prod.Prezzo = pizze.Prezzo;
            prod.TempoConsegna = pizze.TempoConsegna;
            prod.Ingredienti = pizze.Ingredienti;
            Prodotto.ListPizze.Add(prod);
            prodottos.Add(prod);
            //Prodotto p=
            if (Session["Carello"] == null)
            {
                Session["Carello"] = prodottos;
            }
            else
            {
                List<Prodotto> prodott = new List<Prodotto>();
                prodott = (List<Prodotto>)Session["Carello"];
                prodott.Add(prod);
                Session["Carello"] = prodott;
            };


            List<Prodotto> newList = Prodotto.ListPizze;

            return View();
        }

        public ActionResult Cart()
        {
            List<Prodotto> prodotto = new List<Prodotto>();
            prodotto = (List<Prodotto>)Session["Carello"];
            List<decimal> TotaleOrdine = new List<decimal>();
            if (Session["Carello"] != null)
            {
                foreach (Prodotto p in prodotto)
                {
                    decimal q = Convert.ToDecimal(p.Quantità);
                    decimal prezzo = Convert.ToDecimal(p.Prezzo);
                    decimal tot = q *= prezzo;
                    TotaleOrdine.Add(tot);
                }
                ViewBag.totale = TotaleOrdine.Sum();
                Session["TotOrdine"] = TotaleOrdine.Sum();
            }
            else
            {
                return View();
            }



            return View(prodotto);
        }

        public ActionResult CartRemove()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CartRemove(Prodotto p)
        {

            List<Prodotto> prodotto = new List<Prodotto>();
            prodotto = (List<Prodotto>)Session["Carello"];
            string id = Request.QueryString["Id"];

            p.IdPizza = Convert.ToInt16(id);
            prodotto.Remove(prodotto.FirstOrDefault(m => m.IdPizza == p.IdPizza));


            Session["Carello"] = prodotto;

            return RedirectToAction("Cart", "Home");
        }
        public ActionResult CartRemoveAll(Prodotto p)
        {
            DB.ListIncasso.Clear();
            Session["Carello"] = null;

            return RedirectToAction("Cart", "Home");
        }


        public ActionResult Dettagli(int Id)
        {
            if (Id == 0)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            Pizze p = db.Pizze.Find(Id);
            return View(p);

        }
        public ActionResult SelectCliente()
        {
            if (Session["Cliente"] != null && Session["Modifica"] == null)
            {
                Clienti clienti = new Clienti();
                clienti.Nome = (string)Session["Cliente"];
                clienti.Cognome = (string)Session["Cognome"];
                clienti.Indirizzo = (string)Session["Indirizzo"];

                Clienti c = db.Clienti.FirstOrDefault(m => m.Nome == clienti.Nome && m.Cognome == clienti.Cognome && m.Indirizzo == clienti.Indirizzo);
                return View(c);
            }
            else
            {

                int id = (int)Session["IdClien"];
                Clienti c = db.Clienti.Find(id);
                Session["Cliente"] = c.Nome;
                return View(c);
            }


        }

        public ActionResult Ordini()
        {

            ViewBag.totale = Session["TotOrdine"];
            Clienti clienti = new Clienti();
            clienti.Nome = (string)Session["Cliente"];
            if (Session["Cliente"] != null)
            {

                if (Session["OrdineEfettuato"] != null)
                {
                    DB.ListIncasso.Clear();
                    Clienti c = db.Clienti.FirstOrDefault(m => m.Nome == clienti.Nome);
                    Ordini o = db.Ordini.FirstOrDefault(m => m.IdClienti == c.IdCliente);
                    Session["IdOrdine"] = o.IdOrdine;

                    DB.SelectOrdine(o.IdOrdine);
                    return View(DB.ListIncasso);
                }
                else
                {
                    RedirectToAction("SelectProdotti", "Admin");
                }
            }

            return View();
        }

        public ActionResult EditCliente(int id)
        {
            Session["IdClien"] = id;
            Clienti p = db.Clienti.Find(id);
            return View(p);
        }
        [HttpPost]
        public ActionResult EditCliente(Clienti p)
        {
            if (ModelState.IsValid)
            {
                //Session["Cliente"]=null
                int id = (int)Session["IdClien"];
                p.IdCliente = id;
                db.Entry(p).State = EntityState.Modified;
                Session["Modifica"] = p;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
        }

        public ActionResult DeletePizza(int id)
        {
            Pizze p = db.Pizze.Find(id);
            db.Pizze.Remove(p);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}