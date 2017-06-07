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
    public class AuctionRollDisplayController : Controller
    {
        private AuctionDBEntities db = new AuctionDBEntities();
        // GET: AuctionRollDisplay
        public ActionResult Index()
        {
            string sessionID = "0";

            if (Session["auctionID"] == null) // Must have selected an auction.
            {
                // Gaan terug na roll
                TempData["msg"] = "<script>alert('Select an auction first');</script>";
                return RedirectToAction("Index", "Home");
            }

            sessionID = Session["auctionID"].ToString();
            int AuctionID = AuctionID = Int32.Parse(sessionID);

            BidController bd = new BidController();
            DefaultSetupModel dm = bd.LoadDefs(AuctionID); // Get the default values 

            int? rollID = dm.ID;
            ViewBag.BackgroundColor = dm.BackgroundColor;
            ViewBag.TexColor = dm.FontColor;
            ViewBag.LogoBackgroundColor = dm.LogoBackgroundColor;
            ViewBag.ImagePath = dm.RollImagePath;
            ViewBag.LogoPath = dm.LogoPath;
            ViewBag.LogoName = dm.LogoName;
            ViewBag.RefreshTime = dm.RollDispRefreshRate.ToString();
            ViewBag.ImagePath = dm.RollImagePath;
            ViewBag.emptyMessage = dm.message;

            ViewBag.pageSize = dm.pageSize;


            string initApp = "Web";
            if (Session["InitApp"] != null)
            {
                initApp = Session["InitApp"].ToString();
            }


            if (initApp == "App")
            {
                ViewBag.Columns = 0;
            }
            else
            {
                ViewBag.Columns = dm.DispColumns-1; 
            }


            var tblRolls = db.tblRolls.Include(t => t.ltRollDescription).Include(t => t.ltRollDescription1).Include(t => t.ltSpecy).Include(t => t.tblCustomer).Where(t => t.RollId == rollID);
            //            return View(tblRolls.ToList());

            int records = tblRolls.Count();
            ViewBag.records = records;
            

            if (TempData["lastRec"] == null)
            {
                ViewBag.lastRecordDisp = 0;
            }
            else
            { 
                ViewBag.lastRecordDisp = Convert.ToInt32(TempData["lastRec"]);
            }
//            ViewBag.balanceToDispl;

            return View(tblRolls.ToList());

        }
    }
}