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
            Tournoi tournoi = uow.TournoiRepository.ObtenirTournois().Where(t => t.Id == id).SingleOrDefault();
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
                tournoi.TypeEvenement = Evenement.TypeEvent.TypeTournoi;
                tournoi.Actif = true;

                tournoi.Equipes = new List<Equipe>();
                tournoi.Avancements = new List<EquipeAvancement>();
                tournoi.Parties = new List<Partie>();

                uow.TournoiRepository.InsertTournoi(tournoi);
                uow.Save();
                return RedirectToAction("Index");
            }

            SelectList TypeTournoiId = new SelectList(uow.TypeTournoiRepository.ObtenirTypeTournois(), "Id", "Nom", tournoi.TypeTournoiId);
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

            SelectList TypeTournoiId = new SelectList(uow.TypeTournoiRepository.ObtenirTypeTournois(), "Id", "Nom", tournoi.TypeTournoiId);
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
                tournoi.TypeTournoi = uow.TypeTournoiRepository.ObtenirTypeTournoiParID(tournoi.TypeTournoiId);
                tournoi.TypeEvenement = Evenement.TypeEvent.TypeTournoi;
                uow.TournoiRepository.UpdateTournoi(tournoi);
                uow.Save();
                return RedirectToAction("Index");
            }

            SelectList TypeTournoiId = new SelectList(uow.TypeTournoiRepository.ObtenirTypeTournois(), "Id", "Nom", tournoi.TypeTournoiId);
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

        public ActionResult Participate(int? tournamentId, int? teamid)
        {
            if (tournamentId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tournoi tournoi = uow.TournoiRepository.ObtenirTournoiCompletParId(tournamentId);
            if (tournoi == null)
            {
                return HttpNotFound();
            }

            Tournoi_Equipe model = new Tournoi_Equipe();
            model.tournoi = tournoi;


            if(teamid != null)
            {
                Equipe team = tournoi.Equipes.Find(t => t.Id == teamid);

                Equipe realTeam = uow.EquipeRepository.ObtenirEquipeCompletParId(team.Id);

                model.equipe = realTeam;
            }

            return View(model);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Participate(FormCollection formCollection)
        {
            Tournoi tourn = uow.TournoiRepository.ObtenirTournoiCompletParId(int.Parse(formCollection["tournamentid"]));

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

                uow.TournoiRepository.UpdateTournoi(tourn);
                uow.Save();
            }
            else
            {
                Equipe team = uow.EquipeRepository.ObtenirEquipeCompletParId(int.Parse(formCollection["teamid"]));

                // TODO: Add player to team.

                uow.TournoiRepository.UpdateTournoi(tourn);
                uow.Save();
            }
            
            

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
