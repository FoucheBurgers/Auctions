using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Auctions.Models;

namespace Auctions.Controllers
{
    public class smsSendController : Controller
    {
        // GET: smsSend
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(smsSendModel SMSSendModel)
        {
            if (ModelState.IsValid)
            {
                smsMGT smg = new smsMGT();
                var res = smg.SendSingleSMS("1", SMSSendModel.CellNumber, SMSSendModel.Message);
                if (res == "success")
                {
                    ViewBag.ResultMessage = "SMS sent successfully !";
                }
                else
                {
                    ViewBag.ResultMessage = "SMS NOT sent successfully ! Number not valid";
                }
            }

            return View();
        }
    }
}