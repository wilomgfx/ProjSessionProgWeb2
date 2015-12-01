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
        public ActionResult Index()
        {
            List<Spectacle> listeSpectacles = unitOfWork.SpectacleRepository.ObtenirSpectacles().ToList();
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
            SelectList TypeSpectacleId = new SelectList(unitOfWork.TypeSpectacleRepository.ObtenirTypeSpectacles(), "Id", "Nom");
            ViewBag.TypeSpectacleId = TypeSpectacleId;
            return View();
        }

        // POST: Spectacles/Create
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Nom,Description,TypeSpectacleId,Actif")] Spectacle spectacle)
        {
            if (ModelState.IsValid)
            {
                spectacle.TypeEvenement = Evenement.TypeEvent.TypeSpectacle;
                spectacle.TypeSpectacle = unitOfWork.TypeSpectacleRepository.ObtenirTypeSpectacleParID(spectacle.TypeSpectacleId);
                spectacle.Actif = true;
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
