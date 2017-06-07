using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Auctions.Models;
using Microsoft.AspNet.Identity;

namespace Auctions.Controllers
{
    [Authorize]
    public class BidController : Controller
    {
        private AuctionDBEntities db = new AuctionDBEntities();
        private CurrentBuyer currentBuyer = new CurrentBuyer();

        // GET: Bid

        public ActionResult Index(int? AuctionID)
        {
            // Get defauls 


            string sessionID = "0";
            if (AuctionID != null && AuctionID != 0) // Eerste keer
            {
                Session["auctionID"] = AuctionID;
            }
            else // Daar na sal Session 'n waarde he. 
            {
                sessionID = Session["auctionID"].ToString();
                AuctionID = Int32.Parse(sessionID);
            }
            if ((AuctionID == null || AuctionID == 0) && Session["auctionID"] == null) // Must have selected an auction.
            {
                // Gaan terug na roll
                TempData["msg"] = "<script>alert('Select an auction first');</script>";
                return RedirectToAction("Index", "Home");
            }

            sessionID = Session["auctionID"].ToString();

            DefaultSetupModel dm = LoadDefs(AuctionID); // Get the default values

            int? rollID = dm.ID;
            ViewBag.BackgroundColor = dm.BackgroundColor;
            ViewBag.TexColor = dm.FontColor;
            ViewBag.LogoBackgroundColor = dm.LogoBackgroundColor;
            ViewBag.ImagePath = dm.RollImagePath;
            ViewBag.LogoPath = dm.LogoPath;
            ViewBag.LogoName = dm.LogoName;
            ViewBag.RefreshTime = dm.RefreshTime.ToString();
            ViewBag.ImagePath = dm.RollImagePath;
            ViewBag.emptyMessage = dm.message;
            ViewBag.RollImageBackColor = dm.RollImagesBackColor;
            ViewBag.RollActionBidColor = dm.RollActionBidColor;
            ViewBag.RollActionBackIndexColor = dm.RollActionBackIndexColor;


        var tblRolls = db.tblRolls.Include(t => t.ltRollDescription).Include(t => t.ltRollDescription1).Include(t => t.ltSpecy).Include(t => t.tblCustomer).Include(t => t.tblCustomer1).Where(t => t.OnAuction && t.DateTimeClose >= DateTime.Now && t.RollId == rollID);

            //tblRoll time = new tblRoll();
            //ViewBag.TimeSpan = time.DateTimeClose - DateTime.Now;

            string initApp;
            if (Session["InitApp"] == null)
            {
                Session["InitApp"] = "Web"; // creat InitApp as web init if nothing received 
            }
            initApp = Session["InitApp"].ToString();

            if (initApp == "App")
            {
                return View("IndexApp", tblRolls.ToList());
            }
            else
            {
                return View(tblRolls.ToList()); // Web page 
            }
        }

        // GET: Bid/Edit/5
        public ActionResult Bid(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblRoll tblRoll = db.tblRolls.Find(id);

            // store outgoing bidders details 

            currentBuyer.NewBuyerID = tblRoll.BuyerId;  // get the new buyer buyer's ID 

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

            // Get defauls 
            string sessionID = "0";
            int AuctionID = 0;

            if (Session["auctionID"] != null) // Should never be null
            {
                sessionID = Session["auctionID"].ToString();
                AuctionID = Int32.Parse(sessionID);
            }

            DefaultSetupModel dm = LoadDefs(AuctionID); // Get the default values 

            int? rollID = dm.ID;
            ViewBag.BackgroundColor = dm.BackgroundColor;
            ViewBag.TexColor = dm.FontColor;
            ViewBag.LogoBackgroundColor = dm.LogoBackgroundColor;
            ViewBag.ImagePath = dm.RollImagePath;
            ViewBag.LogoPath = dm.LogoPath;
            ViewBag.LogoName = dm.LogoName;
            ViewBag.RefreshTime = dm.RefreshTime.ToString();
            ViewBag.ImagePath = dm.RollImagePath;
            ViewBag.emptyMessage = dm.message;
            ViewBag.RollImageBackColor = dm.RollImagesBackColor;

            string initApp;

            if (Session["InitApp"] == null)
            {
                Session["InitApp"] = "Web"; // creat InitApp as web init if nothing received 
            }
            initApp = Session["InitApp"].ToString();

            if (initApp == "App")
            {
                return View("BidApp", tblRoll);
            }
            else
            {
                return View(tblRoll);
            }
        }

        // POST: Bid/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Bid([Bind(Include = "ID,RollId,Lot,SpeciesCode,TagNr,Age,DateMeasured,HornLength,TipToTip,OtherInfo,DateAvailable,Male,Female,Young,Quantity,Quantity_Lot,SellerId,BuyerId,BiddingPrice,BidDateTime,BidTotalPrice,NewBidPrice,NewBidder,Picture,OnAuction,Sold,DateLoaded,DateSold,PicturePath,PictureName,Increments,ReservePrice,LotQ,DateTimeClose,CustomerNumber")] tblRoll tblRoll)
        {
            if (ModelState.IsValid)
            {

                // nuut
                BidController bd = new BidController();
                Bid bid = new Bid();
                bid.ID = tblRoll.ID;
                bid.NewBidPrice = tblRoll.NewBidPrice;
                bid.RollId = tblRoll.RollId;
                // Kry die user inligting 

                //                var user = UserManager.FindByNameAsync("PIET");

                tblCustomer tblCustomersLog = db.tblCustomers.FirstOrDefault(i => i.CustomerID == User.Identity.Name);

                if (tblCustomersLog.PIN != tblRoll.CustomerNumber)
                {
                    TempData["msg"] = "<script>alert('Incorrect PIN');</script>";
                    return RedirectToAction("Bid", tblRoll.ID);

                }
                if (tblCustomersLog != null)
                {
                    bid.BuyerId = tblCustomersLog.ID; // Kry ID van persoon wat ingelog het.
                    bid.UserID = tblCustomersLog.ID; // Buyer is ook User.
                }

                Bid dm = bd.BidLogic(bid); // Process bid! 

                if (bid.bidToLow)
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

                // end nuut

            }
            else
            {
                string initApp;

                if (Session["InitApp"] == null)
                {
                    Session["InitApp"] = "Web"; // creat InitApp as web init if nothing received 
                }
                initApp = Session["InitApp"].ToString();

                if (initApp == "App")
                {
                    return View("BidApp", tblRoll);
                }
                else
                {
                    return View(tblRoll);
                }
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

        public DefaultSetupModel LoadDefs(int? AuctionID)
        {
            DefaultSetupModel ds = new DefaultSetupModel();

            // Laai eers alles vanaf DefaultSetup table. 

            DefaultSetup defaultSetup = db.DefaultSetups.FirstOrDefault(t => t.Active == true);
            if (defaultSetup != null) // het gekry.
            {
                ds.ID = AuctionID; // Het gekry en rollID is dus reg. 
                ds.Description = defaultSetup.Description;
                ds.BackgroundColor = defaultSetup.BackgroundColor;
                ds.FontColor = defaultSetup.FontColor;
                ds.LogoBackgroundColor = defaultSetup.LogoBackgroundColor;
                ds.RefreshTime = defaultSetup.RefreshTime.ToString();
                ds.RollImagePath = defaultSetup.RollImagesPath;
                ds.Active = defaultSetup.Active;
                ds.LogoPath = defaultSetup.LogoPath;
                ds.LogoName = defaultSetup.LogoName;
                ds.SmallLogoName = defaultSetup.LogoPath;
                ds.RollImagePath = defaultSetup.RollImagesPath;
                ds.AuctionDelayTime = defaultSetup.AuctionDelayTime.ToString();
                ds.SMSNewBidder = defaultSetup.SmsNotification;
                ds.SMSAllFunctionality = defaultSetup.SMSAllFunctionality;
                ds.SMSCustRegistration = defaultSetup.SMSCustRegistration;
                ds.SMSConfirmationLoginRequired = defaultSetup.SMSConfirmationLoginRequired;
                ds.emailConfirmRegistration = defaultSetup.emailConfirmRegistration;
                ds.emailConfirmationLoginRequired = defaultSetup.emailConfirmationLoginRequired;
                ds.BackgroundColorHome = defaultSetup.BackgroundColorHome;
                ds.DispLines = defaultSetup.DispLines;
                ds.DispColumns = defaultSetup.DispColumns;
                ds.pageSize = ds.DispLines * ds.DispColumns;
                ds.RollDispRefreshRate = defaultSetup.RollDispRefreshRate;

                ds.HomePeriodDescription = defaultSetup.HomePeriodDescription;
                ds.HomePeriodDescriptionTextColor = defaultSetup.HomePeriodDescriptionTextColor;
                ds.HomePeriodDescriptionBackColor = defaultSetup.HomePeriodDescriptionBackColor;
                ds.ActionTextColor = defaultSetup.ActionTextColor;
                ds.ActionBackColor = defaultSetup.ActionBackColor;



                if (defaultSetup.DefaultAuction != null)
                {
                    ds.DefaultAuction = defaultSetup.DefaultAuction;
                }
                else
                {
                    ds.DefaultAuction = 0;
                }
                ds.SMSOutBidder = defaultSetup.smsOutBidder;


                ds.message = "Please select an Auction";
            }

            if (AuctionID != 0) // Laai rol spesifieke inligting 
            {
                ltRollDescription ltRollDescriptions = db.ltRollDescriptions.Find(AuctionID);
                if (ltRollDescriptions != null) // het gekry.
                {
                    ds.ID = AuctionID; // Het gekry en rollID is dus reg. 
                    ds.AuctionID = AuctionID.ToString(); // String van ID
                    ds.Description = ltRollDescriptions.Description;
                    ds.BackgroundColor = ltRollDescriptions.BackgroundColor;
                    ds.FontColor = ltRollDescriptions.FontColor;
                    ds.LogoBackgroundColor = ltRollDescriptions.LogoBackgroundColor;
                    ds.RefreshTime = ltRollDescriptions.RefreshTime.ToString();
                    ds.RollImagePath = ltRollDescriptions.RollImagesPath;
                    ds.Active = ltRollDescriptions.Active;
                    ds.LogoPath = ltRollDescriptions.LogoPath;
                    ds.LogoName = ltRollDescriptions.LogoName;
                    ds.SmallLogoName = ltRollDescriptions.LogoPath;
                    ds.RollImagePath = ltRollDescriptions.RollImagesPath;
                    ds.AuctionDelayTime = ltRollDescriptions.AuctionDelayTime.ToString();
                    ds.SMSNewBidder = ltRollDescriptions.SmsNotification;
                    ds.SMSOutBidder = ltRollDescriptions.smsOutBidder;
                    ds.RollImagesBackColor = ltRollDescriptions.RollImagesBackColor;
                    ds.RollActionBidColor = ltRollDescriptions.RollActionBidColor;
                    ds.RollActionBackIndexColor = ltRollDescriptions.RollActionBackIndexColor;

        ds.HomePeriodDescription = ltRollDescriptions.HomePeriodDescription;
                    ds.HomePeriodDescriptionTextColor = ltRollDescriptions.HomePeriodDescriptionTextColor;
                    ds.HomePeriodDescriptionBackColor = ltRollDescriptions.HomePeriodDescriptionBackColor;
                    ds.ActionTextColor = ltRollDescriptions.ActionTextColor;
                    ds.ActionBackColor = ltRollDescriptions.ActionBackColor;

                    ds.message = "Roll not loaded yet. Will be available soon!";
                }
            }
            return (ds);
        }

        // Hier
        public Bid BidLogic(Bid bid)
        {
            currentBuyer.NewBuyerHaveCellNumber = false;
            currentBuyer.OutBuyerHaveCellNumber = false;
            currentBuyer.NewBuyerID = bid.BuyerId; // Die nuwe id wat oorkom.

            using (AuctionDBEntities db = new AuctionDBEntities()) // make sure to get the latest info from DB and not the immage of DB.  
            {

                tblRoll tblRollAfter = db.tblRolls.Find(bid.ID); // find the latest info on db. 
                if (tblRollAfter.Increments == null)
                {
                    tblRollAfter.Increments = 0;
                }
                if (tblRollAfter.BiddingPrice == null)
                {
                    tblRollAfter.BiddingPrice = 0;
                }
                if (bid.NewBidPrice >= (tblRollAfter.BiddingPrice + tblRollAfter.Increments))
                {
                    double ExtendBitTime;
                    bool AuctionSMS = false;
                    ltRollDescription ltRollDescriptions = db.ltRollDescriptions.Find(bid.RollId);
                    if (ltRollDescriptions != null)
                    {
                        ExtendBitTime = System.Convert.ToDouble(ltRollDescriptions.AuctionDelayTime);
                        AuctionSMS = ltRollDescriptions.SmsNotification;
                    }
                    else
                    {
                        ExtendBitTime = 5;
                        AuctionSMS = false;
                    }

                    //

                    bid.BidTotalPrice = bid.NewBidPrice * tblRollAfter.Quantity;
                    if (DateTime.Now <= tblRollAfter.DateTimeClose)
                    {
                        tblRollAfter.DateTimeBid = DateTime.Now;

                        if (DateTime.Now.AddMinutes(ExtendBitTime) >= tblRollAfter.DateTimeClose)
                        {
                            tblRollAfter.DateTimeClose = DateTime.Now.AddMinutes(ExtendBitTime); // add time before close. 
                            db.UpdateBidTimeClose(bid.ID, tblRollAfter.DateTimeClose);
                        }
                        // Kan later weer terugsit.
                        //if (currentBuyer.NewBuyerID != tblRollAfter.BuyerId) // Not same outgoing and new bidder. 
                        //{
                        // Get outgoing bidder info
                        tblCustomer tblCustomersOut = db.tblCustomers.Find(tblRollAfter.BuyerId);
                        currentBuyer.OutBuyerCell = tblCustomersOut.Phone;
                        if (currentBuyer.OutBuyerCell != null && currentBuyer.OutBuyerCell != "")
                        {
                            currentBuyer.OutBuyerHaveCellNumber = true;
                        }

                        // Get incomming bidder info

                        tblCustomer tblCustomersIn = db.tblCustomers.Find(currentBuyer.NewBuyerID);
                        currentBuyer.NewBuyerCell = tblCustomersIn.Phone;
                        if (currentBuyer.NewBuyerCell != null && currentBuyer.NewBuyerCell != "")
                        {
                            currentBuyer.NewBuyerHaveCellNumber = true;
                        }
                        //                        }

                        // Huidige koper

                        // Laaste voor update. 
                        tblRoll tblRollLast = db.tblRolls.Find(bid.ID); // find the latest info on db. 
                        if (tblRollLast.Increments == null)
                        {
                            tblRollLast.Increments = 0;
                        }
                        if (tblRollLast.BiddingPrice == null)
                        {
                            tblRollLast.BiddingPrice = 0;
                        }

                        if (bid.NewBidPrice >= (tblRollLast.BiddingPrice + tblRollLast.Increments))
                        {

                            if (tblRollLast.Quantity_Lot == "Lot")
                            {
                                bid.BidTotalPrice = bid.NewBidPrice;
                            }
                            else
                            {
                                bid.BidTotalPrice = bid.NewBidPrice * tblRollAfter.Quantity;
                            }

                            // Kry die Auction Buyer Nommer

                            string AuctionBuyerNumber;

                            BuyerNo buyeNo = db.BuyerNoes.FirstOrDefault(t => t.CustomerID == bid.BuyerId && t.RollID == bid.RollId);
                            if (buyeNo == null)
                            {
                                AuctionBuyerNumber = "INT "+bid.BuyerId.ToString();
                            }
                            else
                            {
                                AuctionBuyerNumber = "B " + buyeNo.BuyerNumber;
                            }

                            db.UpdateBidder(bid.ID, bid.BuyerId, bid.NewBidPrice, bid.BidTotalPrice, tblRollAfter.DateTimeBid, AuctionBuyerNumber);
                            db.AddBidHistory(bid.ID, bid.RollId, tblRollAfter.Lot, tblRollAfter.SellerId, bid.BuyerId, bid.NewBidPrice, bid.BidTotalPrice, tblRollAfter.DateTimeBid, bid.UserID);

                            // Laai defaults 

                            DefaultSetupModel dm = LoadDefs(0); // Get the default values vir stuur van sms 

                            // Sal net sms stuur as koper cell het, spesifieke auction verys dit en default is aan.

                            // Sms outgoing bidder
                            if (currentBuyer.OutBuyerHaveCellNumber && AuctionSMS && dm.SMSAllFunctionality == true && dm.SMSOutBidder == true)
                            {
                                string x = string.Format("{0:0,000}", bid.NewBidPrice);

                                smsMGT smg = new smsMGT();
                                string messagestring = $"Silent auction: Your bid for Lot {tblRollAfter.Lot} has been exceeded. Current bid is : R {x}";
                                var res = smg.SendSingleSMS("1", currentBuyer.OutBuyerCell, messagestring);
                                if (res != "success")
                                {
                                    string EmailSubject = "SMS NOT sent successfully";
                                    string EMailBody = $"SMS NOT sent successfully to out bidder {tblCustomersOut.CompanyName}" + ".  Error = " + res;
                                    sendEmail sm = new sendEmail();
                                    string emalres = sm.SendEmailFB("support@nwwt.co.za", EmailSubject, EMailBody, null, null);
                                }
                            }

                            // Sms new bidder. 
                            if (currentBuyer.NewBuyerHaveCellNumber && AuctionSMS && dm.SMSAllFunctionality == true && dm.SMSNewBidder == true)
                            {
                                string y = string.Format("{0:0,000}", bid.NewBidPrice);

                                smsMGT smg = new smsMGT();
                                string messagestring = $"Silent auction: Your bid for Lot {tblRollAfter.Lot} has been accepted. Your bid is : R {y}";
                                var res = smg.SendSingleSMS("1", currentBuyer.NewBuyerCell, messagestring);

                                if (res != "success")
                                {
                                    string EmailSubject = "SMS NOT sent successfully";
                                    string EMailBody = $"SMS NOT sent successfully to new bidder {tblCustomersIn.CompanyName}" + "  " + res;
                                    sendEmail sm = new sendEmail();
                                    string emalres = sm.SendEmailFB("support@nwwt.co.za", EmailSubject, EMailBody, null, null);
                                }

                            }
                            if (bid.NewBidPrice >= (tblRollAfter.BiddingPrice + (tblRollAfter.Increments * 10)))
                            {
                                // email warning to NWWT 
                                sendEmail sm = new sendEmail();
                                string y = string.Format("{0:0,000}", bid.NewBidPrice);
                                string z = string.Format("{0:0,000}", tblRollAfter.BiddingPrice);
                                string EmailSubject = "An extraordinary high bid received";
                                string EMailBody = $"An extraordinary high bid was made on Lot {tblRollAfter.Lot} by {tblCustomersIn.CompanyName}. The new bid is : R {y} and the previous bid was R {z}";
                                string res = sm.SendEmailFB("support@nwwt.co.za", EmailSubject, EMailBody, null, null);
                            }
                        }
                        else
                        {
                            bid.bidToLow = true;
                        }
                    }
                    else
                    {
                        bid.bidClosed = true;
                    }
                }
                else
                {
                    bid.bidToLow = true;
                }
            }
            return (bid);
        }
    }
    // end

}

