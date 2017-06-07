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
    public class RollAdminController : Controller
    {
        private AuctionDBEntities db = new AuctionDBEntities();

        [Authorize(Roles = "Administrator")]

        // GET: RollAdmin
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

            var tblRolls = db.tblRolls.Include(t => t.ltRollDescription).Include(t => t.ltRollDescription1).Include(t => t.ltSpecy).Include(t => t.tblCustomer).Include(t => t.tblCustomer1).Where(t => t.RollId == rollAdminModel.AuctionID);

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
            int? AuctionID;
            if (Session["SelectedAuctionID"] == null) // IF sesion var not created yet, create 
            {
                BidController bd = new BidController();
                DefaultSetupModel dm = bd.LoadDefs(0); // Get the default values 
                AuctionID = dm.DefaultAuction;
                Session["SelectedAuctionID"] = AuctionID.ToString(); // Set sesion var met wat geselekteer is
            }
            else
            {
                AuctionID = Int32.Parse(Session["SelectedAuctionID"].ToString());
            }

            ViewBag.auctionID = AuctionID;
            ViewBag.RollId = new SelectList(db.ltRollDescriptions, "ID", "Description", AuctionID);

            //            ViewBag.RollId = new SelectList(db.ltRollDescriptions, "ID", "Description");
            ViewBag.SpeciesCode = new SelectList(db.ltSpecies, "ID", "Description");
            ViewBag.BuyerId = new SelectList(db.tblCustomers, "ID", "CompanyName");
            ViewBag.SellerId = new SelectList(db.tblCustomers, "ID", "CompanyName");
            tblRoll rol = new tblRoll();
            ltRollDescription ltRollDescription = db.ltRollDescriptions.Find(AuctionID);
            if (ltRollDescription != null)
            {
                rol.Quantity_Lot = "Animal";
                rol.OnAuction = true;
                rol.DateTimeClose = ltRollDescription.EndDate;
                rol.DateLoaded = DateTime.Now;
                rol.BidOpen = true;
            }
             
            return View(rol);
        }

        // POST: RollAdmin/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,RollId,Lot,SpeciesCode,TagNr,Age,DateMeasured,HornLength,TipToTip,OtherInfo,DateAvailable,Male,Female,Young,Quantity,Quantity_Lot,SellerId,BuyerId,BiddingPrice,BidDateTime,BidTotalPrice,NewBidPrice,NewBidder,Picture,OnAuction,Sold,DateLoaded,DateSold,PicturePath,PictureName,Increments,ReservePrice,LotQ,DateTimeClose,BidOpen")] tblRoll tblRoll)
        {
            if (ModelState.IsValid)
            {

                db.tblRolls.Add(tblRoll);
                db.SaveChanges();
                return RedirectToAction("Create", new { AuctionID = tblRoll.RollId });
            }

            //ViewBag.RollId = new SelectList(db.ltRollDescriptions, "ID", "Description", tblRoll.RollId);
            ViewBag.RollId = new SelectList(db.ltRollDescriptions, "ID", "Description", tblRoll.RollId);
            ViewBag.SpeciesCode = new SelectList(db.ltSpecies, "ID", "Description", tblRoll.SpeciesCode);
            ViewBag.BuyerId = new SelectList(db.tblCustomers, "ID", "CompanyName", tblRoll.BuyerId);
            ViewBag.SellerId = new SelectList(db.tblCustomers, "ID", "CompanyName", tblRoll.SellerId);
            tblRoll.DateLoaded = DateTime.Now;
            Session["SelectedAuctionID"] = tblRoll.RollId.ToString(); // Set sesion var met wat geselekteer is
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
        public ActionResult Edit([Bind(Include = "ID,RollId,Lot,SpeciesCode,TagNr,Age,DateMeasured,HornLength,TipToTip,OtherInfo,DateAvailable,Male,Female,Young,Quantity,Quantity_Lot,SellerId,BuyerId,BiddingPrice,BidDateTime,BidTotalPrice,NewBidPrice,NewBidder,Picture,OnAuction,Sold,DateLoaded,DateSold,PicturePath,PictureName,Increments,ReservePrice,LotQ,CustomerNumber,DateTimeBid,DateTimeClose,BidOpen")] tblRoll tblRoll)
        {
            if (ModelState.IsValid)
            {
                Session["SelectedAuctionID"] = tblRoll.RollId.ToString(); // Set sesion var met wat geselekteer is
                db.Entry(tblRoll).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new { AuctionID = tblRoll.RollId });
            }
            ViewBag.RollId = new SelectList(db.ltRollDescriptions, "ID", "Description", tblRoll.RollId);
            ViewBag.RollId = new SelectList(db.ltRollDescriptions, "ID", "Description", tblRoll.RollId);
            ViewBag.SpeciesCode = new SelectList(db.ltSpecies, "ID", "Description", tblRoll.SpeciesCode);
            //            ViewBag.SpeciesCode = new SelectList(db.Species, "ID", "NameAfr", tblRoll.SpeciesCode);
            ViewBag.BuyerId = new SelectList(db.tblCustomers, "ID", "CompanyName", tblRoll.BuyerId);
            ViewBag.SellerId = new SelectList(db.tblCustomers, "ID", "CompanyName", tblRoll.SellerId);
            Session["SelectedAuctionID"] = tblRoll.RollId.ToString(); // Set sesion var met wat geselekteer is
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
