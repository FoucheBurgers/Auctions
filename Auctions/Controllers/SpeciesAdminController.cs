using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Auctions.Models;

namespace Auctions.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class SpeciesAdminController : Controller
    {
               
        private AuctionDBEntities db = new AuctionDBEntities();

        // GET: SpeciesAdmin
        public ActionResult Index()
        {
            return View(db.ltSpecies.ToList());
        }

        // GET: SpeciesAdmin/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ltSpecy ltSpecy = db.ltSpecies.Find(id);
            if (ltSpecy == null)
            {
                return HttpNotFound();
            }
            return View(ltSpecy);
        }

        // GET: SpeciesAdmin/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SpeciesAdmin/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Description,Active")] ltSpecy ltSpecy)
        {
            if (ModelState.IsValid)
            {
                db.ltSpecies.Add(ltSpecy);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(ltSpecy);
        }

        // GET: SpeciesAdmin/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ltSpecy ltSpecy = db.ltSpecies.Find(id);
            if (ltSpecy == null)
            {
                return HttpNotFound();
            }
            return View(ltSpecy);
        }

        // POST: SpeciesAdmin/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Description,Active")] ltSpecy ltSpecy)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ltSpecy).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(ltSpecy);
        }

        // GET: SpeciesAdmin/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ltSpecy ltSpecy = db.ltSpecies.Find(id);
            if (ltSpecy == null)
            {
                return HttpNotFound();
            }
            return View(ltSpecy);
        }

        // POST: SpeciesAdmin/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ltSpecy ltSpecy = db.ltSpecies.Find(id);
            db.ltSpecies.Remove(ltSpecy);
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
