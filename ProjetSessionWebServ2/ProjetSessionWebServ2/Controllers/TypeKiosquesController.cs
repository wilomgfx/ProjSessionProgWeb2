using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ProjetSessionWebServ2.Models;
using GestionPhotoImmobilier.DAL;

namespace ProjetSessionWebServ2.Controllers
{
    public class TypeKiosquesController : Controller
    {
        private UnitOfWork uow = new UnitOfWork();

        // GET: TypeKiosques
        public ActionResult Index()
        {
            return View(uow.TypeKiosqueRepository.ObtenirTypeKiosques().ToList());
        }

        // GET: TypeKiosques/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TypeKiosque typeKiosque = uow.TypeKiosqueRepository.ObtenirTypeKiosqueParID(id);
            if (typeKiosque == null)
            {
                return HttpNotFound();
            }
            return View(typeKiosque);
        }

        // GET: TypeKiosques/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TypeKiosques/Create
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Nom")] TypeKiosque typeKiosque)
        {
            if (ModelState.IsValid)
            {
                uow.TypeKiosqueRepository.Insert(typeKiosque);
                uow.Save();
                return RedirectToAction("Index");
            }

            return View(typeKiosque);
        }

        // GET: TypeKiosques/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TypeKiosque typeKiosque = uow.TypeKiosqueRepository.ObtenirTypeKiosqueParID(id);
            if (typeKiosque == null)
            {
                return HttpNotFound();
            }
            return View(typeKiosque);
        }

        // POST: TypeKiosques/Edit/5
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Nom")] TypeKiosque typeKiosque)
        {
            if (ModelState.IsValid)
            {
                uow.TypeKiosqueRepository.UpdateTypeKiosque(typeKiosque);
                uow.Save();
                return RedirectToAction("Index");
            }
            return View(typeKiosque);
        }

        //// GET: TypeKiosques/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    TypeKiosque typeKiosque = uow.TypeKiosqueRepository.ObtenirTypeKiosqueParID(id);
        //    if (typeKiosque == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(typeKiosque);
        //}

        //// POST: TypeKiosques/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    TypeKiosque typeKiosque = uow.TypeKiosqueRepository.ObtenirTypeKiosqueParID(id);
        //    ty
        //    uow.TypeKiosqueRepository.UpdateTypeKiosque(typeKiosque);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

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
