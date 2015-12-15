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
        private UnitOfWork unitOfWork = new UnitOfWork();

        // GET: /Evenement/
        public ActionResult Index(string currentFilter, string searchNomEvenementAutre, string trieEvenementAutre)
        {
            ViewBag.searchsearchTypeEvenementAutre = new SelectList(unitOfWork.EvenementRepository.ObtenirEvenementParType(Evenement.TypeEvent.TypeAutre).ToList(), "Nom", "Nom", string.Empty);
            List<Evenement> lstEvenementAutreApresTrie = new List<Evenement>();
            if (trieEvenementAutre == null)// On trie selon les parametre
            {
                if (searchNomEvenementAutre == null)
                {
                    searchNomEvenementAutre = "";
                }

                //Trie selon les parametre de recherche entre par l'utilisateur
                List<Evenement> colEvenementAutre = unitOfWork.EvenementRepository.ObtenirEvenementParType(Evenement.TypeEvent.TypeAutre).ToList();
                List<Evenement> colEvenementAutreApresRechercheNomAutre = colEvenementAutre.Where(u => u.Nom.ToLower().Contains(searchNomEvenementAutre.ToLower())).ToList();

                lstEvenementAutreApresTrie = colEvenementAutreApresRechercheNomAutre;
            }

            return View(lstEvenementAutreApresTrie);
            //return View(db.Evenements.ToList());
            //return View(unitofwork.EvenementRepository.ObtenirEvenementParType(Evenement.TypeEvent.TypeAutre).ToList());
        }

        // GET: /Evenement/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Evenement evenement = db.Evenements.Find(id);
            //Evenement evenement = unitofwork.EvenementRepository.ObtenirEvenementParID(id);
            Evenement evenement = unitOfWork.EvenementRepository.ObtenirEvenements().Where(e => e.Id.Equals(id)).FirstOrDefault();
            if (evenement == null)
            {
                return HttpNotFound();
            }
            return View(evenement);
        }

        // GET: /Evenement/Create
        public ActionResult Create()
        {
            ViewBag.Congres = new SelectList(unitOfWork.CongresRepository.ObtenirCongres(), "Id", "Nom");
            return View();
        }

        // POST: /Evenement/Create
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Nom,Description,Salle,Actif")] Evenement evenement, Salle salle, int Congres, string DateConference, string HeureDebut, string HeureFin)
        {
            DateTime dateEvenement;
            int heureDebut;
            int heureFin;
            ViewBag.Congres = new SelectList(unitOfWork.CongresRepository.ObtenirCongres(), "Id", "Nom");
            try
            {
                dateEvenement = DateTime.Parse(DateConference);
                heureDebut = int.Parse(HeureDebut);
                heureFin = int.Parse(HeureFin);
            }
            catch (Exception e)
            {
                TempData["message"] = "La date de l'événement doit être une date valide sous le format AAAA-MM-JJ. L'heure de début et l'heure de fin doivent être des chiffres";
                return View(evenement);
            }
            if (ModelState.IsValid)
            {
                //db.Evenements.Add(evenement);
                //db.SaveChanges();
                evenement.TypeEvenement = Evenement.TypeEvent.TypeAutre;
                evenement.Actif = true;
                evenement.Salle = salle;
                Congres congres = unitOfWork.CongresRepository.ObtenirCongres().Where(u => u.Id.Equals(Congres)).FirstOrDefault();
                evenement.Congres = congres;

                unitOfWork.EvenementRepository.Insert(evenement);
                unitOfWork.Save();


                PlageHoraire newPlageHoraire = new PlageHoraire();
                DateTime dateEtHeureDebut = dateEvenement.AddHours(heureDebut);
                DateTime dateEtHeureFin = dateEvenement.AddHours(heureFin);
                newPlageHoraire.DateEtHeureDebut = dateEtHeureDebut;
                newPlageHoraire.DateEtHeureFin = dateEtHeureFin;
                newPlageHoraire.Evenement = evenement;
                newPlageHoraire.Congres = congres;
                unitOfWork.PlageHoraireRepository.InsertPlageHoraire(newPlageHoraire);
                unitOfWork.Save();

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
            //Evenement evenement = unitofwork.EvenementRepository.ObtenirEvenementParID(id);
            Evenement evenement = unitOfWork.EvenementRepository.ObtenirEvenements().Where(e => e.Id.Equals(id)).FirstOrDefault();
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
        [Authorize(Roles = "administrateur")]
        public ActionResult Edit([Bind(Include = "Id,Nom,Description,TypeEvenement,Salle,Actif")] Evenement evenement, Salle salle)
        {
            if (ModelState.IsValid)
            {
                //db.Entry(evenement).State = EntityState.Modified;
                // db.SaveChanges();
                evenement.TypeEvenement = Evenement.TypeEvent.TypeAutre;
                evenement.Salle = salle;
                unitOfWork.EvenementRepository.UpdateEvenement(evenement);
                unitOfWork.Save();
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
            Evenement evenement = unitOfWork.EvenementRepository.ObtenirEvenementParID(id);
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
            //Evenement evenement = unitofwork.EvenementRepository.ObtenirEvenementParID(id);
            Evenement evenement = unitOfWork.EvenementRepository.ObtenirEvenements().Where(e => e.Id.Equals(id)).FirstOrDefault();
            //db.Evenements.Remove(evenement);
            //db.SaveChanges();
            //unitofwork.EvenementRepository.DeleteEvenement(evenement);
            evenement.Actif = false;
            unitOfWork.EvenementRepository.UpdateEvenement(evenement);
            unitOfWork.Save();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                //db.Dispose();
                unitOfWork.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
