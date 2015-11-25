using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ProjetSessionWebServ2.Models;
using GestionPhotoImmobilier.DAL;

namespace ProjetSessionWebServ2.Controllers
{
    public class ConferencesController : Controller
    {
       // private ApplicationDbContext db = new ApplicationDbContext()Default1;
        private UnitOfWork unitOfWork = new UnitOfWork();
        // GET: Conferences
        public ActionResult Index()
        {
            return View(unitOfWork.ConferenceRepository.ObtenirConference());
            //return View(unitOfWork.ConferenceRepository.ObtenirConference().Where(t=>t.Actif == true));
            //return View(db.Evenements.ToList());
        }

        // GET: Conferences/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Conference conference = unitOfWork.ConferenceRepository.ObtenirConferenceParID(id);
           
            if (conference == null)
            {
                return HttpNotFound();
            }
            return View(conference);
        }

        // GET: Conferences/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Conferences/Create
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Nom,Description,TypeEvenement")] Conference conference)
        {
            if (ModelState.IsValid)
            {
                conference.Actif = true;
                unitOfWork.ConferenceRepository.InsertConference(conference);
                unitOfWork.Save();
                //db.Evenements.Add(conference);
                //db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(conference);
        }

        // GET: Conferences/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Conference conference = unitOfWork.ConferenceRepository.ObtenirConferenceParID(id);
            if (conference == null)
            {
                return HttpNotFound();
            }
            return View(conference);
        }

        // POST: Conferences/Edit/5
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Nom,Description,TypeEvenement")] Conference conference)
        {
            if (ModelState.IsValid)
            {
                unitOfWork.ConferenceRepository.UpdateConference(conference);
                unitOfWork.Save();
                //db.Entry(conference).State = EntityState.Modified;
                //db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(conference);
        }

        // GET: Conferences/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Conference conference = unitOfWork.ConferenceRepository.ObtenirConferenceParID(id);
            if (conference == null)
            {
                return HttpNotFound();
            }
            return View(conference);
        }

        // POST: Conferences/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Conference conference = unitOfWork.ConferenceRepository.ObtenirConferenceParID(id);
            conference.Actif = false;
            unitOfWork.ConferenceRepository.UpdateConference(conference);
            //unitOfWork.ConferenceRepository.DeleteConference(conference);
            unitOfWork.Save();
            //db.Evenements.Remove(conference);
            //db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                unitOfWork.Dispose();
               // db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
