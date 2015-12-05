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
    public class KiosquesController : Controller
    {
        //private ApplicationDbContext db = new ApplicationDbContext();
        private UnitOfWork uow = new UnitOfWork();

        // GET: Kiosques
        public ActionResult Index()
        {
            var stuff = uow.KiosqueRepository.ObtenirKiosques();
            return View(stuff);
        }

        // GET: Kiosques/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Kiosque Kiosque = db.Evenements.Find(id);
            Kiosque Kiosque = uow.KiosqueRepository.ObtenirKiosques().Where(k => k.Id == id).SingleOrDefault();
            if (Kiosque == null)
            {
                return HttpNotFound();
            }
            return View(Kiosque);
        }

        // GET: Kiosques/Create
        public ActionResult Create()
        {
            SelectList TypeKiosqueId = new SelectList(uow.TypeKiosqueRepository.ObtenirTypeKiosques(), "Id", "Nom");
            ViewBag.TypeKiosqueId = TypeKiosqueId;
            return View();
        }

        // POST: Kiosques/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Nom,Description,TypeKiosqueId,Actif")] Kiosque Kiosque)
        {
            Kiosque.Actif = true;

            if (ModelState.IsValid)
            {
                //db.Evenements.Add(Kiosque);
                //db.SaveChanges();
                Kiosque.TypeKiosque = uow.TypeKiosqueRepository.ObtenirTypeKiosqueParID(Kiosque.TypeKiosqueId);
                Kiosque.TypeEvenement = Evenement.TypeEvent.TypeKiosque;
                Kiosque.Actif = true;
                uow.KiosqueRepository.InsertKiosque(Kiosque);
                uow.Save();
                return RedirectToAction("Index");
            }

            SelectList typesKiosques = new SelectList(uow.TypeKiosqueRepository.ObtenirTypeKiosques(), "Id", "Nom", Kiosque.TypeKiosqueId);
            ViewBag.TypeKiosqueId = typesKiosques;

            return View(Kiosque);
        }

        // GET: Kiosques/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Kiosque Kiosque = db.Evenements.Find(id);
            Kiosque Kiosque = uow.KiosqueRepository.ObtenirKiosqueParID(id);
            if (Kiosque == null)
            {
                return HttpNotFound();
            }

            SelectList TypeKiosqueId = new SelectList(uow.TypeKiosqueRepository.ObtenirTypeKiosques(), "Id", "Nom", Kiosque.TypeKiosqueId);
            ViewBag.TypeKiosqueId = TypeKiosqueId;

            return View(Kiosque);
        }

        // POST: Kiosques/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Nom,Description,TypeKiosqueId,Actif")] Kiosque Kiosque)
        {
            if (ModelState.IsValid)
            {
                Kiosque.TypeKiosque = uow.TypeKiosqueRepository.ObtenirTypeKiosqueParID(Kiosque.TypeKiosqueId);
                Kiosque.TypeEvenement = Evenement.TypeEvent.TypeKiosque;
                uow.KiosqueRepository.UpdateKiosque(Kiosque);
                uow.Save();
                return RedirectToAction("Index");
            }

            SelectList typesKiosques = new SelectList(uow.TypeKiosqueRepository.ObtenirTypeKiosques(), "Id", "Nom", Kiosque.TypeKiosqueId);
            ViewBag.TypeKiosqueId = typesKiosques;

            return View(Kiosque);
        }

        // GET: Kiosques/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Kiosque Kiosque = db.Evenements.Find(id);
            Kiosque Kiosque = uow.KiosqueRepository.ObtenirKiosqueParID(id);
            if (Kiosque == null)
            {
                return HttpNotFound();
            }
            return View(Kiosque);
        }

        // POST: Kiosques/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {

            Kiosque Kiosque = uow.KiosqueRepository.ObtenirKiosqueParID(id);
            Kiosque.Actif = false;
            uow.KiosqueRepository.UpdateKiosque(Kiosque);
            uow.Save();
            //db.Evenements.Remove(Kiosque);
            //db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                uow.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
