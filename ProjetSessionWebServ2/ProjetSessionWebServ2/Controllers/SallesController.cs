﻿using System;
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
    [Authorize]
    public class SallesController : Controller
    {
        //private ApplicationDbContext db = new ApplicationDbContext();
        private UnitOfWork uow = new UnitOfWork();

        // GET: Salles
        public ActionResult Index()
        {
            //return View(db.Salles.ToList());
            return View(uow.SalleRepository.ObtenirSalles().ToList());
        }

        // GET: Salles/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Salle salle = db.Salles.Find(id);
            Salle salle = uow.SalleRepository.ObtenirSalleParID(id);
            if (salle == null)
            {
                return HttpNotFound();
            }
            return View(salle);
        }
        [CustomUserAttribute(Roles = "administrateur", AccessLevel = "Create")]
        // GET: Salles/Create
        public ActionResult Create()
        {
            //SelectList TailleSalle = new SelectList(, "Id", "Nom");
            //ViewBag.TypeKiosqueId = TypeKiosqueId;

            return View();
        }

        // POST: Salles/Create
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,NoSalle,TailleSalle,lstSalle")] Salle salle, Dimension Dimension, int? lstSalle)
        {
            if (ModelState.IsValid)
            {
                //db.Salles.Add(salle);
                //db.SaveChanges();
                salle.Dimension = Dimension;
                uow.SalleRepository.InsertSalle(salle);
                uow.Save();
                return RedirectToAction("Index");
            }

            return View(salle);
        }
        [CustomUserAttribute(Roles = "administrateur", AccessLevel = "Edit")]
        // GET: Salles/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Salle salle = db.Salles.Find(id);
            Salle salle = uow.SalleRepository.ObtenirSalleParID(id);
            if (salle == null)
            {
                return HttpNotFound();
            }
            return View(salle);
        }

        // POST: Salles/Edit/5
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,NoSalle")] Salle salle, Dimension Dimension, Section Section, Taille taille)
        {
            if (ModelState.IsValid)
            {
                //db.Entry(salle).State = EntityState.Modified;
                //db.SaveChanges();
                uow.SalleRepository.UpdateSalle(salle);
                uow.Save();
                return RedirectToAction("Index");
            }
            return View(salle);
        }
        [CustomUserAttribute(Roles = "administrateur", AccessLevel = "Delete")]
        // GET: Salles/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Salle salle = db.Salles.Find(id);
            Salle salle = uow.SalleRepository.ObtenirSalleParID(id);
            if (salle == null)
            {
                return HttpNotFound();
            }
            return View(salle);
        }

        // POST: Salles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            //Salle salle = db.Salles.Find(id);
            Salle salle = uow.SalleRepository.ObtenirSalleParID(id);
            //db.Salles.Remove(salle);
            //db.SaveChanges();
            uow.SalleRepository.DeleteSalle(salle);
            uow.Save();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                //db.Dispose();
                uow.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
