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
    [Authorize]
    public class DimensionsController : Controller
    {
        //private ApplicationDbContext db = new ApplicationDbContext();
        private UnitOfWork uow = new UnitOfWork();

        // GET: Dimensions
        public ActionResult Index()
        {
            return View(uow.DimensionRepository.ObtenirDimensions().ToList());
        }

        // GET: Dimensions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Dimension dimension = uow.DimensionRepository.ObtenirDimensionParID(id);
            if (dimension == null)
            {
                return HttpNotFound();
            }
            return View(dimension);
        }
        [CustomUserAttribute(Roles = "administrateur", AccessLevel = "Create")]
        // GET: Dimensions/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Dimensions/Create
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Longueur,Largeur")] Dimension dimension)
        {
            if (ModelState.IsValid)
            {
                //db.Dimensions.Add(dimension);
                //db.SaveChanges();
                uow.DimensionRepository.InsertDimension(dimension);
                uow.Save();
                return RedirectToAction("Index");
            }

            return View(dimension);
        }
        [CustomUserAttribute(Roles = "administrateur", AccessLevel = "Edit")]
        // GET: Dimensions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Dimension dimension = uow.DimensionRepository.ObtenirDimensionParID(id);
            if (dimension == null)
            {
                return HttpNotFound();
            }
            return View(dimension);
        }

        // POST: Dimensions/Edit/5
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Longueur,Largeur")] Dimension dimension)
        {
            if (ModelState.IsValid)
            {
                //db.Entry(dimension).State = EntityState.Modified;
                //db.SaveChanges();
                uow.DimensionRepository.UpdateDimension(dimension);
                uow.Save();
                return RedirectToAction("Index");
            }
            return View(dimension);
        }
        [CustomUserAttribute(Roles = "administrateur", AccessLevel = "Delete")]
        // GET: Dimensions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Dimension dimension = uow.DimensionRepository.ObtenirDimensionParID(id);
            if (dimension == null)
            {
                return HttpNotFound();
            }
            return View(dimension);
        }

        // POST: Dimensions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Dimension dimension = uow.DimensionRepository.ObtenirDimensionParID(id);
            //db.Dimensions.Remove(dimension);
            //db.SaveChanges();
            uow.DimensionRepository.DeleteDimension(dimension);
            uow.Save();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
               // db.Dispose();
                uow.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
