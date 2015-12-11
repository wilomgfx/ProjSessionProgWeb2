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
using System.Globalization;
using System.Threading;

namespace ProjetSessionWebServ2.Controllers
{
    public class CongresController : Controller
    {
        private UnitOfWork unitOfWork = new UnitOfWork();

        // GET: Congres
        public ActionResult Index()
        {
            return View(unitOfWork.CongresRepository.ObtenirCongres().ToList());
        }

        public ActionResult HoraireCongres(int? id)
        {
            List<PlageHoraire> plageHoraires = unitOfWork.PlageHoraireRepository.Get(c => c.Congres.Id == id).ToList();
            return View(plageHoraires);
        }

        // GET: Congres/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Congres congres = unitOfWork.CongresRepository.ObtenirCongresParID(id);
            
            ViewBag.ListeConference = unitOfWork.ConferenceRepository.ObtenirConferences().Where(u => u.Congres.Id.Equals(id)).ToList();// && u.Actif.Equals(true)).ToList();
            ViewBag.ListeSpectacle = unitOfWork.SpectacleRepository.ObtenirSpectacles().Where(u => u.Congres.Id.Equals(id)).ToList();// && u.Actif.Equals(true)).ToList();
            ViewBag.ListeKiosque = unitOfWork.KiosqueRepository.ObtenirKiosques().Where(u => u.Congres.Id.Equals(id)).ToList();// && u.Actif.Equals(true)).ToList();
            ViewBag.ListeAutre = unitOfWork.EvenementRepository.ObtenirEvenements().Where(u => u.Congres.Id.Equals(id) && u.TypeEvenement.Equals(ProjetSessionWebServ2.Models.Evenement.TypeEvent.TypeAutre)).ToList();// && u.Actif.Equals(true)
            ViewBag.ListeTournoi = unitOfWork.TournoiRepository.ObtenirTournois().Where(u => u.Congres.Id.Equals(id)).ToList(); // && u.Actif.Equals(true)).ToList();
            

            if (congres == null)
            {
                return HttpNotFound();
            }
            return View(congres);
        }

        // GET: Congres/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Congres/Create
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Description,Adresse,Nom,DateDebut,DateFin,Actif")] Congres congres)
        {
            if (ModelState.IsValid)
            {
                congres.Actif = true;
                unitOfWork.CongresRepository.InsertCongres(congres);
                unitOfWork.Save();
                return RedirectToAction("Index");
            }

            return View(congres);
        }

        // GET: Congres/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Congres congres = unitOfWork.CongresRepository.ObtenirCongresParID(id);
            if (congres == null)
            {
                return HttpNotFound();
            }
            return View(congres);
        }

        // POST: Congres/Edit/5
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Description,Adresse,Nom,DateDebut,DateFin,Actif")] Congres congres)
        {
            if (ModelState.IsValid)
            {
                unitOfWork.CongresRepository.UpdateCongres(congres);
                unitOfWork.Save();
                return RedirectToAction("Index");
            }
            return View(congres);
        }

        // GET: Congres/Delete/5

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Congres congres = unitOfWork.CongresRepository.ObtenirCongresParID(id);
            if (congres == null)
            {
                return HttpNotFound();
            }
            return View(congres);
        }
        [Authorize(Roles = "administrateur")]
        public ActionResult RapportDesVentes()
        {      
            return View(unitOfWork.TransactionRepository.ObtenirTransactions());
        }
        [Authorize(Roles = "administrateur")]
        public ActionResult AdminPanel()
        {
            return View();
        }



        // POST: Congres/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Congres congres = unitOfWork.CongresRepository.ObtenirCongresParID(id);
            congres.Actif = false;
            unitOfWork.CongresRepository.UpdateCongres(congres);
            unitOfWork.Save();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                unitOfWork.Dispose();
            }
            base.Dispose(disposing);
        }


        public ActionResult ChangerLangue(string langue)
        {
            Session["Culture"] = new CultureInfo(langue);            

            return new EmptyResult();
           // return RedirectToAction("Index");
        }
    }
}
