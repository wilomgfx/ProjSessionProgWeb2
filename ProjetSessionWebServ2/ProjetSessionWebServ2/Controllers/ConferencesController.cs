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
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Web.Security;
using Microsoft.Owin;

namespace ProjetSessionWebServ2.Controllers
{
    [Authorize]
    public class ConferencesController : Controller
    {
        // private ApplicationDbContext db = new ApplicationDbContext()Default1;
        private UnitOfWork unitOfWork = new UnitOfWork();
        //private ApplicationDbContext context = new ApplicationDbContext();
        // GET: Conferences
        public ActionResult Index(string currentFilter, string searchTypeConference, string searchNomConference, string searchConferencier, string trieConference)
        {   
            ViewBag.searchTypeConference = new SelectList(unitOfWork.TypeConferenceRepository.ObtenirTypeConferences(), "Nom", "Nom", string.Empty);
            List<Conference> lstConferenceApresTrie = new List<Conference>();
            if (trieConference == null)// On trie selon les parametre
            {
                if (searchConferencier == null)
                {
                    searchConferencier = "";
                }
                if (searchNomConference == null)
                {
                    searchNomConference = "";
                }
                if (searchTypeConference == null)
                {
                    searchTypeConference = "";
                }

                //Trie selon les parametre de recherche entre par l'utilisateur
                List<Conference> colConference = unitOfWork.ConferenceRepository.ObtenirConferences().ToList();
                List<Conference> colConfenreceApresrechecheType = colConference.Where(u => u.TypeConference.Nom.Contains(searchTypeConference)).ToList();
                List<Conference> colConfenrenceApresRechercheNomConference = colConfenreceApresrechecheType.Where(u => u.Nom.ToLower().Contains(searchNomConference.ToLower())).ToList();
                List<Conference> colConferenceApresRechercheConferencier = new List<Conference>();


                foreach (Conference conference in colConfenrenceApresRechercheNomConference)
                {
                    foreach (ApplicationUser user in conference.Users)
                    {
                        if (user.UserName.ToLower().Contains(searchConferencier.ToLower()))
                        {
                            colConferenceApresRechercheConferencier.Add(conference);
                        }
                    }
                }
                lstConferenceApresTrie = colConferenceApresRechercheConferencier;
            }
            else // On trie par type de conference
            {
                lstConferenceApresTrie = unitOfWork.ConferenceRepository.ObtenirConferences().OrderBy(x => x.TypeConference.Nom).ToList();
            }

            return View(lstConferenceApresTrie);
            // return View(unitOfWork.ConferenceRepository.ObtenirConferences());
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
            //  Conference conference = unitOfWork.ConferenceRepository.ObtenirConferenceParID(id);
            Conference confenrence2 = unitOfWork.ConferenceRepository.ObtenirConferences().Where(u => u.Id.Equals(id)).FirstOrDefault();



            if (confenrence2 == null)
            {
                return HttpNotFound();
            }

            return View(confenrence2);
        }
        [CustomUserAttribute(Roles = "conferencier,administrateur", AccessLevel = "Create")]
        // GET: Conferences/Create
        public ActionResult Create()
        {
            ViewBag.Congres = new SelectList(unitOfWork.CongresRepository.ObtenirCongres(), "Id", "Nom");
            ViewBag.lstSalle = new SelectList(unitOfWork.SalleRepository.ObtenirSalles(), "Id", "NoSalle");
            SelectList TypeConferenceId = new SelectList(unitOfWork.TypeConferenceRepository.ObtenirTypeConferences(), "Id", "Nom");
            ViewBag.TypeConferenceIdViewBag = TypeConferenceId;
            SelectList lstSalle = new SelectList(unitOfWork.SalleRepository.ObtenirSalles(), "Id", "NoSalle");
            ViewBag.lstSalle = lstSalle;

            return View();
        }

        // POST: Conferences/Create
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Nom,Description,TypeConferenceId,lstSalle")] Conference conference, int TypeConferenceIdViewBag, int Congres, string DateConference, string HeureDebut, string HeureFin,int? lstSalle)
        {
            conference.Salle = unitOfWork.SalleRepository.ObtenirSalleParID(lstSalle);
            conference.TypeEvenement = Evenement.TypeEvent.TypeConference;
            
            conference.TypeConferenceId = TypeConferenceIdViewBag;
            TypeConference typeConferenceRevenu = unitOfWork.TypeConferenceRepository.ObtenirTypeConferences().Where(u => u.Id.Equals(TypeConferenceIdViewBag)).FirstOrDefault();
            conference.TypeConference = typeConferenceRevenu;

            DateTime dateConference;
            int heureDebut;
            int heureFin;
            SelectList TypeConferenceId2 = new SelectList(unitOfWork.TypeConferenceRepository.ObtenirTypeConferences(), "Id", "Nom");
            ViewBag.Congres = new SelectList(unitOfWork.CongresRepository.ObtenirCongres(), "Id", "Nom");
            try
            {
                dateConference = DateTime.Parse(DateConference);
                heureDebut = int.Parse(HeureDebut);
                heureFin = int.Parse(HeureFin);
            }
            catch (Exception e)
            {
                TempData["message"] = "La date de conférence doit être une date valide sous le format AAAA-MM-JJ. L'heure de début et l'heure de fin doivent être des chiffres";
                ViewBag.TypeConferenceIdViewBag = TypeConferenceId2;
                return View(conference);
            }
            if (ModelState.IsValid)
            {
                conference.Actif = true;

                //UserManager<ApplicationUser> userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

                UserManager<ApplicationUser> UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(unitOfWork.context));
                ApplicationUser utilisateur = UserManager.FindById(User.Identity.GetUserId());
                if (conference.Users == null)
                {
                    conference.Users = new List<ApplicationUser>();
                }
                Congres congres = unitOfWork.CongresRepository.ObtenirCongres().Where(u => u.Id.Equals(Congres)).FirstOrDefault();

                conference.Users.Add(utilisateur);
                conference.Congres = congres;


                PlageHoraire newPlageHoraire = new PlageHoraire();
                DateTime dateEtHeureDebut = dateConference.AddHours(heureDebut);
                DateTime dateEtHeureFin = dateConference.AddHours(heureFin);
                newPlageHoraire.DateEtHeureDebut = dateEtHeureDebut;
                newPlageHoraire.DateEtHeureFin = dateEtHeureFin;
                newPlageHoraire.Evenement = conference;
                newPlageHoraire.Congres = congres;

                List<PlageHoraire> lst = new List<PlageHoraire>();
                lst.Add(newPlageHoraire);

                if (!unitOfWork.IsRoomAvailableForTime(conference, lst))
                {
                    // Si la il y a conflit d'horaire pour une pièce....

                    TempData["message"] = "Il y a conflit d'horaire pour la salle que vous essayez de choisir. Veuillez choisir une autre salle, ou faire votre évènement à un moment différent.";
                    ViewBag.TypeConferenceIdViewBag = TypeConferenceId2;
                    return View(conference);
                }

                unitOfWork.ConferenceRepository.InsertConference(conference);
                
                unitOfWork.Save();



                
                unitOfWork.PlageHoraireRepository.InsertPlageHoraire(newPlageHoraire);
                unitOfWork.Save();

               
                Transaction nouvelleTransaction = new Transaction();
                nouvelleTransaction.DateAchat = DateTime.Now;
                nouvelleTransaction.Montant = 500;
                nouvelleTransaction.TypeAchat = "Location pour une conference";
                unitOfWork.TransactionRepository.InsertTransaction(nouvelleTransaction);
                unitOfWork.Save();
                //db.Evenements.Add(conference);
                //db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.TypeConferenceIdViewBag = TypeConferenceId2;


           

            return View(conference);
        }

        [CustomUserAttribute(Roles = "conferencier,administrateur", AccessLevel = "Edit")]
        // GET: Conferences/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }


            Conference conference = unitOfWork.ConferenceRepository.ObtenirConferenceParID(id);

            SelectList TypeConferenceId = new SelectList(unitOfWork.TypeConferenceRepository.ObtenirTypeConferences(), "Id", "Nom", conference.TypeConference.Id);
            ViewBag.TypeConferenceIdViewBag = TypeConferenceId;
            SelectList lstSalle = new SelectList(unitOfWork.SalleRepository.ObtenirSalles(), "Id", "NoSalle");
            ViewBag.lstSalle = lstSalle;
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
        [Authorize(Roles = "administrateur,conferencier")]
        public ActionResult Edit([Bind(Include = "Id,Nom,Description,TypeEvenement,TypeConferenceId, Actif,lstSalle")] Conference conference, int TypeConferenceIdViewBag, int? lstSalle)
        {
            TypeConference typeConferenceRevenu = unitOfWork.TypeConferenceRepository.ObtenirTypeConferences().Where(u => u.Id.Equals(TypeConferenceIdViewBag)).FirstOrDefault();
            conference.TypeConferenceId = TypeConferenceIdViewBag;
            conference.TypeConference = typeConferenceRevenu;
            conference.Salle = unitOfWork.SalleRepository.ObtenirSalleParID(lstSalle);
            if (ModelState.IsValid)
            {
                unitOfWork.ConferenceRepository.UpdateConference(conference);
                unitOfWork.Save();
                //db.Entry(conference).State = EntityState.Modified;
                //db.SaveChanges();
                return RedirectToAction("Index");
            }
            SelectList TypeConferenceId2 = new SelectList(unitOfWork.TypeConferenceRepository.ObtenirTypeConferences(), "Id", "Nom", conference.TypeConference.Id);
            ViewBag.TypeConferenceIdViewBag = TypeConferenceId2;
            return View(conference);
        }
        [CustomUserAttribute(Roles = "conferencier,administrateur", AccessLevel = "Delete")]
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
        [Authorize(Roles = "administrateur")]
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
