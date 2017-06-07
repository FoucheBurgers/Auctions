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
    [Authorize(Roles = "Administrator,Auction Admin")]
    public class AdminBidController : Controller
    {
        
        private AuctionDBEntities db = new AuctionDBEntities();

        // GET: AdminBid
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

            var tblRolls = db.tblRolls.Include(t => t.ltRollDescription).Include(t => t.ltRollDescription1).Include(t => t.ltSpecy).Include(t => t.tblCustomer).Include(t => t.tblCustomer1).Where(t => t.RollId == rollAdminModel.AuctionID && (t.Lot == rollAdminModel.Lot || rollAdminModel.Lot == null));

            return View(tblRolls.ToList());
        }

        //// GET: AdminBid/Bid/5
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
            ViewBag.rollIDBag = tblRoll.RollId;

            ViewBag.RollId = new SelectList(db.ltRollDescriptions, "ID", "Description", tblRoll.RollId);
            ViewBag.BuyerId = new SelectList(db.tblCustomers.OrderBy(t => t.CompanyName), "ID", "CompanyName", tblRoll.BuyerId);

            ViewBag.CustomerNumber = db.BuyerNoes.OrderBy(t => t.BuyerNumber).Where(g => g.CustomerID != null).Select(rr => new SelectListItem { Value = rr.CustomerID.ToString(), Text = rr.BuyerNumber + "   ("+ rr.tblCustomer.CompanyName + ")" }).ToList();

            //ViewBag.CustomerNumber = new SelectList(db.BuyerNoes.OrderBy(t => t.BuyerNumber).Where(g => g.CustomerID != null), "CustomerID", "BuyerNumber", tblRoll.BuyerId);

            return View(tblRoll);
        }

        // POST: AdminBid/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Bid([Bind(Include = "ID,RollId,Lot,SpeciesCode,TagNr,Age,DateMeasured,HornLength,TipToTip,OtherInfo,DateAvailable,Male,Female,Young,Quantity,Quantity_Lot,SellerId,BuyerId,BiddingPrice,BidDateTime,BidTotalPrice,NewBidPrice,NewBidder,Picture,OnAuction,Sold,DateLoaded,DateSold,PicturePath,PictureName,Increments,ReservePrice,LotQ,DateTimeBid,DateTimeClose,CustomerNumber")] tblRoll tblRoll)
        {
            if (ModelState.IsValid)
            {

                BidController bd = new BidController();
                Bid bid = new Bid();
                bid.ID = tblRoll.ID;
                bid.BuyerId = Int32.Parse(tblRoll.CustomerNumber);
                bid.NewBidPrice = tblRoll.NewBidPrice;
                bid.RollId = tblRoll.RollId;

                // Kry die user inligting 
                tblCustomer tblCustomersLog = db.tblCustomers.FirstOrDefault(i => i.CustomerID == User.Identity.Name);
                if (tblCustomersLog != null)
                {
                    bid.UserID = tblCustomersLog.ID; // Kry ID van persoon wat ingelog het.
                }

                Bid dm = bd.BidLogic(bid); // Process bid! 

                if(bid.bidToLow)
                {
                    TempData["msg"] = "<script>alert('Bidding price must be higher than current bid + increment');</script>";
                    return RedirectToAction("Bid", tblRoll.ID);

                }
                if (bid.bidClosed)
                {
                    TempData["msg"] = "<script>alert('Bid has closed');</script>"; // Moet dit toets
                    return RedirectToAction("Bid", tblRoll.ID);
                }

                return RedirectToAction("Index", new { AuctionID = tblRoll.RollId });

            }

            ViewBag.RollId = new SelectList(db.ltRollDescriptions, "ID", "Description", tblRoll.RollId);
            ViewBag.BuyerId = new SelectList(db.tblCustomers, "ID", "CompanyName", tblRoll.BuyerId);
//            ViewBag.CustomerNumber = new SelectList(db.BuyerNoes, "CustomerID", "BuyerNumber", tblRoll.BuyerId);
            ViewBag.CustomerNumber = db.BuyerNoes.OrderBy(t => t.BuyerNumber).Where(g => g.CustomerID != null).Select(rr => new SelectListItem { Value = rr.CustomerID.ToString(), Text = rr.BuyerNumber + "   (" + rr.tblCustomer.CompanyName + ")" }).ToList();

            return View(tblRoll);
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
