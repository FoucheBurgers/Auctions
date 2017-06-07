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
    public class AuctionsAdminController : Controller
    {
        private AuctionDBEntities db = new AuctionDBEntities();

        // GET: AuctionsAdmin
        public ActionResult Index()
        {
            return View(db.ltRollDescriptions.ToList());
        }

        // GET: AuctionsAdmin/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ltRollDescription ltRollDescription = db.ltRollDescriptions.Find(id);
            if (ltRollDescription == null)
            {
                return HttpNotFound();
            }
            return View(ltRollDescription);
        }

        // GET: AuctionsAdmin/Create
        public ActionResult Create()
        {
            //ViewBag.Path = "~/Images/";
            ltRollDescription rol = new ltRollDescription();
            DefaultSetup defaultSetup = db.DefaultSetups.FirstOrDefault(t => t.Active == true);

            rol.BackgroundColor = defaultSetup.BackgroundColor;
            rol.FontColor = defaultSetup.FontColor;
            rol.RefreshTime = defaultSetup.RefreshTime;
            rol.StartDate = DateTime.Now;
            rol.EndDate = DateTime.Now;
            rol.AuctionDelayTime = 5;
            rol.LogoPath = defaultSetup.LogoPath;
            rol.LogoName = defaultSetup.LogoName;
            rol.SmallLogoName = defaultSetup.SmallLogoName;
            rol.RollImagesPath = defaultSetup.RollImagesPath;
            rol.SmsNotification = true;
            rol.LogoBackgroundColor = defaultSetup.LogoBackgroundColor;
            rol.smsOutBidder = defaultSetup.smsOutBidder;
            rol.RollImagesBackColor = defaultSetup.BackgroundColor;

            rol.HomePeriodDescriptionBackColor = defaultSetup.HomePeriodDescriptionBackColor;
            rol.ActionBackColor = defaultSetup.ActionBackColor;
            rol.RollActionBidColor = defaultSetup.RollActionBidColor;
            rol.RollActionBackIndexColor = defaultSetup.RollActionBackIndexColor;



            return View(rol);
        }

        // POST: AuctionsAdmin/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Description,Active,BackgroundColor,FontColor,RefreshTime,StartDate,EndDate,AuctionDelayTime,SortPosition,LogoPath,LogoName,SmallLogoName,RollImagesPath,SmsNotification,LogoBackgroundColor, smsOutBidder, RollImagesBackColor, HomePeriodDescription, HomePeriodDescriptionTextColor, HomePeriodDescriptionBackColor, ActionTextColor, ActionBackColor, RollActionBidColor, RollActionBackIndexColor")] ltRollDescription ltRollDescription)
        {
            if (ModelState.IsValid)
            {

                db.ltRollDescriptions.Add(ltRollDescription);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(ltRollDescription);
        }

        // GET: AuctionsAdmin/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ltRollDescription ltRollDescription = db.ltRollDescriptions.Find(id);
            if (ltRollDescription == null)
            {
                return HttpNotFound();
            }
            return View(ltRollDescription);
        }

        // POST: AuctionsAdmin/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Description,Active,BackgroundColor,FontColor,RefreshTime,StartDate,EndDate,AuctionDelayTime,SortPosition,LogoPath,LogoName,SmallLogoName,RollImagesPath,SmsNotification,LogoBackgroundColor, smsOutBidder, RollImagesBackColor, HomePeriodDescription, HomePeriodDescriptionTextColor, HomePeriodDescriptionBackColor, ActionTextColor, ActionBackColor, RollActionBidColor, RollActionBackIndexColor")] ltRollDescription ltRollDescription)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ltRollDescription).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(ltRollDescription);
        }

        // GET: AuctionsAdmin/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ltRollDescription ltRollDescription = db.ltRollDescriptions.Find(id);
            if (ltRollDescription == null)
            {
                return HttpNotFound();
            }
            return View(ltRollDescription);
        }

        // POST: AuctionsAdmin/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ltRollDescription ltRollDescription = db.ltRollDescriptions.Find(id);
            db.ltRollDescriptions.Remove(ltRollDescription);
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
