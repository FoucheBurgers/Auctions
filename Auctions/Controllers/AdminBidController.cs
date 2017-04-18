using Auctions.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Auctions.Controllers
{
    public class AdminBidController : Controller
    {
        // GET: AdminBid
        private AuctionDBEntities db = new AuctionDBEntities();
        public ActionResult Index()
        {
            ViewBag.LotID = new SelectList(db.tblRolls, "Id", "Lot");
            ViewBag.BuyerID = new SelectList(db.tblCustomers, "Id", "CustomerNumber").Where(t => t.Text != null && t.Text != "");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(AdminBidModel bid)
        {
            int lot = bid.LotID;
            int Buyer = bid.BuyerID;

            //

            tblRoll tblRollAfter = db.tblRolls.Find(bid.LotID);

            if (bid.NewBidPrice >= (tblRollAfter.BiddingPrice + tblRollAfter.Increments))
            {

                tblCustomer tblCustomersLog = db.tblCustomers.FirstOrDefault(i => i.CustomerID == User.Identity.Name);
                //tblRoll.BiddingPrice = bid.NewBidPrice;
                //tblRoll.BidTotalPrice = bid.NewBidPrice * tblRollAfter.Quantity;


                if (DateTime.Now <= tblRollAfter.DateTimeClose)
                {
                    //tblRoll.DateTimeBid = DateTime.Now;
                    if (DateTime.Now.AddMinutes(15) >= tblRollAfter.DateTimeClose)
                    {
                        //tblRoll.DateTimeClose = DateTime.Now.AddMinutes(15); // tel 15 minute by sluitings tyd. 
                        db.UpdateBidTimeClose(bid.LotID, DateTime.Now.AddMinutes(15));
                    }
                    db.UpdateBidder(bid.LotID, bid.BuyerID, bid.NewBidPrice, (bid.NewBidPrice * tblRollAfter.Quantity), DateTime.Now);
                    db.AddBidHistory(tblRollAfter.ID, tblRollAfter.RollId, tblRollAfter.Lot, tblRollAfter.SellerId, bid.BuyerID, bid.NewBidPrice, (bid.NewBidPrice * tblRollAfter.Quantity), DateTime.Now, tblCustomersLog.ID);
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



            //

            ViewBag.LotID = new SelectList(db.tblRolls, "Id", "Lot");
            ViewBag.BuyerID = new SelectList(db.tblCustomers, "Id", "CustomerNumber").Where(t => t.Text != null && t.Text !="");

            return View();
        }
    }
}