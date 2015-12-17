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
    public class SectionsController : Controller
    {
        //private ApplicationDbContext db = new ApplicationDbContext();
        private UnitOfWork uow = new UnitOfWork();

        // GET: Sections
        public ActionResult Index()
        {
            return View(uow.SectionRepository.ObtenirSections().ToList());
        }

        // GET: Sections/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Section section = uow.SectionRepository.ObtenirSectionParID(id);
            if (section == null)
            {
                return HttpNotFound();
            }
            return View(section);
        }
        [CustomUserAttribute(Roles = "administrateur", AccessLevel = "Create")]
        // GET: Sections/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Sections/Create
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Nom,TailleSection")] Section section, Dimension Dimension/*, Taille TailleSection*/)
        {
            if (ModelState.IsValid)
            {
                //db.Sections.Add(section);
                //db.SaveChanges
                section.Dimension = Dimension;
                uow.SectionRepository.InsertSection(section);
                uow.Save();
                return RedirectToAction("Index");
            }

            return View(section);
        }
        [CustomUserAttribute(Roles = "administrateur", AccessLevel = "Edit")]
        // GET: Sections/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Section section = uow.SectionRepository.ObtenirSectionParID(id);
            if (section == null)
            {
                return HttpNotFound();
            }
            return View(section);
        }

        // POST: Sections/Edit/5
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Nom")] Section section, Dimension Dimension, Taille taille)
        {
            if (ModelState.IsValid)
            {
                //db.Entry(section).State = EntityState.Modified;
                //db.SaveChanges();
                uow.SectionRepository.UpdateSection(section);
                uow.Save();
                return RedirectToAction("Index");
            }
            return View(section);
        }
        [CustomUserAttribute(Roles = "administrateur", AccessLevel = "Delete")]
        // GET: Sections/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Section section = uow.SectionRepository.ObtenirSectionParID(id);
            if (section == null)
            {
                return HttpNotFound();
            }
            return View(section);
        }

        // POST: Sections/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Section section = uow.SectionRepository.ObtenirSectionParID(id);
            //db.Sections.Remove(section);
            //db.SaveChanges();
            uow.SectionRepository.DeleteSection(section);
            uow.Save();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                //db.Dispose();
                uow.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
