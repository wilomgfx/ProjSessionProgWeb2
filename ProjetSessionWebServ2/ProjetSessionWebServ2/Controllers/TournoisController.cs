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
            ViewBag.lstSalle = new SelectList(unitOfWork.SalleRepository.ObtenirSalles(), "Id", "NoSalle");
            SelectList TypeTournoiId = new SelectList(unitOfWork.TypeTournoiRepository.ObtenirTypeTournois(), "Id", "Nom");
            ViewBag.TypeTournoiId = TypeTournoiId;
            SelectList lstSalle = new SelectList(unitOfWork.SalleRepository.ObtenirSalles(), "Id", "NoSalle");
            ViewBag.lstSalle = lstSalle;
            return View();
        }

        // POST: Tournois/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]

       // public ActionResult Create([Bind(Include = "Id,Nom,Description,TypeEvenement,TypeTournoiId,Actif")] TournoiVM tournoiVM, int Congres)

        public ActionResult Create([Bind(Include = "Id,Nom,Description,TypeEvenement,TypeTournoiId,Actif,lstSalle")] Tournoi tournoi,FormCollection collection,int? lstSalle)
        {
            DateTime dateTournoi;
            int heureDebut;
            int heureFin;
            ViewBag.Congres = new SelectList(unitOfWork.CongresRepository.ObtenirCongres(), "Id", "Nom");
            SelectList listSalle = new SelectList(unitOfWork.SalleRepository.ObtenirSalles(), "Id", "NoSalle");
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
                ViewBag.lstSalle = listSalle;
                return View(tournoi);
            }
            if (ModelState.IsValid)
            {
                int congresId = int.Parse(collection["Congres"]);
                Congres congres = unitOfWork.CongresRepository.ObtenirCongres().Where(u => u.Id == congresId).FirstOrDefault();
                   
                tournoi.TypeTournoi = unitOfWork.TypeTournoiRepository.ObtenirTypeTournoiParID(tournoi.TypeTournoiId);
                tournoi.TypeEvenement = Evenement.TypeEvent.TypeTournoi;
                tournoi.Actif = true;

                tournoi.Salle = unitOfWork.SalleRepository.ObtenirSalleParID(lstSalle);
                tournoi.Equipes = new List<Equipe>();
                tournoi.Avancements = new List<EquipeAvancement>();
                tournoi.Parties = new List<Partie>();
                tournoi.Congres = congres;

                //Creating the plage horaire
                PlageHoraire newPlageHoraire = new PlageHoraire();
                DateTime dateEtHeureDebut = dateTournoi.AddHours(heureDebut);
                DateTime dateEtHeureFin = dateTournoi.AddHours(heureFin);
                newPlageHoraire.DateEtHeureDebut = dateEtHeureDebut;
                newPlageHoraire.DateEtHeureFin = dateEtHeureFin;
                newPlageHoraire.Evenement = tournoi;
                newPlageHoraire.Congres = congres;

                List<PlageHoraire> lst = new List<PlageHoraire>();
                lst.Add(newPlageHoraire);

                if(!unitOfWork.IsRoomAvailableForTime(tournoi, lst))
                {
                    // Si la il y a conflit d'horaire pour une pièce....

                    TempData["message"] = "Il y a conflit d'horaire pour la salle que vous essayez de choisir. Veuillez choisir une autre salle, ou faire votre évènement à un moment différent.";
                    ViewBag.TypeTournoiId = TypeTournoiId;
                    ViewBag.lstSalle = listSalle;
                    return View(tournoi);
                }

                unitOfWork.TournoiRepository.InsertTournoi(tournoi);
                unitOfWork.Save();

               
                unitOfWork.PlageHoraireRepository.InsertPlageHoraire(newPlageHoraire);
                unitOfWork.Save();


                return RedirectToAction("Index");
            }

            ViewBag.TypeTournoiId = TypeTournoiId;
            ViewBag.lstSalle = lstSalle;

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
            SelectList SalleCongres = new SelectList(unitOfWork.SalleRepository.ObtenirSalles(), "Id", "NoSalle");
            ViewBag.lstSalle = SalleCongres;

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
            SelectList SalleCongres = new SelectList(unitOfWork.SalleRepository.ObtenirSalles(), "Id", "NoSalle");
            ViewBag.lstSalle = SalleCongres;

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

        public ActionResult AlreadyInTournament()
        {
            return View();
        }

        public ActionResult DetailsForTeam(int? id, int? tournid)
        {
            if (id == null || tournid == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Tournoi tournoi = db.Evenements.Find(id);
            Equipe eq = unitOfWork.EquipeRepository.ObtenirEquipeCompletParId(id);

            if (eq == null)
            {
                return HttpNotFound();
            }

            ViewData["TournID"] = tournid;
            return View(eq);
        }

        [Authorize]
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

            // On va chercher l'utilisateur courant.
            UserManager<ApplicationUser> UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(unitOfWork.context));
            ApplicationUser utilisateur = UserManager.FindById(User.Identity.GetUserId());

            // On vérifis si l'utilisateur participe déjà au tournois. Si oui, on le redirige.
            foreach (Equipe eq in tournoi.Equipes)
            {
                Equipe vraiEquipe = unitOfWork.EquipeRepository.ObtenirEquipeCompletParId(eq.Id);

                foreach (ApplicationUser joueur in vraiEquipe.Joueurs)
                {
                    if(joueur.Email.Equals(utilisateur.Email) || joueur.UserName.Equals(utilisateur.UserName))
                    {
                        return RedirectToAction("AlreadyInTournament");
                    }
                }
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
        [Authorize]
        //[ValidateAntiForgeryToken]
        public ActionResult Participate(FormCollection formCollection)
        {
            Tournoi tourn = unitOfWork.TournoiRepository.ObtenirTournoiCompletParId(int.Parse(formCollection["tournamentid"]));
            //On va chercher l'utilisateur courant.
            UserManager<ApplicationUser> UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(unitOfWork.context));
            ApplicationUser utilisateur = UserManager.FindById(User.Identity.GetUserId());

            // On s'assure que le tournoi et ses champs sont initialisés.
            if (tourn.Users == null)
            {
                tourn.Users = new List<ApplicationUser>();
            }
            
            if(tourn.Equipes == null)
            {
                tourn.Equipes = new List<Equipe>();
            }
            if(tourn.Avancements == null)
            {
                tourn.Avancements = new List<EquipeAvancement>();
            }
            if(tourn.Parties == null)
            {
                tourn.Parties = new List<Partie>();
            }

            // Si on participe avec une nouvelle équipe...
            if(formCollection["teamid"].Equals("-1"))
            {
                Equipe team = new Equipe();
                team.Nom = formCollection["teamname"];
            
                team.Joueurs = new List<ApplicationUser>();
                team.Joueurs.Add(utilisateur);

                tourn.Equipes.Add(team);

                unitOfWork.TournoiRepository.UpdateTournoi(tourn);
                unitOfWork.Save();
            }
                // Si on participe avec une équipe existante...
            else
            {
                Equipe team = unitOfWork.EquipeRepository.ObtenirEquipeCompletParId(int.Parse(formCollection["teamid"]));

                team.Joueurs.Add(utilisateur);

                unitOfWork.TournoiRepository.UpdateTournoi(tourn);
                unitOfWork.Save();
            }
            
            

            return RedirectToAction("Index");
        }

        public ActionResult TournamentStatus(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // On va chercher le tournoi...
            Tournoi tournoi = unitOfWork.TournoiRepository.ObtenirTournoiCompletParId(id);

            if (tournoi == null)
            {
                return HttpNotFound();
            }

            // On initialise

            // On va chercher les joueurs dans chaque équipe. Foutu Lazy Loading ne les charge pas correctement.
            foreach (Equipe eq in tournoi.Equipes)
            {
                eq.Joueurs = unitOfWork.EquipeRepository.ObtenirEquipeCompletParId(eq.Id).Joueurs;
            }

            List<EquipeAvancement> equipeAvancement = unitOfWork.EquipeAvancementRepository.ObtenirEquipeAvancementsParTournoiTriee(id).ToList();

            Tournoi_EquipesPointages viewModel = new Tournoi_EquipesPointages();
            viewModel.Tournoi = tournoi;
            viewModel.Equipes = equipeAvancement;

            return View(viewModel);
        }

        public ActionResult TournamentGames(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // On va chercher le tournoi...
            Tournoi tournoi = unitOfWork.TournoiRepository.ObtenirTournoiCompletParId(id);

            if (tournoi == null)
            {
                return HttpNotFound();
            }

            // On va chercher les parties du tournoi...
            tournoi.Parties = unitOfWork.PartieRepository.ObtenirPartiesParTournoi(tournoi.Id).OrderByDescending(p => p.DateEtHeureDebut).ToList();

            return View(tournoi);
        }

        [CustomUserAttribute(Roles = "administrateur", AccessLevel = "AddGame")]
        public ActionResult AddGame(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // On va chercher le tournoi...
            Tournoi tournoi = unitOfWork.TournoiRepository.ObtenirTournoiCompletParId(id);

            if (tournoi == null)
            {
                return HttpNotFound();
            }

            // infos pour la vue
            ViewBag.NomTournoi = tournoi.Nom;
            ViewBag.idTournoi = tournoi.Id;
            ViewBag.Equipes = tournoi.Equipes;
            

            return View(new Partie());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [CustomUserAttribute(Roles = "administrateur", AccessLevel = "AddGame")]
        public ActionResult AddGame(FormCollection collection, int? NomRound)
        {
            // On prépare la vue, au cas où la création de partie ne fonctionne pas.
            Tournoi tournoi = unitOfWork.TournoiRepository.ObtenirTournoiCompletParId(int.Parse(collection["tournid"]));

            // Pour éviter le Lazy Loading
            tournoi.Parties = unitOfWork.PartieRepository.ObtenirPartiesParTournoi(tournoi.Id).ToList();

            if (tournoi == null)
            {
                return HttpNotFound();
            }

            ViewBag.NomTournoi = tournoi.Nom;
            ViewBag.idTournoi = tournoi.Id;
            ViewBag.Equipes = tournoi.Equipes;

            // On créer notre partie.
            Partie partie = new Partie();
            // Elle est active.
            partie.Actif = true;

            // On essaye d'assigner une date à la partie.
            DateTime datePartie;
            int heureDebut;
            int heureFin;
            try
            {
                datePartie = DateTime.Parse(collection["DatePartie"]);
                heureDebut = int.Parse(collection["HeureDebut"]);
                heureFin = int.Parse(collection["HeureFin"]);
            }
            catch (Exception e)
            {
                // La date est mal formatée...
                TempData["message"] = "La date de l'événement doit être une date valide sous le format AAAA-MM-JJ. L'heure de début et l'heure de fin doivent être des chiffres";
                
                return View(partie);
            }
            if (ModelState.IsValid)
            {
                // On assigne les équipes à la partie.
                partie.Equipes = new List<Equipe>();

                string equipesString = collection["Equipes"];

                if(equipesString.Equals(""))
                {
                    // Il n'y a pas d'équipes de choisis...
                    TempData["message"] = "Veuillez choisir des équipes pour cette partie.";

                    return View(partie);
                }

                string[] equipeStringAr = equipesString.Split(',');

                try
                {
                    // Avec les noms d'équipe, on va chercher les objets Equipes. On ajoute ces equipes à notre partie.
                    for (int i = 0; i < equipeStringAr.Length; i++)
                    {
                        string teamName = equipeStringAr[i];

                        // Dans le cas que la personne a mal formatté les équipes, et qu'il reste une ',' à la fin.
                        if (teamName == "")
                        {
                            continue;
                        }
                        else
                        {
                            // Si la personne a formatté avec des espaces après les virgules, on enlève les espaces.
                            if(teamName[0].ToString().Equals(" "))
                            {
                                teamName = teamName.Substring(1);
                            }
                            Equipe e = unitOfWork.EquipeRepository.ObtenirEquipeCompleteParNom(teamName);

                            if(e == null)
                            {
                                throw new InvalidOperationException("Mauvaise Equipe");
                            }

                            partie.Equipes.Add(e);
                        }
                    }
                }
                catch(Exception e)
                {
                    // Les équipes sont mal formattés, ou une équipe n'existe pas.
                    TempData["message"] = "Veuillez choisir des équipes valides pour cette partie, et vérifier le formattage.";

                    return View(partie);
                }

                // Assignation de la date et heure

                DateTime dateEtHeureDebut = datePartie.AddHours(heureDebut);
                DateTime dateEtHeureFin = datePartie.AddHours(heureFin);

                if(dateEtHeureDebut < tournoi.PlageHoraires.First().DateEtHeureDebut ||
                   dateEtHeureFin > tournoi.PlageHoraires.First().DateEtHeureFin)
                {
                    PlageHoraire plage = tournoi.PlageHoraires.First();

                    TempData["message"] = "La date de l'événement doit être une date valide pour la période du tournoi, c'est à dire entre le " + 
                        plage.DateEtHeureDebut.ToString() + " et le " + plage.DateEtHeureFin.ToString();

                    return View(partie);
                }

                partie.DateEtHeureDebut = dateEtHeureDebut;
                partie.DateEtHeureFin = dateEtHeureFin;

                if(tournoi.Parties == null)
                {
                    tournoi.Parties = new List<Partie>();
                }

                tournoi.Parties.Add(partie);
                unitOfWork.TournoiRepository.UpdateTournoi(tournoi);

                //unitOfWork.PartieRepository.InsertPartie(partie);
                unitOfWork.Save();



                return RedirectToAction("TournamentGames", new { id=tournoi.Id });
            }

            return View(new Partie());
        }

        // Annule une partie
        [CustomUserAttribute(Roles = "administrateur", AccessLevel = "CancelGame")]
        public ActionResult CancelGame(int? id, int? tournid)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // On va chercher la partie...
            Partie partie = unitOfWork.PartieRepository.ObtenirPartieCompletParId(id);

            if (partie == null)
            {
                return HttpNotFound();
            }

            // On annule la partie.
            partie.Actif = false;

            unitOfWork.PartieRepository.UpdatePartie(partie);
            unitOfWork.Save();

            return RedirectToAction("TournamentGames", new { id=tournid });
        }

        // Amène vers la page qui permet de choisir un gagnant pour une partie
        [CustomUserAttribute(Roles = "administrateur", AccessLevel = "SetGameWinner")]
        public ActionResult SetGameWinner(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // On va chercher la partie...
            Partie partie = unitOfWork.PartieRepository.ObtenirPartieCompletParId(id);

            if (partie == null)
            {
                return HttpNotFound();
            }

            return View(partie);
        }

        // Assigne un gagnant à une partie
        [CustomUserAttribute(Roles = "administrateur", AccessLevel = "ChooseWinner")]
        public ActionResult ChooseWinner(int? id, int? eqid)
        {
            if (id == null || eqid == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // On va chercher la partie...
            Partie partie = unitOfWork.PartieRepository.ObtenirPartieCompletParId(id);

            if (partie == null)
            {
                return HttpNotFound();
            }

            // On va chercher l'équipe, si elle fait partie de celles inclues dans la partie...
            Equipe eq = partie.Equipes.Find(e => e.Id == eqid);

            if(eq == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // On assigne le gagnant.
            partie.Gagnant = eq.Nom;

            // Il faut maintenant assigner les points à l'équipe gagnante.
            Tournoi tournoi = partie.Tournoi;

            EquipeAvancement avance = unitOfWork.EquipeAvancementRepository.ObtenirEquipeAvancementParIdEquipeParTournoi(eqid, tournoi.Id);

            if(avance == null)
            {
                if(tournoi.Avancements == null)
                {
                    tournoi.Avancements = new List<EquipeAvancement>();
                }

                EquipeAvancement ava = new EquipeAvancement();

                ava.Equipe = eq;
                ava.NbrDePoints = 0;
                ava.Tournoi = tournoi;

                avance = ava;

                tournoi.Avancements.Add(ava);

                unitOfWork.TournoiRepository.UpdateTournoi(tournoi);
                unitOfWork.Save();
            }

            // On ajoute un point.
            avance.NbrDePoints += 1;

            unitOfWork.PartieRepository.UpdatePartie(partie);
            unitOfWork.EquipeAvancementRepository.UpdateEquipeAvancement(avance);
            unitOfWork.Save();

            return RedirectToAction("TournamentGames", new { id = tournoi.Id });
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
