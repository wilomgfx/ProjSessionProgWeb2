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
    public class CongresController : Controller
    {
        private UnitOfWork unitOfWork = new UnitOfWork();

        // GET: Congres
        public ActionResult Index()
        {
            return View(unitOfWork.CongresRepository.ObtenirCongres().ToList());
        }

        // GET: Congres/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Congres congres = unitOfWork.CongresRepository.ObtenirCongresParID(id);
            if (congres == null)
            {
                return HttpNotFound();
            }
            return View(congres);
        }

        // GET: Congres/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Congres/Create
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Description,Adresse,Nom,DateDebut,DateFin")] Congres congres)
        {
            if (ModelState.IsValid)
            {
                unitOfWork.CongresRepository.InsertCongres(congres);
                unitOfWork.Save();
                return RedirectToAction("Index");
            }

            return View(congres);
        }

        // GET: Congres/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Congres congres = unitOfWork.CongresRepository.ObtenirCongresParID(id);
            if (congres == null)
            {
                return HttpNotFound();
            }
            return View(congres);
        }

        // POST: Congres/Edit/5
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Description,Adresse,Nom,DateDebut,DateFin")] Congres congres)
        {
            if (ModelState.IsValid)
            {
                unitOfWork.CongresRepository.UpdateCongres(congres);
                unitOfWork.Save();
                return RedirectToAction("Index");
            }
            return View(congres);
        }

        // GET: Congres/Delete/5

        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Congres congres = unitOfWork.CongresRepository.ObtenirCongresParID(id);
        //    if (congres == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(congres);
        //}

        //// POST: Congres/Delete/5

        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    Congres congres = unitOfWork.CongresRepository.ObtenirCongresParID(id);
        //    congres.Actif = false;
        //    unitOfWork.CongresRepository.UpdateCongres(congres);
            //unitOfWork.Save();
            //return RedirectToAction("Index");
        //}

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
