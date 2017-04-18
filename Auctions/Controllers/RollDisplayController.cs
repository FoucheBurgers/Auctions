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
    public class RollDisplayController : Controller
    {
        private AuctionDBEntities db = new AuctionDBEntities();

        // GET: RollDisplay
        public ActionResult Index()
        {
            var tblRolls = db.tblRolls.Include(t => t.ltRollDescription).Include(t => t.ltRollDescription1).Include(t => t.ltSpecy).Include(t => t.tblCustomer);
 //           int count = tblRolls.Count();


            return View(tblRolls.ToList());
        }

    }
}
