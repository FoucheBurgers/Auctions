using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Auctions.Models;
using System.Xml.Serialization;
using System.IO;
using Microsoft.AspNet.Identity;
using System.Threading.Tasks;
using System.Data.Entity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;


namespace Auctions.Controllers

{
    public class TestController : Controller
    {
        // GET: Test
        public ActionResult Index()
        {


            return View();
        }

        public ActionResult TempIndex()
        {

            return View();
        }

        public async Task<ActionResult> ConfirmCell (RegisterViewModel model)
        {

            // Generate code and sms

            //var smscode = await Microsoft.AspNet.Identity.UserManager.GenerateChangePhoneNumberTokenAsync(user.Id, model.PhoneNumber);

            //// generate code to confirm if the same

            //var code = await UserManager.GenerateChangePhoneNumberTokenAsync(id, phoneNumber);

            //// stuur oor en verander cell.

            //var result = await UserManager.ChangePhoneNumberAsync(model.userID, model.PhoneNumber, model.Code);


            return null;

        }
    }
}