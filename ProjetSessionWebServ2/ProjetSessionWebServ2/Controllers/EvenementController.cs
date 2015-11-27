using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ProjetSessionWebServ2.Models;
using ProjetSessionWebServ2.DAL;

namespace ProjetSessionWebServ2.Controllers
{
    public class EvenementController : Controller
    {
        //private ApplicationDbContext db = new ApplicationDbContext();
        private UnitOfWork unitofwork = new UnitOfWork();

        // GET: /Evenement/
        public ActionResult Index()
        {
            //return View(db.Evenements.ToList());
            return View(unitofwork.EvenementRepository.ObtenirEvenements().ToList());
        }

        // GET: /Evenement/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Evenement evenement = db.Evenements.Find(id);
            Evenement evenement = unitofwork.EvenementRepository.ObtenirEvenementParID(id);
            if (evenement == null)
            {
                return HttpNotFound();
            }
            return View(evenement);
        }

        // GET: /Evenement/Create
        public ActionResult Create()
        {
            ViewBag.DropDownValue = new SelectList(new[] { Evenement.TypeEvent.TypeTournoi.ToString(), Evenement.TypeEvent.TypeKiosque.ToString(), Evenement.TypeEvent.TypeSpectacle.ToString(), Evenement.TypeEvent.TypeConference.ToString(), Evenement.TypeEvent.TypeAutre.ToString() });
            return View();
        }

        // POST: /Evenement/Create
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="Id,Nom,Description,TypeEvenement,Actif")] Evenement evenement)
        {
            if (ModelState.IsValid)
            {
                //db.Evenements.Add(evenement);
                //db.SaveChanges();
                unitofwork.EvenementRepository.Insert(evenement);
                unitofwork.Save();
                return RedirectToAction("Index");
            }

            return View(evenement);
        }

        // GET: /Evenement/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Evenement evenement = db.Evenements.Find(id);
            Evenement evenement = unitofwork.EvenementRepository.ObtenirEvenementParID(id);
            if (evenement == null)
            {
                return HttpNotFound();
            }
            return View(evenement);
        }

        // POST: /Evenement/Edit/5
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="Id,Nom,Description,TypeEvenement,Actif")] Evenement evenement)
        {
            if (ModelState.IsValid)
            {
                //db.Entry(evenement).State = EntityState.Modified;
                // db.SaveChanges();
                unitofwork.EvenementRepository.UpdateEvenement(evenement);
                unitofwork.Save();
                return RedirectToAction("Index");
            }
            return View(evenement);
        }

        // GET: /Evenement/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Evenement evenement = db.Evenements.Find(id);
            Evenement evenement = unitofwork.EvenementRepository.ObtenirEvenementParID(id);
            if (evenement == null)
            {
                return HttpNotFound();
            }
            return View(evenement);
        }

        // POST: /Evenement/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            //Evenement evenement = db.Evenements.Find(id);
            Evenement evenement = unitofwork.EvenementRepository.ObtenirEvenementParID(id);
            //db.Evenements.Remove(evenement);
            //db.SaveChanges();
            unitofwork.EvenementRepository.DeleteEvenement(evenement);
            unitofwork.Save();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                //db.Dispose();
                unitofwork.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
