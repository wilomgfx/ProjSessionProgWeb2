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
    public class SpectaclesController : Controller
    {
        private UnitOfWork unitOfWork = new UnitOfWork();

        // GET: Spectacles
        public ActionResult Index(FormCollection collection)
        {
            List<Spectacle> listeSpectacles = unitOfWork.SpectacleRepository.ObtenirSpectacles().ToList();
            ViewBag.TypeSpectacles = new SelectList(unitOfWork.TypeSpectacleRepository.ObtenirTypeSpectacles(), "Nom", "Nom", string.Empty);
            if (collection.HasKeys())
            {
                string type = collection[0];
                string nom = collection[1];
                string musicien = collection[2];

                List<Spectacle> colSpectacle = unitOfWork.SpectacleRepository.ObtenirSpectacles().ToList();
                List<Spectacle> colSpectacleApresrechecheType = colSpectacle.Where(s => s.TypeSpectacle.Nom.Contains(type)).ToList();
                List<Spectacle> colSpectacleApresRechercheNomSpectacle = colSpectacleApresrechecheType.Where(s => s.Nom.Contains(nom)).ToList(); //A changer une fois les rôles implenté List<Spectacle> colConferenceApresRechercheConferencier = colSpectacleApresRechercheNomConference;

                return View(colSpectacleApresRechercheNomSpectacle);
            }
            return View(listeSpectacles);
        }

        // GET: Spectacles/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Spectacle spectacle = unitOfWork.SpectacleRepository.ObtenirSpectacles().Where(s => s.Id == id).SingleOrDefault();

            if (spectacle == null)
            {
                return HttpNotFound();
            }
            return View(spectacle);
        }

        // GET: Spectacles/Create
        public ActionResult Create()
        {
            ViewBag.Congres = new SelectList(unitOfWork.CongresRepository.ObtenirCongres(), "Id", "Nom");
            SelectList TypeSpectacleId = new SelectList(unitOfWork.TypeSpectacleRepository.ObtenirTypeSpectacles(), "Id", "Nom");
            ViewBag.TypeSpectacleId = TypeSpectacleId;
            return View();
        }

        // POST: Spectacles/Create
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Nom,Description,TypeSpectacleId,Actif")] Spectacle spectacle, int Congres)
        {
            if (ModelState.IsValid)
            {
                spectacle.TypeEvenement = Evenement.TypeEvent.TypeSpectacle;
                spectacle.TypeSpectacle = unitOfWork.TypeSpectacleRepository.ObtenirTypeSpectacleParID(spectacle.TypeSpectacleId);
                spectacle.Actif = true;

                Congres congres = unitOfWork.CongresRepository.ObtenirCongres().Where(u => u.Id.Equals(Congres)).FirstOrDefault();
                spectacle.Congres = congres;

                unitOfWork.SpectacleRepository.InsertSpectacle(spectacle);
                unitOfWork.Save();
                return RedirectToAction("Index");
            }
            SelectList TypeSpectacleId = new SelectList(unitOfWork.TypeSpectacleRepository.ObtenirTypeSpectacles(), "Id", "Nom", spectacle.TypeSpectacleId);
            ViewBag.TypeSpectacleId = TypeSpectacleId;
            return View(spectacle);
        }

        // GET: Spectacles/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Spectacle spectacle = unitOfWork.SpectacleRepository.ObtenirSpectacleParID(id);
            if (spectacle == null)
            {
                return HttpNotFound();
            }
            SelectList TypeSpectacleId = new SelectList(unitOfWork.TypeSpectacleRepository.ObtenirTypeSpectacles(), "Id", "Nom", spectacle.TypeSpectacleId);
            ViewBag.TypeSpectacleId = TypeSpectacleId;
            return View(spectacle);
        }

        // POST: Spectacles/Edit/5
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Nom,Description,TypeSpectacleId,Actif")] Spectacle spectacle)
        {
            if (ModelState.IsValid)
            {
                spectacle.TypeSpectacle = unitOfWork.TypeSpectacleRepository.ObtenirTypeSpectacleParID(spectacle.TypeSpectacleId);
                spectacle.TypeEvenement = Evenement.TypeEvent.TypeSpectacle;
                unitOfWork.SpectacleRepository.UpdateSpectacle(spectacle);
                unitOfWork.Save();
                return RedirectToAction("Index");
            }
            SelectList TypeSpectacleId = new SelectList(unitOfWork.TypeSpectacleRepository.ObtenirTypeSpectacles(), "Id", "Nom", spectacle.TypeSpectacleId);
            ViewBag.TypeSpectacleId = TypeSpectacleId;
            return View(spectacle);
        }

        // GET: Spectacles/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Spectacle spectacle = unitOfWork.SpectacleRepository.ObtenirSpectacleParID(id);
            if (spectacle == null)
            {
                return HttpNotFound();
            }
            return View(spectacle);
        }

        // POST: Spectacles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Spectacle spectacle = unitOfWork.SpectacleRepository.ObtenirSpectacleParID(id);
            spectacle.Actif = false;
            unitOfWork.SpectacleRepository.UpdateSpectacle(spectacle);
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
    }
}
