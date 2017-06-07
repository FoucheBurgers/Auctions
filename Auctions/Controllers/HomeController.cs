using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Auctions.Models;

namespace Auctions.Controllers
{
    public class HomeController : Controller
    {
        private AuctionDBEntities db = new AuctionDBEntities();

        public ActionResult Index(string initApp)
        {
            string temp;

            if (Session["InitApp"] == null && initApp == null)
            {
                Session["InitApp"] = "Web"; // creat InitApp as web init if nothing received 
            }
            else if (initApp != null) // maak hom wat ingekom het.  Dit is wanneer parametor gestuur word. 
            {
                Session["InitApp"] = initApp;
            }
            temp = Session["InitApp"].ToString();

            var tlAuctions = db.ltRollDescriptions.OrderBy(t => t.SortPosition).ToList().Where(t => t.Active == true);
//            var tlAuctions = db.ltRollDescriptions.ToList();

//            return View();
            return View(tlAuctions);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
        [Authorize]
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}