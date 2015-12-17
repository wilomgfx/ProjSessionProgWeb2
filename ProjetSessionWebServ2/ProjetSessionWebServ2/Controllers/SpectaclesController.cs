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
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace ProjetSessionWebServ2.Controllers
{
    [Authorize]
    public class SpectaclesController : Controller
    {
        private UnitOfWork unitOfWork = new UnitOfWork();

        // GET: Spectacles
        public ActionResult Index(FormCollection collection,string trieSpectacle)
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
            if (trieSpectacle != null)
            {
                listeSpectacles = unitOfWork.SpectacleRepository.ObtenirSpectacles().OrderBy(x => x.TypeSpectacle.Nom).ToList();
            }
            else
            {
                trieSpectacle = "";
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
        [CustomUserAttribute(Roles = "musicien,administrateur", AccessLevel = "Create")]
        public ActionResult Create()
        {
            ViewBag.Congres = new SelectList(unitOfWork.CongresRepository.ObtenirCongres(), "Id", "Nom");
            SelectList TypeSpectacleId = new SelectList(unitOfWork.TypeSpectacleRepository.ObtenirTypeSpectacles(), "Id", "Nom");
            ViewBag.TypeSpectacleId = TypeSpectacleId;
            SelectList SalleCongres = new SelectList(unitOfWork.SalleRepository.ObtenirSalles(), "Id", "NoSalle");
            ViewBag.lstSalle = SalleCongres;
            return View();
        }

        // POST: Spectacles/Create
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Nom,Description,TypeSpectacleId,Actif,lstSalle")] Spectacle spectacle, int Congres, string DateSpectacle, string HeureDebut, string HeureFin,int? lstSalle)
        {
            DateTime dateSpectacle;
            int heureDebut;
            int heureFin;
            SelectList TypeSpectacleId = new SelectList(unitOfWork.TypeSpectacleRepository.ObtenirTypeSpectacles(), "Id", "Nom", spectacle.TypeSpectacleId);
            ViewBag.Congres = new SelectList(unitOfWork.CongresRepository.ObtenirCongres(), "Id", "Nom");
            try
            {
                dateSpectacle = DateTime.Parse(DateSpectacle);
                heureDebut = int.Parse(HeureDebut);
                heureFin = int.Parse(HeureFin);
            }
            catch(Exception e)
            {
                TempData["message"] = "La date de spectacle doit être une date valide sous le format AAAA-MM-JJ. L'heure de début et l'heure de fin doivent être des chiffres";
                ViewBag.TypeSpectacleId = TypeSpectacleId;
                return View(spectacle);
            }
            if (ModelState.IsValid)
            {
                spectacle.TypeEvenement = Evenement.TypeEvent.TypeSpectacle;
                spectacle.TypeSpectacle = unitOfWork.TypeSpectacleRepository.ObtenirTypeSpectacleParID(spectacle.TypeSpectacleId);
                spectacle.Actif = true;


                Congres congres = unitOfWork.CongresRepository.ObtenirCongres().Where(u => u.Id.Equals(Congres)).FirstOrDefault();
                spectacle.Congres = congres;
                spectacle.Salle = unitOfWork.SalleRepository.ObtenirSalleParID(lstSalle);

                UserManager<ApplicationUser> UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(unitOfWork.context));
                ApplicationUser utilisateur = UserManager.FindById(User.Identity.GetUserId());
                if (spectacle.Users == null)
                {
                    spectacle.Users = new List<ApplicationUser>();
                }

                spectacle.Users.Add(utilisateur);

                unitOfWork.SpectacleRepository.InsertSpectacle(spectacle);
                unitOfWork.Save();


                PlageHoraire newPlageHoraire = new PlageHoraire();
                DateTime dateEtHeureDebut = dateSpectacle.AddHours(heureDebut);
                DateTime dateEtHeureFin = dateSpectacle.AddHours(heureFin);
                newPlageHoraire.DateEtHeureDebut = dateEtHeureDebut;
                newPlageHoraire.DateEtHeureFin = dateEtHeureFin;
                newPlageHoraire.Evenement = spectacle;
                newPlageHoraire.Congres = congres;
                unitOfWork.PlageHoraireRepository.InsertPlageHoraire(newPlageHoraire);
                unitOfWork.Save();

                Transaction nouvelleTransaction = new Transaction();
                nouvelleTransaction.DateAchat = DateTime.Now;
                nouvelleTransaction.Montant = 1000;
                nouvelleTransaction.TypeAchat = "Location pour un Spectacle";
                unitOfWork.TransactionRepository.InsertTransaction(nouvelleTransaction);
                unitOfWork.Save();
                return RedirectToAction("Index");
            }
            ViewBag.TypeSpectacleId = TypeSpectacleId;
            return View(spectacle);
        }

        // GET: Spectacles/Edit/5
        [CustomUserAttribute(Roles = "administrateur,musicien", AccessLevel = "Edit")]
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
            ViewBag.Congres = new SelectList(unitOfWork.CongresRepository.ObtenirCongres(), "Id", "Nom");
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
                //Salle salletest = unitOfWork.SalleRepository.ObtenirSalleParID(1);
                //spectacle.Salle = salletest;
                unitOfWork.Save();
                return RedirectToAction("Index");
            }
            ViewBag.Congres = new SelectList(unitOfWork.CongresRepository.ObtenirCongres(), "Id", "Nom");
            SelectList TypeSpectacleId = new SelectList(unitOfWork.TypeSpectacleRepository.ObtenirTypeSpectacles(), "Id", "Nom", spectacle.TypeSpectacleId);
            ViewBag.TypeSpectacleId = TypeSpectacleId;
            SelectList SalleCongres = new SelectList(unitOfWork.SalleRepository.ObtenirSalles(), "Id", "NoSalle");
            ViewBag.lstSalle = SalleCongres;
            return View(spectacle);
        }

        // GET: Spectacles/Delete/5
        [CustomUserAttribute(Roles = "administrateur,musicien", AccessLevel = "Delete")]
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
