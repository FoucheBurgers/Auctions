using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Auctions.Controllers
{
    public class smsStatusController : Controller
    {
        // GET: smsStatus
        public ActionResult Index()
        {
            smsMGT smg = new smsMGT();
            string strSource = smg.SMSStatus();

            int Start, End;
            string strStart = "<credits>";
            string strEnd = "</credits>";

            if (strSource.Contains(strStart) && strSource.Contains(strEnd))
            {
                Start = strSource.IndexOf(strStart, 0) + strStart.Length;
                End = strSource.IndexOf(strEnd, Start);
                ViewBag.res = strSource.Substring(Start, End - Start);
            }
            else
            {
                ViewBag.res = "Unknown";
            }
            return View();
          
        }
    }
}