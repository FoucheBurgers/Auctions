using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using Rotativa;
using System.Web.Mvc;
using Auctions.Models;

namespace Auctions.Controllers
{
    [Authorize(Roles = "Administrator,Auction Admin")]
    public class BidHistoriesController : Controller
    {
        private AuctionDBEntities db = new AuctionDBEntities();

        // GET: BidHistories
        [AllowAnonymous]
        public ActionResult Index(RollAdminModel rollAdminModel)
        {
            if (Session["SelectedAuctionID"] == null) // IF sesion var not created yet, create 
            {
                Session["SelectedAuctionID"] = "0";
            }
            if (rollAdminModel.AuctionID == 0 || rollAdminModel.AuctionID == null) // kry gegewens vanag default table. 
            {
                BidController bd = new BidController();
                DefaultSetupModel dm = bd.LoadDefs(0); // Get the default values 
                rollAdminModel.AuctionID = dm.DefaultAuction;
            }

            Session["SelectedAuctionID"] = rollAdminModel.AuctionID.ToString(); // Set sesion var met wat geselekteer is

            ViewBag.rollIDBag = rollAdminModel.AuctionID;
            ViewBag.AuctionID = new SelectList(db.ltRollDescriptions, "Id", "Description", rollAdminModel.AuctionID);
            ViewBag.Lot = new SelectList(db.tblRolls.Where(p => p.RollId == rollAdminModel.AuctionID), "Lot", "Lot");


            var bidHistories = db.BidHistories.Include(b => b.tblRoll).Include(b => b.tblCustomer).Include(b => b.tblCustomer1).Include(b => b.ltRollDescription).Include(b => b.tblCustomer2).Where(t => t.RollId == rollAdminModel.AuctionID && (t.Lot == rollAdminModel.Lot || rollAdminModel.Lot == null));
            return View(bidHistories.ToList());
        }


        public ActionResult ExportPDF()
        {
            return new ActionAsPdf("Index", "BidHistories") { FileName = "Test.pdf" };

        }
        // GET: BidHistories/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BidHistory bidHistory = db.BidHistories.Find(id);
            if (bidHistory == null)
            {
                return HttpNotFound();
            }
            return View(bidHistory);
        }

        // GET: BidHistories/Create
        public ActionResult Create()
        {
            ViewBag.LinkID = new SelectList(db.tblRolls, "ID", "Lot");
            ViewBag.SellerId = new SelectList(db.tblCustomers, "ID", "CustomerID");
            ViewBag.BuyerId = new SelectList(db.tblCustomers, "ID", "CustomerID");
            ViewBag.RollId = new SelectList(db.ltRollDescriptions, "ID", "Description");
            ViewBag.UserID = new SelectList(db.tblCustomers, "ID", "CustomerID");
            return View();
        }

        // POST: BidHistories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,LinkID,RollId,Lot,SellerId,BuyerId,BiddingPrice,BidTotalPrice,DateTimeBid,UserID")] BidHistory bidHistory)
        {
            if (ModelState.IsValid)
            {
                db.BidHistories.Add(bidHistory);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.LinkID = new SelectList(db.tblRolls, "ID", "Lot", bidHistory.LinkID);
            ViewBag.SellerId = new SelectList(db.tblCustomers, "ID", "CustomerID", bidHistory.SellerId);
            ViewBag.BuyerId = new SelectList(db.tblCustomers, "ID", "CustomerID", bidHistory.BuyerId);
            ViewBag.RollId = new SelectList(db.ltRollDescriptions, "ID", "Description", bidHistory.RollId);
            ViewBag.UserID = new SelectList(db.tblCustomers, "ID", "CustomerID", bidHistory.UserID);
            return View(bidHistory);
        }

        // GET: BidHistories/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BidHistory bidHistory = db.BidHistories.Find(id);
            if (bidHistory == null)
            {
                return HttpNotFound();
            }
            ViewBag.LinkID = new SelectList(db.tblRolls, "ID", "Lot", bidHistory.LinkID);
            ViewBag.SellerId = new SelectList(db.tblCustomers, "ID", "CustomerID", bidHistory.SellerId);
            ViewBag.BuyerId = new SelectList(db.tblCustomers, "ID", "CustomerID", bidHistory.BuyerId);
            ViewBag.RollId = new SelectList(db.ltRollDescriptions, "ID", "Description", bidHistory.RollId);
            ViewBag.UserID = new SelectList(db.tblCustomers, "ID", "CustomerID", bidHistory.UserID);
            return View(bidHistory);
        }

        // POST: BidHistories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,LinkID,RollId,Lot,SellerId,BuyerId,BiddingPrice,BidTotalPrice,DateTimeBid,UserID")] BidHistory bidHistory)
        {
            if (ModelState.IsValid)
            {
                db.Entry(bidHistory).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.LinkID = new SelectList(db.tblRolls, "ID", "Lot", bidHistory.LinkID);
            ViewBag.SellerId = new SelectList(db.tblCustomers, "ID", "CustomerID", bidHistory.SellerId);
            ViewBag.BuyerId = new SelectList(db.tblCustomers, "ID", "CustomerID", bidHistory.BuyerId);
            ViewBag.RollId = new SelectList(db.ltRollDescriptions, "ID", "Description", bidHistory.RollId);
            ViewBag.UserID = new SelectList(db.tblCustomers, "ID", "CustomerID", bidHistory.UserID);
            return View(bidHistory);
        }

        // GET: BidHistories/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BidHistory bidHistory = db.BidHistories.Find(id);
            if (bidHistory == null)
            {
                return HttpNotFound();
            }
            return View(bidHistory);
        }

        // POST: BidHistories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            BidHistory bidHistory = db.BidHistories.Find(id);
            db.BidHistories.Remove(bidHistory);
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
