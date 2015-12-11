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
                List<Conference> colConfenrenceApresRechercheNomConference = colConfenreceApresrechecheType.Where(u => u.Nom.Contains(searchNomConference)).ToList();
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
        [Authorize(Roles = "conferencier,administrateur")]
        // GET: Conferences/Create
        public ActionResult Create()
        {
            ViewBag.Congres = new SelectList(unitOfWork.CongresRepository.ObtenirCongres(), "Id", "Nom");
            SelectList TypeConferenceId = new SelectList(unitOfWork.TypeConferenceRepository.ObtenirTypeConferences(), "Id", "Nom");
            ViewBag.TypeConferenceIdViewBag = TypeConferenceId;

            return View();
        }

        // POST: Conferences/Create
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Nom,Description,TypeConferenceId")] Conference conference, int TypeConferenceIdViewBag, int Congres, string DateConference, string HeureDebut, string HeureFin)
        {

            conference.TypeEvenement = Evenement.TypeEvent.TypeConference;
            
            conference.TypeConferenceId = TypeConferenceIdViewBag;
            TypeConference typeConferenceRevenu = unitOfWork.TypeConferenceRepository.ObtenirTypeConferences().Where(u => u.Id.Equals(TypeConferenceIdViewBag)).FirstOrDefault();
            conference.TypeConference = typeConferenceRevenu;
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
                unitOfWork.ConferenceRepository.InsertConference(conference);
                
                unitOfWork.Save();



                PlageHoraire newPlageHoraire = new PlageHoraire();
                DateTime dateTournoi = DateTime.Parse(DateConference);
                int heureDebut = int.Parse(HeureDebut);
                int heureFin = int.Parse(HeureFin);
                DateTime dateEtHeureDebut = dateTournoi.AddHours(heureDebut);
                DateTime dateEtHeureFin = dateTournoi.AddHours(heureFin);
                newPlageHoraire.DateEtHeureDebut = dateEtHeureDebut;
                newPlageHoraire.DateEtHeureFin = dateEtHeureFin;
                newPlageHoraire.Evenement = conference;
                unitOfWork.PlageHoraireRepository.InsertPlageHoraire(newPlageHoraire);
                unitOfWork.Save();



                //db.Evenements.Add(conference);
                //db.SaveChanges();
                return RedirectToAction("Index");
            }
            SelectList TypeConferenceId2 = new SelectList(unitOfWork.TypeConferenceRepository.ObtenirTypeConferences(), "Id", "Nom");
            ViewBag.TypeConferenceIdViewBag = TypeConferenceId2;


           

            return View(conference);
        }
        [Authorize(Roles = "administrateur,conferencier")]
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
        public ActionResult Edit([Bind(Include = "Id,Nom,Description,TypeEvenement,TypeConferenceId, Actif")] Conference conference, int TypeConferenceIdViewBag)
        {
            TypeConference typeConferenceRevenu = unitOfWork.TypeConferenceRepository.ObtenirTypeConferences().Where(u => u.Id.Equals(TypeConferenceIdViewBag)).FirstOrDefault();
            conference.TypeConferenceId = TypeConferenceIdViewBag;
            conference.TypeConference = typeConferenceRevenu;
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
        [Authorize(Roles = "administrateur,conferencier")]
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
