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
//    [Authorize(Roles = "Administrator")]
    [Authorize(Roles = "Administrator,Auction Admin")]
    public class BuyerNumberController : Controller
    {
        
        private AuctionDBEntities db = new AuctionDBEntities();
       
        // GET: BuyerNumber
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

            ViewBag.AuctionID = new SelectList(db.ltRollDescriptions, "Id", "Description", rollAdminModel.AuctionID);
            ViewBag.rollIDBag = rollAdminModel.AuctionID;

            var buyerNoes = db.BuyerNoes.Include(b => b.tblCustomer).Include(b => b.ltRollDescription).Where(t => t.RollID == rollAdminModel.AuctionID);


            return View(buyerNoes.ToList());
        }

        // GET: BuyerNumber/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BuyerNo buyerNo = db.BuyerNoes.Find(id);
            if (buyerNo == null)
            {
                return HttpNotFound();
            }
            ViewBag.rollIDBag = buyerNo.RollID;
            return View(buyerNo);
        }

        // GET: BuyerNumber/Create
        public ActionResult Create(RollAdminModel rollAdminModel)
        {

            if (Session["SelectedAuctionID"] == null) // behoort te bestaan
            {
                BidController bd = new BidController();
                DefaultSetupModel dm = bd.LoadDefs(0); // Get the default values 
                rollAdminModel.AuctionID = dm.DefaultAuction;
                Session["SelectedAuctionID"] = rollAdminModel.AuctionID.ToString();
            }
            else
            {
                int Selectedauction = Int32.Parse(Session["SelectedAuctionID"].ToString());
                rollAdminModel.AuctionID = Selectedauction;
            }

            ViewBag.CustomerID = new SelectList(db.tblCustomers, "ID", "CompanyName");
            ViewBag.RollID = new SelectList(db.ltRollDescriptions, "ID", "Description", rollAdminModel.AuctionID);
            ViewBag.rollIDBag = rollAdminModel.AuctionID;
            return View();
        }

        // POST: BuyerNumber/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,CustomerID,RollID,BuyerNumber")] BuyerNo buyerNo)
        {
            if (ModelState.IsValid)
            {
                // Kyk vir duplicates 
                using (AuctionDBEntities db = new AuctionDBEntities()) // make sure to get the latest info from DB and not the immage of DB.  
                {
                    BuyerNo buyerNoAfter = db.BuyerNoes.FirstOrDefault(i => i.RollID == buyerNo.RollID && i.BuyerNumber == buyerNo.BuyerNumber);
                    if(buyerNoAfter != null) // Lot bestaan reeds 
                    {
                        TempData["msg"] = "<script>alert('Lot number already exists');</script>"; // Moet dit toets
                        int x = 1;
                        return RedirectToAction("Create", new { AuctionID = buyerNo.RollID });
//                        return View(buyerNo);
                    }
                }
                db.BuyerNoes.Add(buyerNo);
                db.SaveChanges();
                return RedirectToAction("Index", new { AuctionID = buyerNo.RollID });
            }

            ViewBag.CustomerID = new SelectList(db.tblCustomers, "ID", "CompanyName", buyerNo.CustomerID);
            ViewBag.RollID = new SelectList(db.ltRollDescriptions, "ID", "Description", buyerNo.RollID);
            return View(buyerNo);
        }

        // GET: BuyerNumber/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BuyerNo buyerNo = db.BuyerNoes.Find(id);

            if (buyerNo == null)
            {
                return HttpNotFound();
            }
            ViewBag.CustomerID = new SelectList(db.tblCustomers, "ID", "CompanyName", buyerNo.CustomerID);
            ViewBag.RollID = new SelectList(db.ltRollDescriptions, "ID", "Description", buyerNo.RollID);
            ViewBag.rollIDBag = buyerNo.RollID;

            return View(buyerNo);
        }

        // POST: BuyerNumber/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,CustomerID,RollID,BuyerNumber")] BuyerNo buyerNo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(buyerNo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new { AuctionID = buyerNo.RollID });
            }
            ViewBag.CustomerID = new SelectList(db.tblCustomers, "ID", "CompanyName", buyerNo.CustomerID);
            ViewBag.RollID = new SelectList(db.ltRollDescriptions, "ID", "Description", buyerNo.RollID);
            return View(buyerNo);
        }

        // GET: BuyerNumber/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BuyerNo buyerNo = db.BuyerNoes.Find(id);
            if (buyerNo == null)
            {
                return HttpNotFound();
            }
            return View(buyerNo);
        }

        // POST: BuyerNumber/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            BuyerNo buyerNo = db.BuyerNoes.Find(id);
            int? rollid = buyerNo.RollID;
            db.BuyerNoes.Remove(buyerNo);
            db.SaveChanges();
            return RedirectToAction("Index", new { AuctionID = rollid });
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
