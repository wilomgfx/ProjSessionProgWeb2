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
    public class TournoisController : Controller
    {
        //private ApplicationDbContext db = new ApplicationDbContext();
        private UnitOfWork uow = new UnitOfWork();

        // GET: Tournois
        public ActionResult Index()
        {
            return View(uow.TournoiRepository.ObtenirTournois());
        }
        [HttpPost]
        public ActionResult SearchTournoi(FormCollection collection)
        {
            string nameToSearch = collection["search"];

            IEnumerable<Tournoi> lstTournoi = uow.TournoiRepository.ObtenirTournoiParNom(nameToSearch);

            return View("Index", lstTournoi);
        }

        // GET: Tournois/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Tournoi tournoi = db.Evenements.Find(id);
            Tournoi tournoi = uow.TournoiRepository.ObtenirTournoiParID(id);
            if (tournoi == null)
            {
                return HttpNotFound();
            }
            return View(tournoi);
        }

        // GET: Tournois/Create
        public ActionResult Create()
        {
            SelectList TypeTournoiId = new SelectList(uow.TypeTournoiRepository.ObtenirTypeTournois(), "Id", "Nom");
            ViewBag.TypeTournoiId = TypeTournoiId;
            return View();
        }

        // POST: Tournois/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Nom,Description,TypeEvenement,TypeTournoiId,Actif")] Tournoi tournoi)
        {

            if (ModelState.IsValid)
            {
                tournoi.TypeTournoi = uow.TypeTournoiRepository.ObtenirTypeTournoiParID(tournoi.TypeTournoiId);
                tournoi.TypeEvenement = Evenement.TypeEvent.TypeKiosque;
                tournoi.Actif = true;
                uow.TournoiRepository.InsertTournoi(tournoi);
                uow.Save();
                return RedirectToAction("Index");
            }

            SelectList TypeTournoiId = new SelectList(uow.TypeTournoiRepository.ObtenirTypeTournois(), "Id", "Nom");
            ViewBag.TypeTournoiId = TypeTournoiId;

            return View(tournoi);
        }

        // GET: Tournois/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Tournoi tournoi = db.Evenements.Find(id);
            Tournoi tournoi = uow.TournoiRepository.ObtenirTournoiParID(id);
            if (tournoi == null)
            {
                return HttpNotFound();
            }

            SelectList TypeTournoiId = new SelectList(uow.TypeTournoiRepository.ObtenirTypeTournois(), "Id", "Nom");
            ViewBag.TypeTournoiId = TypeTournoiId;

            return View(tournoi);
        }

        // POST: Tournois/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Nom,Description,TypeEvenement,TypeTournoiId,Actif")] Tournoi tournoi)
        {
            if (ModelState.IsValid)
            {
                uow.TournoiRepository.UpdateTournoi(tournoi);
                uow.Save();
                return RedirectToAction("Index");
            }

            SelectList TypeTournoiId = new SelectList(uow.TypeTournoiRepository.ObtenirTypeTournois(), "Id", "Nom");
            ViewBag.TypeTournoiId = TypeTournoiId;

            return View(tournoi);
        }

        // GET: Tournois/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Tournoi tournoi = db.Evenements.Find(id);
            Tournoi tournoi = uow.TournoiRepository.ObtenirTournoiParID(id);
            if (tournoi == null)
            {
                return HttpNotFound();
            }
            return View(tournoi);
        }

        // POST: Tournois/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            
            Tournoi tournoi = uow.TournoiRepository.ObtenirTournoiParID(id);
            tournoi.Actif = false;
            uow.TournoiRepository.UpdateTournoi(tournoi);
            uow.Save();
            //db.Evenements.Remove(tournoi);
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
