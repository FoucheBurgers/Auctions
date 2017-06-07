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
    public class DefaultSetupsController : Controller
    {
        private AuctionDBEntities db = new AuctionDBEntities();

        // GET: DefaultSetups
        public ActionResult Index()
        {
            return View(db.DefaultSetups.ToList());
        }

        // GET: DefaultSetups/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DefaultSetup defaultSetup = db.DefaultSetups.Find(id);
            if (defaultSetup == null)
            {
                return HttpNotFound();
            }
            return View(defaultSetup);
        }

        // GET: DefaultSetups/Create
        public ActionResult Create()
        {
            ViewBag.DefaultAuction = new SelectList(db.ltRollDescriptions, "ID", "Description");
            return View();
        }

        // POST: DefaultSetups/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Description,Active,BackgroundColor,FontColor,RefreshTime,AuctionDelayTime,LogoPath,LogoName,SmallLogoName,RollImagesPath,SmsNotification,LogoBackgroundColor,SMSAllFunctionality,SMSCustRegistration,SMSConfirmationLoginRequired,emailConfirmRegistration,emailConfirmationLoginRequired,BackgroundColorHome, DefaultAuction, smsOutBidder, PageSize, , DispLines, DispColumns, RollDispRefreshRate, HomePeriodDescription, HomePeriodDescriptionTextColor, HomePeriodDescriptionBackColor, ActionTextColor, ActionBackColor, RollActionBidColor, RollActionBackIndexColor")] DefaultSetup defaultSetup)
        {
            if (ModelState.IsValid)
            {
                db.DefaultSetups.Add(defaultSetup);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.DefaultAuction = new SelectList(db.ltRollDescriptions, "ID", "Description", defaultSetup.DefaultAuction);
            return View(defaultSetup);
        }

        // GET: DefaultSetups/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DefaultSetup defaultSetup = db.DefaultSetups.Find(id);
            if (defaultSetup == null)
            {
                return HttpNotFound();
            }
            ViewBag.DefaultAuction = new SelectList(db.ltRollDescriptions, "ID", "Description", defaultSetup.DefaultAuction);
            return View(defaultSetup);
        }

        // POST: DefaultSetups/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Description,Active,BackgroundColor,FontColor,RefreshTime,AuctionDelayTime,LogoPath,LogoName,SmallLogoName,RollImagesPath,SmsNotification,LogoBackgroundColor,SMSAllFunctionality,SMSCustRegistration,SMSConfirmationLoginRequired,emailConfirmRegistration,emailConfirmationLoginRequired,BackgroundColorHome, DefaultAuction, smsOutBidder, PageSize, DispLines, DispColumns, RollDispRefreshRate, HomePeriodDescription, HomePeriodDescriptionTextColor, HomePeriodDescriptionBackColor, ActionTextColor, ActionBackColor, RollActionBidColor, RollActionBackIndexColor")] DefaultSetup defaultSetup)
        {
            if (ModelState.IsValid)
            {
                db.Entry(defaultSetup).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.DefaultAuction = new SelectList(db.ltRollDescriptions, "ID", "Description", defaultSetup.DefaultAuction);
            return View(defaultSetup);
        }

        // GET: DefaultSetups/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DefaultSetup defaultSetup = db.DefaultSetups.Find(id);
            if (defaultSetup == null)
            {
                return HttpNotFound();
            }
            return View(defaultSetup);
        }

        // POST: DefaultSetups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DefaultSetup defaultSetup = db.DefaultSetups.Find(id);
            db.DefaultSetups.Remove(defaultSetup);
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
