using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ProjetSessionWebServ2.Models;

namespace ProjetSessionWebServ2.DAL
{
    public class TypeSpectaclesController : Controller
    {
        private UnitOfWork unitOfWork = new UnitOfWork();

        // GET: TypeSpectacles
        public ActionResult Index()
        {
            return View(unitOfWork.TypeSpectacleRepository.ObtenirTypeSpectacles());
        }

        // GET: TypeSpectacles/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TypeSpectacle typeSpectacle = unitOfWork.TypeSpectacleRepository.ObtenirTypeSpectacleParID(id);

            if (typeSpectacle == null)
            {
                return HttpNotFound();
            }
            return View(typeSpectacle);
        }

        // GET: TypeSpectacles/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TypeSpectacles/Create
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Nom")] TypeSpectacle typeSpectacle)
        {
            if (ModelState.IsValid)
            {
                unitOfWork.TypeSpectacleRepository.InsertTypeSpectacle(typeSpectacle);
                unitOfWork.Save();
                return RedirectToAction("Index");
            }

            return View(typeSpectacle);
        }

        // GET: TypeSpectacles/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TypeSpectacle typeSpectacle = unitOfWork.TypeSpectacleRepository.ObtenirTypeSpectacleParID(id);

            if (typeSpectacle == null)
            {
                return HttpNotFound();
            }
            return View(typeSpectacle);
        }

        // POST: TypeSpectacles/Edit/5
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Nom")] TypeSpectacle typeSpectacle)
        {
            if (ModelState.IsValid)
            {
                unitOfWork.TypeSpectacleRepository.InsertTypeSpectacle(typeSpectacle);
                unitOfWork.Save();
                return RedirectToAction("Index");
            }

            return View(typeSpectacle);
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
