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
    public class TypeConferencesController : Controller
    {
        private UnitOfWork unitOfWork = new UnitOfWork();

        // GET: TypeConferences
        public ActionResult Index()
        {
            return View(unitOfWork.TypeConferenceRepository.ObtenirTypeConferences().ToList());
        }

        // GET: TypeConferences/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TypeConference typeConference = unitOfWork.TypeConferenceRepository.ObtenirTypeConferenceParID(id);
            if (typeConference == null)
            {
                return HttpNotFound();
            }
            return View(typeConference);
        }

        // GET: TypeConferences/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TypeConferences/Create
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Nom")] TypeConference typeConference)
        {
            if (ModelState.IsValid)
            {
                unitOfWork.TypeConferenceRepository.InsertTypeConference(typeConference);
                unitOfWork.Save();
                return RedirectToAction("Index");
            }

            return View(typeConference);
        }

        // GET: TypeConferences/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TypeConference typeConference = unitOfWork.TypeConferenceRepository.ObtenirTypeConferenceParID(id);
            if (typeConference == null)
            {
                return HttpNotFound();
            }
            return View(typeConference);
        }

        // POST: TypeConferences/Edit/5
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Nom")] TypeConference typeConference)
        {
            if (ModelState.IsValid)
            {
                unitOfWork.TypeConferenceRepository.UpdateTypeConference(typeConference);
                unitOfWork.Save();
                return RedirectToAction("Index");
            }
            return View(typeConference);
        }

        // GET: TypeConferences/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TypeConference typeConference = unitOfWork.TypeConferenceRepository.ObtenirTypeConferenceParID(id);
            if (typeConference == null)
            {
                return HttpNotFound();
            }
            return View(typeConference);
        }

        // POST: TypeConferences/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TypeConference typeConference = unitOfWork.TypeConferenceRepository.ObtenirTypeConferenceParID(id);

            unitOfWork.TypeConferenceRepository.DeleteTypeConference(typeConference);
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
