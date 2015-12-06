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
    public class PlageHorairesController : Controller
    {
        //private ApplicationDbContext db = new ApplicationDbContext();
        private UnitOfWork uow = new UnitOfWork();

        // GET: PlageHoraires
        public ActionResult Index()
        {
            List<PlageHoraire> lstPlageHoraire = uow.PlageHoraireRepository.Get().ToList();
            return View(lstPlageHoraire);
        }

        // GET: PlageHoraires/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PlageHoraire plageHoraire = uow.PlageHoraireRepository.GetByID(id);
            if (plageHoraire == null)
            {
                return HttpNotFound();
            }
            return View(plageHoraire);
        }

        // GET: PlageHoraires/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PlageHoraires/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id")] PlageHoraire plageHoraire)
        {
            if (ModelState.IsValid)
            {
                uow.PlageHoraireRepository.InsertPlageHoraire(plageHoraire);
                uow.Save();
                return RedirectToAction("Index");
            }

            return View(plageHoraire);
        }

        // GET: PlageHoraires/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PlageHoraire plageHoraire = uow.PlageHoraireRepository.GetByID(id);
            if (plageHoraire == null)
            {
                return HttpNotFound();
            }
            return View(plageHoraire);
        }

        // POST: PlageHoraires/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id")] PlageHoraire plageHoraire)
        {
            if (ModelState.IsValid)
            {
                uow.PlageHoraireRepository.UpdatePlageHoraire(plageHoraire);
                uow.Save();
                return RedirectToAction("Index");
            }
            return View(plageHoraire);
        }

        // GET: PlageHoraires/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PlageHoraire plageHoraire = uow.PlageHoraireRepository.GetByID(id);
            if (plageHoraire == null)
            {
                return HttpNotFound();
            }
            return View(plageHoraire);
        }

        // POST: PlageHoraires/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PlageHoraire plageHoraire = uow.PlageHoraireRepository.GetByID(id);
            uow.PlageHoraireRepository.DeletePlageHoraire(plageHoraire);
            uow.Save();
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
