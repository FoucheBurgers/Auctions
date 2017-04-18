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
    public class RollAdminController : Controller
    {
        private AuctionDBEntities db = new AuctionDBEntities();

        [Authorize(Roles = "Administrator")]

        // GET: RollAdmin
        public ActionResult Index()
        {

            var tblRolls = db.tblRolls.Include(t => t.ltRollDescription).Include(t => t.ltRollDescription1).Include(t => t.ltSpecy).Include(t => t.tblCustomer).Include(t => t.tblCustomer1);
            return View(tblRolls.ToList());
        }

        // GET: RollAdmin/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblRoll tblRoll = db.tblRolls.Find(id);
            if (tblRoll == null)
            {
                return HttpNotFound();
            }
            return View(tblRoll);
        }

        // GET: RollAdmin/Create
        public ActionResult Create()
        {
            ViewBag.RollId = new SelectList(db.ltRollDescriptions, "ID", "Description");
            ViewBag.RollId = new SelectList(db.ltRollDescriptions, "ID", "Description");
            ViewBag.SpeciesCode = new SelectList(db.ltSpecies, "ID", "Description");
            ViewBag.BuyerId = new SelectList(db.tblCustomers, "ID", "CompanyName");
            ViewBag.SellerId = new SelectList(db.tblCustomers, "ID", "CompanyName");
            return View();
        }

        // POST: RollAdmin/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,RollId,Lot,SpeciesCode,TagNr,Age,DateMeasured,HornLength,TipToTip,OtherInfo,DateAvailable,Male,Female,Young,Quantity,Quantity_Lot,SellerId,BuyerId,BiddingPrice,BidDateTime,BidTotalPrice,NewBidPrice,NewBidder,Picture,OnAuction,Sold,DateLoaded,DateSold,PicturePath,PictureName,Increments,ReservePrice,LotQ")] tblRoll tblRoll)
        {
            if (ModelState.IsValid)
            {
                db.tblRolls.Add(tblRoll);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.RollId = new SelectList(db.ltRollDescriptions, "ID", "Description", tblRoll.RollId);
            ViewBag.RollId = new SelectList(db.ltRollDescriptions, "ID", "Description", tblRoll.RollId);
            ViewBag.SpeciesCode = new SelectList(db.ltSpecies, "ID", "Description", tblRoll.SpeciesCode);
            ViewBag.BuyerId = new SelectList(db.tblCustomers, "ID", "CompanyName", tblRoll.BuyerId);
            ViewBag.SellerId = new SelectList(db.tblCustomers, "ID", "CompanyName", tblRoll.SellerId);
            return View(tblRoll);
        }

        // GET: RollAdmin/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblRoll tblRoll = db.tblRolls.Find(id);
            if (tblRoll == null)
            {
                return HttpNotFound();
            }
            ViewBag.RollId = new SelectList(db.ltRollDescriptions, "ID", "Description", tblRoll.RollId);
            ViewBag.RollId = new SelectList(db.ltRollDescriptions, "ID", "Description", tblRoll.RollId);
            ViewBag.SpeciesCode = new SelectList(db.ltSpecies, "ID", "Description", tblRoll.SpeciesCode);
//            ViewBag.SpeciesCode = new SelectList(db.Species, "ID", "NameAfr", tblRoll.SpeciesCode);
            ViewBag.BuyerId = new SelectList(db.tblCustomers, "ID", "CompanyName", tblRoll.BuyerId);
            ViewBag.SellerId = new SelectList(db.tblCustomers, "ID", "CompanyName", tblRoll.SellerId);
            return View(tblRoll);
        }

        // POST: RollAdmin/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,RollId,Lot,SpeciesCode,TagNr,Age,DateMeasured,HornLength,TipToTip,OtherInfo,DateAvailable,Male,Female,Young,Quantity,Quantity_Lot,SellerId,BuyerId,BiddingPrice,BidDateTime,BidTotalPrice,NewBidPrice,NewBidder,Picture,OnAuction,Sold,DateLoaded,DateSold,PicturePath,PictureName,Increments,ReservePrice,LotQ")] tblRoll tblRoll)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tblRoll).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.RollId = new SelectList(db.ltRollDescriptions, "ID", "Description", tblRoll.RollId);
            ViewBag.RollId = new SelectList(db.ltRollDescriptions, "ID", "Description", tblRoll.RollId);
            ViewBag.SpeciesCode = new SelectList(db.ltSpecies, "ID", "Description", tblRoll.SpeciesCode);
//            ViewBag.SpeciesCode = new SelectList(db.Species, "ID", "NameAfr", tblRoll.SpeciesCode);
            ViewBag.BuyerId = new SelectList(db.tblCustomers, "ID", "CompanyName", tblRoll.BuyerId);
            ViewBag.SellerId = new SelectList(db.tblCustomers, "ID", "CompanyName", tblRoll.SellerId);
            return View(tblRoll);
        }

        // GET: RollAdmin/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblRoll tblRoll = db.tblRolls.Find(id);
            if (tblRoll == null)
            {
                return HttpNotFound();
            }
            return View(tblRoll);
        }

        // POST: RollAdmin/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tblRoll tblRoll = db.tblRolls.Find(id);
            db.tblRolls.Remove(tblRoll);
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
