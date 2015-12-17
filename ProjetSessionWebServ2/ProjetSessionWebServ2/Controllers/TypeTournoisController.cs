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
    [CustomUserAttribute(Roles = "administrateur", AccessLevel = "TypeTournoisController")]
    public class TypeTournoisController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: TypeTournois
        public ActionResult Index()
        {
            return View(db.TypeTournois.ToList());
        }

        // GET: TypeTournois/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TypeTournoi typeTournoi = db.TypeTournois.Find(id);
            if (typeTournoi == null)
            {
                return HttpNotFound();
            }
            return View(typeTournoi);
        }

        // GET: TypeTournois/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TypeTournois/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Nom")] TypeTournoi typeTournoi)
        {
            if (ModelState.IsValid)
            {
                db.TypeTournois.Add(typeTournoi);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(typeTournoi);
        }

        // GET: TypeTournois/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TypeTournoi typeTournoi = db.TypeTournois.Find(id);
            if (typeTournoi == null)
            {
                return HttpNotFound();
            }
            return View(typeTournoi);
        }

        // POST: TypeTournois/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Nom")] TypeTournoi typeTournoi)
        {
            if (ModelState.IsValid)
            {
                db.Entry(typeTournoi).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(typeTournoi);
        }

        // GET: TypeTournois/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TypeTournoi typeTournoi = db.TypeTournois.Find(id);
            if (typeTournoi == null)
            {
                return HttpNotFound();
            }
            return View(typeTournoi);
        }

        // POST: TypeTournois/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TypeTournoi typeTournoi = db.TypeTournois.Find(id);
            db.TypeTournois.Remove(typeTournoi);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

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
