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
    public class TempRollsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: TempRolls
        public ActionResult Index()
        {
            return View(db.tblRolls.ToList());
        }

        // GET: TempRolls/Details/5
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

        // GET: TempRolls/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TempRolls/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,RollId,Lot,SpeciesCode,TagNr,Age,DateMeasured,HornLength,TipToTip,OtherInfo,DateAvailable,Male,Female,Young,Quantity,Quantity_Lot,SellerId,BuyerId,BiddingPrice,BidDateTime,BidTotalPrice,NewBidPrice,NewBidder,Picture,OnAuction,Sold,DateLoaded,DateSold,PicturePath,PictureName,Increments,ReservePrice,LotQ,DateTimeBid,DateTimeClose,CustomerNumber,BidOpen")] tblRoll tblRoll)
        {
            if (ModelState.IsValid)
            {
                db.tblRolls.Add(tblRoll);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tblRoll);
        }

        // GET: TempRolls/Edit/5
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
            return View(tblRoll);
        }

        // POST: TempRolls/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,RollId,Lot,SpeciesCode,TagNr,Age,DateMeasured,HornLength,TipToTip,OtherInfo,DateAvailable,Male,Female,Young,Quantity,Quantity_Lot,SellerId,BuyerId,BiddingPrice,BidDateTime,BidTotalPrice,NewBidPrice,NewBidder,Picture,OnAuction,Sold,DateLoaded,DateSold,PicturePath,PictureName,Increments,ReservePrice,LotQ,DateTimeBid,DateTimeClose,CustomerNumber,BidOpen")] tblRoll tblRoll)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tblRoll).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tblRoll);
        }

        // GET: TempRolls/Delete/5
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

        // POST: TempRolls/Delete/5
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
