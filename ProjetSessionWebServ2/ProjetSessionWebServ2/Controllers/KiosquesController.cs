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
    public class KiosquesController : Controller
    {
        //private ApplicationDbContext db = new ApplicationDbContext();
        private UnitOfWork uow = new UnitOfWork();

        // GET: Kiosques
        public ActionResult Index(string currentFilter, string searchTypeKiosque, string searchNomKiosque, string searchKiosqueur, string trieKiosque)
        {
            ViewBag.searchTypeKiosque = new SelectList(uow.TypeKiosqueRepository.ObtenirTypeKiosques(), "Nom", "Nom", string.Empty);
            List<Kiosque> lstKiosqueApresTrie = new List<Kiosque>();
            if (trieKiosque == null)// On trie selon les parametre
            {
                if (searchKiosqueur == null)
                {
                    searchKiosqueur = "";
                }
                if (searchNomKiosque == null)
                {
                    searchNomKiosque = "";
                }
                if (searchTypeKiosque == null)
                {
                    searchTypeKiosque = "";
                }

                //Trie selon les parametre de recherche entre par l'utilisateur
                List<Kiosque> colKiosque = uow.KiosqueRepository.ObtenirKiosques().ToList();

                List<Kiosque> colKiosqueApresrechecheType = colKiosque.Where(u => u.TypeKiosque.Nom.Contains(searchTypeKiosque)).ToList();
                List<Kiosque> colKiosqueeApresRechercheNomKiosque = colKiosqueApresrechecheType.Where(u => u.Nom.Contains(searchNomKiosque)).ToList();
                List<Kiosque> colKiosqueApresRechercheKiosqueur = new List<Kiosque>();

                if(!searchKiosqueur.Equals(""))
                {
                    foreach (Kiosque Kiosque in colKiosqueeApresRechercheNomKiosque)
                    {
                        foreach (ApplicationUser user in Kiosque.Users)
                        {
                            if (user.UserName.ToLower().Contains(searchKiosqueur.ToLower()))
                            {
                                colKiosqueApresRechercheKiosqueur.Add(Kiosque);
                            }
                        }
                    }

                    lstKiosqueApresTrie = colKiosqueApresRechercheKiosqueur;
                }
                else 
                {
                    lstKiosqueApresTrie = colKiosqueeApresRechercheNomKiosque;
                }         
            }
            else // On trie par type de Kiosque
            {
                lstKiosqueApresTrie = uow.KiosqueRepository.ObtenirKiosques().OrderBy(x => x.TypeKiosque.Nom).ToList();
            }

            return View(lstKiosqueApresTrie);
        }

        // GET: Kiosques/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Kiosque Kiosque = db.Evenements.Find(id);
            Kiosque Kiosque = uow.KiosqueRepository.ObtenirKiosques().Where(k => k.Id == id).SingleOrDefault();
            if (Kiosque == null)
            {
                return HttpNotFound();
            }
            return View(Kiosque);
        }

        [Authorize(Roles = "kiosqueur")]
        // GET: Kiosques/Create
        public ActionResult Create()
        {
            ViewBag.Congres = new SelectList(uow.CongresRepository.ObtenirCongres(), "Id", "Nom");
            SelectList TypeKiosqueId = new SelectList(uow.TypeKiosqueRepository.ObtenirTypeKiosques(), "Id", "Nom");
            ViewBag.TypeKiosqueId = TypeKiosqueId;
            return View();
        }

        // POST: Kiosques/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Nom,Description,TypeKiosqueId,Actif")] Kiosque Kiosque, int Congres)
        {
            Kiosque.Actif = true;

            if (ModelState.IsValid)
            {

                UserManager<ApplicationUser> UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(uow.context));
                ApplicationUser utilisateur = UserManager.FindById(User.Identity.GetUserId());
                if (Kiosque.Users == null)
                {
                    Kiosque.Users = new List<ApplicationUser>();
                }

                Kiosque.Users.Add(utilisateur);

                //db.Evenements.Add(Kiosque);
                //db.SaveChanges();
                Kiosque.TypeKiosque = uow.TypeKiosqueRepository.ObtenirTypeKiosqueParID(Kiosque.TypeKiosqueId);
                Kiosque.TypeEvenement = Evenement.TypeEvent.TypeKiosque;
                Kiosque.Actif = true;
                Congres congres = uow.CongresRepository.ObtenirCongres().Where(u => u.Id.Equals(Congres)).FirstOrDefault();
                Kiosque.Congres = congres;

                uow.KiosqueRepository.InsertKiosque(Kiosque);
                uow.Save();
                return RedirectToAction("Index");
            }

            SelectList typesKiosques = new SelectList(uow.TypeKiosqueRepository.ObtenirTypeKiosques(), "Id", "Nom", Kiosque.TypeKiosqueId);
            ViewBag.TypeKiosqueId = typesKiosques;

            return View(Kiosque);
        }

        [Authorize(Roles = "kiosqueur")]
        // GET: Kiosques/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Kiosque Kiosque = db.Evenements.Find(id);
            Kiosque Kiosque = uow.KiosqueRepository.ObtenirKiosqueParID(id);
            if (Kiosque == null)
            {
                return HttpNotFound();
            }

            SelectList TypeKiosqueId = new SelectList(uow.TypeKiosqueRepository.ObtenirTypeKiosques(), "Id", "Nom", Kiosque.TypeKiosqueId);
            ViewBag.TypeKiosqueId = TypeKiosqueId;

            return View(Kiosque);
        }

        // POST: Kiosques/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Nom,Description,TypeKiosqueId,Actif")] Kiosque Kiosque)
        {
            if (ModelState.IsValid)
            {
                Kiosque.TypeKiosque = uow.TypeKiosqueRepository.ObtenirTypeKiosqueParID(Kiosque.TypeKiosqueId);
                Kiosque.TypeEvenement = Evenement.TypeEvent.TypeKiosque;
                uow.KiosqueRepository.UpdateKiosque(Kiosque);
                uow.Save();
                return RedirectToAction("Index");
            }

            SelectList typesKiosques = new SelectList(uow.TypeKiosqueRepository.ObtenirTypeKiosques(), "Id", "Nom", Kiosque.TypeKiosqueId);
            ViewBag.TypeKiosqueId = typesKiosques;

            return View(Kiosque);
        }

        [Authorize(Roles = "kiosqueur")]
        // GET: Kiosques/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Kiosque Kiosque = db.Evenements.Find(id);
            Kiosque Kiosque = uow.KiosqueRepository.ObtenirKiosqueParID(id);
            if (Kiosque == null)
            {
                return HttpNotFound();
            }
            return View(Kiosque);
        }

        // POST: Kiosques/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {

            Kiosque Kiosque = uow.KiosqueRepository.ObtenirKiosqueParID(id);
            Kiosque.Actif = false;
            uow.KiosqueRepository.UpdateKiosque(Kiosque);
            uow.Save();
            //db.Evenements.Remove(Kiosque);
            //db.SaveChanges();
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
