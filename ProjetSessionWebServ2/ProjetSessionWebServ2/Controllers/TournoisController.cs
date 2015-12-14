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
using ProjetSessionWebServ2.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace ProjetSessionWebServ2.Controllers
{
    [Authorize]
    public class TournoisController : Controller
    {
        //private ApplicationDbContext db = new ApplicationDbContext();
        private UnitOfWork unitOfWork = new UnitOfWork();

        // GET: Tournois
        public ActionResult Index(string currentFilter, string searchTypeTournoi, string searchNomTournoi, string searchParticant, string trieTournoi)
        {
            ViewBag.searchTypeTournoi = new SelectList(unitOfWork.TypeTournoiRepository.ObtenirTypeTournois(), "Nom", "Nom", string.Empty);
            List<Tournoi> lstTournoiApresTrie = new List<Tournoi>();
            if (trieTournoi == null)// On trie selon les parametre
            {
                if (searchParticant == null)
                {
                    searchParticant = "";
                }
                if (searchNomTournoi == null)
                {
                    searchNomTournoi = "";
                }
                if (searchTypeTournoi == null)
                {
                    searchTypeTournoi = "";
                }

                //Trie selon les parametre de recherche entre par l'utilisateur
                List<Tournoi> colTournoi = unitOfWork.TournoiRepository.ObtenirTournois().ToList();
                List<Tournoi> colTournoiApresrechecheType = colTournoi.Where(u => u.TypeTournoi.Nom.Contains(searchTypeTournoi)).ToList();
                List<Tournoi> colTournoiApresRechercheNomTournoi = colTournoiApresrechecheType.Where(u => u.Nom.ToLower().Contains(searchNomTournoi.ToLower())).ToList();


                lstTournoiApresTrie = colTournoiApresRechercheNomTournoi;
            }
            else // On trie par type de tournoi
            {
                lstTournoiApresTrie = unitOfWork.TournoiRepository.ObtenirTournois().OrderBy(x => x.TypeTournoi.Nom).ToList();
            }

            return View(lstTournoiApresTrie);
        }
        
        // GET: Tournois/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Tournoi tournoi = db.Evenements.Find(id);
            Tournoi tournoi = unitOfWork.TournoiRepository.ObtenirTournois().Where(t => t.Id == id).SingleOrDefault();
            if (tournoi == null)
            {
                return HttpNotFound();
            }
            return View(tournoi);
        }

        [CustomUserAttribute(Roles = "administrateur", AccessLevel = "Create")]
        // GET: Tournois/Create
        public ActionResult Create()
        {
            ViewBag.Congres = new SelectList(unitOfWork.CongresRepository.ObtenirCongres(), "Id", "Nom");
            SelectList TypeTournoiId = new SelectList(unitOfWork.TypeTournoiRepository.ObtenirTypeTournois(), "Id", "Nom");
            ViewBag.TypeTournoiId = TypeTournoiId;
            return View();
        }

        // POST: Tournois/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]

       // public ActionResult Create([Bind(Include = "Id,Nom,Description,TypeEvenement,TypeTournoiId,Actif")] TournoiVM tournoiVM, int Congres)

        public ActionResult Create([Bind(Include = "Id,Nom,Description,TypeEvenement,TypeTournoiId,Actif")] Tournoi tournoi,FormCollection collection)
        {
            DateTime dateTournoi;
            int heureDebut;
            int heureFin;
            ViewBag.Congres = new SelectList(unitOfWork.CongresRepository.ObtenirCongres(), "Id", "Nom");
            SelectList TypeTournoiId = new SelectList(unitOfWork.TypeTournoiRepository.ObtenirTypeTournois(), "Id", "Nom", tournoi.TypeTournoiId);
            try
            {
                dateTournoi = DateTime.Parse(collection["DateTournoi"]);
                heureDebut = int.Parse(collection["HeureDebut"]);
                heureFin = int.Parse(collection["HeureFin"]);
            }
            catch (Exception e)
            {
                TempData["message"] = "La date de l'événement doit être une date valide sous le format AAAA-MM-JJ. L'heure de début et l'heure de fin doivent être des chiffres";
                ViewBag.TypeTournoiId = TypeTournoiId;
                return View(tournoi);
            }
            if (ModelState.IsValid)
            {
                int congresId = int.Parse(collection["Congres"]);
                Congres congres = unitOfWork.CongresRepository.ObtenirCongres().Where(u => u.Id == congresId).FirstOrDefault();
                   
                tournoi.TypeTournoi = unitOfWork.TypeTournoiRepository.ObtenirTypeTournoiParID(tournoi.TypeTournoiId);
                tournoi.TypeEvenement = Evenement.TypeEvent.TypeTournoi;
                tournoi.Actif = true;

                tournoi.Equipes = new List<Equipe>();
                tournoi.Avancements = new List<EquipeAvancement>();
                tournoi.Parties = new List<Partie>();
                tournoi.Congres = congres;
                unitOfWork.TournoiRepository.InsertTournoi(tournoi);
                unitOfWork.Save();

                //Creating the plage horaire
                PlageHoraire newPlageHoraire = new PlageHoraire();
                DateTime dateEtHeureDebut = dateTournoi.AddHours(heureDebut);
                DateTime dateEtHeureFin = dateTournoi.AddHours(heureFin);
                newPlageHoraire.DateEtHeureDebut = dateEtHeureDebut;
                newPlageHoraire.DateEtHeureFin = dateEtHeureFin;
                newPlageHoraire.Evenement = tournoi;
                newPlageHoraire.Congres = congres;
                unitOfWork.PlageHoraireRepository.InsertPlageHoraire(newPlageHoraire);
                unitOfWork.Save();


                return RedirectToAction("Index");
            }

            ViewBag.TypeTournoiId = TypeTournoiId;

            return View(tournoi);
        }

        [CustomUserAttribute(Roles = "administrateur", AccessLevel = "Edit")]
        // GET: Tournois/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Tournoi tournoi = db.Evenements.Find(id);
            Tournoi tournoi = unitOfWork.TournoiRepository.ObtenirTournoiParID(id);
            if (tournoi == null)
            {
                return HttpNotFound();
            }

            SelectList TypeTournoiId = new SelectList(unitOfWork.TypeTournoiRepository.ObtenirTypeTournois(), "Id", "Nom", tournoi.TypeTournoiId);
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
                tournoi.TypeTournoi = unitOfWork.TypeTournoiRepository.ObtenirTypeTournoiParID(tournoi.TypeTournoiId);
                tournoi.TypeEvenement = Evenement.TypeEvent.TypeTournoi;
                unitOfWork.TournoiRepository.UpdateTournoi(tournoi);
                unitOfWork.Save();
                return RedirectToAction("Index");
            }

            SelectList TypeTournoiId = new SelectList(unitOfWork.TypeTournoiRepository.ObtenirTypeTournois(), "Id", "Nom", tournoi.TypeTournoiId);
            ViewBag.TypeTournoiId = TypeTournoiId;

            return View(tournoi);
        }

        [CustomUserAttribute(Roles = "administrateur", AccessLevel = "Delete")]
        // GET: Tournois/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Tournoi tournoi = db.Evenements.Find(id);
            Tournoi tournoi = unitOfWork.TournoiRepository.ObtenirTournoiParID(id);
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
            
            Tournoi tournoi = unitOfWork.TournoiRepository.ObtenirTournoiParID(id);
            tournoi.Actif = false;
            unitOfWork.TournoiRepository.UpdateTournoi(tournoi);
            unitOfWork.Save();
            //db.Evenements.Remove(tournoi);
            //db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Participate(int? tournamentId, int? teamid)
        {
            if (tournamentId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tournoi tournoi = unitOfWork.TournoiRepository.ObtenirTournoiCompletParId(tournamentId);
            if (tournoi == null)
            {
                return HttpNotFound();
            }

            Tournoi_Equipe model = new Tournoi_Equipe();
            model.tournoi = tournoi;


            if(teamid != null)
            {
                Equipe team = tournoi.Equipes.Find(t => t.Id == teamid);

                Equipe realTeam = unitOfWork.EquipeRepository.ObtenirEquipeCompletParId(team.Id);

                model.equipe = realTeam;
            }

            return View(model);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Participate(FormCollection formCollection)
        {
            Tournoi tourn = unitOfWork.TournoiRepository.ObtenirTournoiCompletParId(int.Parse(formCollection["tournamentid"]));

            if(formCollection["teamid"].Equals("-1"))
            {
                Equipe team = new Equipe();
                team.Nom = formCollection["teamname"];

                string id = User.Identity.GetUserId();

                if (id != null)
                {
                    //TODO: Get current user. Might need a UserRepository or something.
                    //UserManager<ApplicationUser> UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(uow.));
                    //ApplicationUser user = UserManager.FindById(User.Identity.GetUserId());
                    //ApplicationUser user = db.Users.Include(u => u.Cart).Where(u => u.Id.Equals(id)).FirstOrDefault();
                }
                //team.Joueurs.Add();

                team.Joueurs = new List<ApplicationUser>();

                tourn.Equipes.Add(team);

                unitOfWork.TournoiRepository.UpdateTournoi(tourn);
                unitOfWork.Save();
            }
            else
            {
                Equipe team = unitOfWork.EquipeRepository.ObtenirEquipeCompletParId(int.Parse(formCollection["teamid"]));

                // TODO: Add player to team.

                unitOfWork.TournoiRepository.UpdateTournoi(tourn);
                unitOfWork.Save();
            }
            
            

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
