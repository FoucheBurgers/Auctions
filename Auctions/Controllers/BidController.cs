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
    [Authorize]
    public class BidController : Controller
    {
        private AuctionDBEntities db = new AuctionDBEntities();

        //    [Authorize(Roles="Bidder")]
        //        [Authorize] // all registered users
        // GET: Bid
        public ActionResult Index()
        {
            var tblRolls = db.tblRolls.Include(t => t.ltRollDescription).Include(t => t.ltRollDescription1).Include(t => t.ltSpecy).Include(t => t.tblCustomer).Include(t => t.tblCustomer1).Where(t => t.OnAuction && t.DateTimeClose >= DateTime.Now);
            return View(tblRolls.ToList());
        }
        // GET: Bid/Edit/5
        public ActionResult Bid(int? id)
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
            ViewBag.BuyerId = new SelectList(db.tblCustomers, "ID", "CustomerID", tblRoll.BuyerId);
            ViewBag.SellerId = new SelectList(db.tblCustomers, "ID", "CustomerID", tblRoll.SellerId);

            if (tblRoll.BiddingPrice == null)
            {
                tblRoll.BiddingPrice = 0;
            }
            if (tblRoll.Increments == null)
            {
                tblRoll.Increments = 0;
            }
            if (tblRoll.Quantity == null)
            {
                tblRoll.Quantity = 1;
            }

            return View(tblRoll);
        }

        // POST: Bid/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Bid([Bind(Include = "ID,RollId,Lot,SpeciesCode,TagNr,Age,DateMeasured,HornLength,TipToTip,OtherInfo,DateAvailable,Male,Female,Young,Quantity,Quantity_Lot,SellerId,BuyerId,BiddingPrice,BidDateTime,BidTotalPrice,NewBidPrice,NewBidder,Picture,OnAuction,Sold,DateLoaded,DateSold,PicturePath,PictureName,Increments,ReservePrice,LotQ, DateTimeClose")] tblRoll tblRoll)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tblRoll).State = EntityState.Modified;

                tblRoll tblRollAfter = db.tblRolls.Find(tblRoll.ID); // die een werk amper
                if (tblRoll.NewBidPrice >= (tblRollAfter.BiddingPrice + tblRoll.Increments))
                {
                    tblCustomer tblCustomersLog = db.tblCustomers.FirstOrDefault(i => i.CustomerID == User.Identity.Name);
                    if (tblCustomersLog != null)
                    {
                        tblRoll.BuyerId = tblCustomersLog.ID; // Kry ID van persoon wat ingelog het.
                    }
                    else
                    {
                        tblRoll.BuyerId = 1; // default user. Moet dalk nog error code hier skryf. 
                    }
                    tblRoll.BiddingPrice = tblRoll.NewBidPrice;
                    tblRoll.BidTotalPrice = tblRoll.NewBidPrice * tblRoll.Quantity;
                    if (DateTime.Now <= tblRoll.DateTimeClose)
                    {
                        tblRoll.DateTimeBid = DateTime.Now;
                        if (DateTime.Now.AddMinutes(15) >= tblRoll.DateTimeClose)
                        {
                            tblRoll.DateTimeClose = DateTime.Now.AddMinutes(15); // add 15 minute by sluitings tyd. 
                            db.UpdateBidTimeClose(tblRoll.ID, tblRoll.DateTimeClose);
                        }
                        db.UpdateBidder(tblRoll.ID, tblRoll.BuyerId, tblRoll.BiddingPrice, tblRoll.BidTotalPrice, tblRoll.DateTimeBid);
                        db.AddBidHistory(tblRoll.ID, tblRoll.RollId, tblRoll.Lot, tblRoll.SellerId, tblRoll.BuyerId, tblRoll.BiddingPrice, tblRoll.BidTotalPrice, tblRoll.DateTimeBid, tblRoll.BuyerId);
                        smsMGT smg = new smsMGT();
                        string messagestring = $"This was the last price : {tblRoll.BiddingPrice}";
                        var res = smg.SendSingleSMS("12345","083449343",messagestring);

                        var c = 1;
                    }
                    else
                    {
                        TempData["msg"] = "<script>alert('Bid has closed');</script>"; // Moet dit toets
                    }
                }
                else
                {
                    TempData["msg"] = "<script>alert('Bidding price must be higher than current bid + increment');</script>";
                }

                return RedirectToAction("Bid", tblRollAfter.ID);

                //ViewBag.RollId = new SelectList(db.ltRollDescriptions, "ID", "Description", tblRoll.RollId);
                ////ViewBag.RollId = new SelectList(db.ltRollDescriptions, "ID", "Description", tblRoll.RollId);
                //ViewBag.SpeciesCode = new SelectList(db.ltSpecies, "ID", "Description", tblRoll.SpeciesCode);
                //ViewBag.BuyerId = new SelectList(db.tblCustomers, "ID", "CustomerID", tblRoll.BuyerId);
                //ViewBag.SellerId = new SelectList(db.tblCustomers, "ID", "CustomerID", tblRoll.SellerId);

            }
            else
            {
                return View(tblRoll);
            }
        }

        // GET: Bid/Delete/5
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

        // POST: Bid/Delete/5
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
