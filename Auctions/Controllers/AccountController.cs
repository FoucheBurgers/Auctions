using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Auctions.Models;
using System.Data.Entity;
using Auctions.Controllers;
using static Auctions.Controllers.ManageController;

namespace Auctions.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        private AuctionDBEntities db = new AuctionDBEntities(); // FB Auction DB

        public AccountController()
        {
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;

            // FB Added 
            string aid = "0";
            int? rollID;
            if (Session["auctionID"] == null)
            {
                rollID = 0;
            }
            else
            {
                aid = Session["auctionID"].ToString();
                rollID = Int32.Parse(aid);
            }


            // Find user record for role id.

            BidController bd = new BidController();
            DefaultSetupModel dm = bd.LoadDefs(rollID); // Get the default values 


            ViewBag.BackgroundColor = dm.BackgroundColor;
            ViewBag.TexColor = dm.FontColor;
            ViewBag.LogoBackgroundColor = dm.LogoBackgroundColor;
            ViewBag.LogoPath = dm.LogoPath;
            ViewBag.LogoName = dm.LogoName;

            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // FB Added 
            string aid = "0";
            int? rollID;
            if (Session["auctionID"] == null)
            {
                rollID = 0;
            }
            else
            {
                aid = Session["auctionID"].ToString();
                rollID = Int32.Parse(aid);
            }


            // Find user record for role id.

            BidController bd = new BidController();
            DefaultSetupModel dm = bd.LoadDefs(rollID); // Get the default values 

            ViewBag.BackgroundColor = dm.BackgroundColor;
            ViewBag.TexColor = dm.FontColor;
            ViewBag.LogoBackgroundColor = dm.LogoBackgroundColor;
            ViewBag.LogoPath = dm.LogoPath;
            ViewBag.LogoName = dm.LogoName;

            // Require the user to have a confirmed email before they can log on.
            var user = await UserManager.FindByNameAsync(model.Email);
            if (user != null)
            {
                if (!await UserManager.IsEmailConfirmedAsync(user.Id) && dm.emailConfirmationLoginRequired == true)
                {
                    string callbackUrl = await SendEmailConfirmationTokenAsync(user.Id, "Confirm your account-Resend");
                    ViewBag.errorMessage = "You must have confirmed your email to log on.";
                    return View("Error");
                }

                if (!await UserManager.IsPhoneNumberConfirmedAsync(user.Id) && dm.SMSConfirmationLoginRequired == true)
                {
                    // Stuur weer boodskap. 

                    ViewBag.errorMessage = "You must confirmed your details before login. Re-send Code";
                    ViewBag.email = model.Email;
                    return View("ResentCode");
                }
                // Fouche : Adde Active check
                tblCustomer tblCustomerEx = db.tblCustomers.FirstOrDefault(i => i.CustomerID == model.Email);

                if (tblCustomerEx != null) // Customer bestaan
                {
                    if (!tblCustomerEx.Active) // Customer nie aangelog nie. 
                    {
                        ViewBag.errorMessage = "Log in not authorised. Please contact NWWT at support@nwwt.co.za";

                        // Stuur email vir NWWT
                        // email warning to NWWT 
                        sendEmail sm = new sendEmail();
                        string EmailSubject = "An attempt to log in was made by a suspended customer";
                        string EMailBody = $"An attempt to log in was made by a suspended customer. Customer {tblCustomerEx.CompanyName}.";
                        string res = sm.SendEmailFB("support@nwwt.co.za", EmailSubject, EMailBody, null, null);


                        return View("Error");
                    }

                }


            }


            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true

            var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);

                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return View(model);
            }
        }




        //
        // GET: /Account/VerifyCode
        [AllowAnonymous]
        public async Task<ActionResult> VerifyCode(string provider, string returnUrl, bool rememberMe)
        {
            // Require that the user has already logged in via username/password or external login
            if (!await SignInManager.HasBeenVerifiedAsync())
            {
                return View("Error");
            }
            return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/VerifyCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // The following code protects for brute force attacks against the two factor codes. 
            // If a user enters incorrect codes for a specified amount of time then the user account 
            // will be locked out for a specified amount of time. 
            // You can configure the account lockout settings in IdentityConfig
            var result = await SignInManager.TwoFactorSignInAsync(model.Provider, model.Code, isPersistent: model.RememberMe, rememberBrowser: model.RememberBrowser);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(model.ReturnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid code.");
                    return View(model);
            }
        }

        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            // FB Added 
            string aid = "0";
            int? rollID;
            if (Session["auctionID"] == null)
            {
                rollID = 0;
            }
            else
            {
                aid = Session["auctionID"].ToString();
                rollID = Int32.Parse(aid);
            }


            // Find Auction record for role roll id.

            BidController bd = new BidController();
            DefaultSetupModel dm = bd.LoadDefs(rollID); // Get the default values 

            ViewBag.BackgroundColor = dm.BackgroundColor;
            ViewBag.TexColor = dm.FontColor;
            ViewBag.LogoBackgroundColor = dm.LogoBackgroundColor;
            ViewBag.LogoPath = dm.LogoPath;
            ViewBag.LogoName = dm.LogoName;

            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {

            // FB Added 
            string aid = "0";
            int? rollID;
            if (Session["auctionID"] == null)
            {
                rollID = 0;
            }
            else
            {
                aid = Session["auctionID"].ToString();
                rollID = Int32.Parse(aid);
            }

            BidController bd = new BidController();
            DefaultSetupModel dm = bd.LoadDefs(rollID); // Get the default values 

            ViewBag.BackgroundColor = dm.BackgroundColor;
            ViewBag.TexColor = dm.FontColor;
            ViewBag.LogoBackgroundColor = dm.LogoBackgroundColor;
            ViewBag.LogoPath = dm.LogoPath;
            ViewBag.LogoName = dm.LogoName;

            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { CompanyName = model.CompanyName, PhoneNumber = model.PhoneNumber, UserName = model.Email, Email = model.Email, CustomerID = model.CustomerID, ContactPerson = model.ContactPerson, ContactCellPhone = model.ContactCellPhone, PIN = model.PIN };
                var result = await UserManager.CreateAsync(user, model.Password);
                // User is created in Accounts table

                if (result.Succeeded)
                {
                    UserManager.AddClaim(user.Id, new Claim(ClaimTypes.GivenName, model.CompanyName));

                    // FB added this
                    // User is successfully created in Accounts table. 
                    // Create Customer record with Basic info al is die inligting nie bevestig deur email of sms
                    //

                    tblCustomer tblCustomers = new tblCustomer();
                    tblCustomer tblCustomerEx = db.tblCustomers.FirstOrDefault(i => i.CustomerID == model.Email);
                    string companyName;
                    string Email;

                    if (tblCustomerEx == null)  // voeg by
                    {
                        tblCustomers.CustomerID = model.Email; // Kan dalk later verander.  
                        tblCustomers.CompanyName = model.CompanyName;
                        tblCustomers.CompanyID = model.CustomerID;
                        tblCustomers.eMail = model.Email;
                        tblCustomers.Phone = model.PhoneNumber;
                        tblCustomers.ContactPerson = model.ContactPerson;
                        tblCustomers.CellPhone = model.ContactCellPhone;
                        tblCustomers.Active = true;
                        tblCustomers.VATRegistered = false;
                        tblCustomers.PIN = model.PIN;
                        companyName = model.CompanyName;
                        Email = model.Email;

                        db.tblCustomers.Add(tblCustomers);
                        db.SaveChanges();
                    }
                    else
                    {
                        tblCustomer tblCustomersF = db.tblCustomers.FirstOrDefault(i => i.CustomerID == model.Email);
                        db.Entry(tblCustomersF).State = EntityState.Modified;
                        tblCustomersF.CompanyName = model.CompanyName;
                        tblCustomersF.CompanyID = model.CustomerID;
                        tblCustomersF.eMail = model.Email;
                        tblCustomersF.Phone = model.PhoneNumber;
                        tblCustomersF.ContactPerson = model.ContactPerson;
                        tblCustomersF.CellPhone = model.ContactCellPhone;
                        tblCustomersF.Active = true;
                        tblCustomersF.VATRegistered = false;
                        tblCustomersF.PIN = model.PIN;
                        companyName = model.CompanyName;
                        Email = model.Email;

                        db.SaveChanges();
                    }

                    BidController loadDefs = new BidController();
                    DefaultSetupModel dms = loadDefs.LoadDefs(0); // Get the default values 
                    bool confirmation = false;
                    // Stuur confirmation email as nodig
                    if (dms.emailConfirmRegistration == true)
                    {
                        string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                        string callbackUrl = await SendEmailConfirmationTokenAsync(user.Id, "Confirm your account");
                        ViewBag.Message = "Check your email and confirm your account, you must be confirmed "
                        + "before you can log in.";
                        await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");
                        confirmation = true;

                    }
                    if (dms.SMSAllFunctionality == true && dms.SMSCustRegistration == true)
                    {
                        // Generate OTP
                        var smscode = await UserManager.GenerateChangePhoneNumberTokenAsync(user.Id, model.PhoneNumber);
                        // FB added
                        smsMGT smg = new smsMGT();
                        string messagestring = "Your security code for Silent Auction is: " + smscode;
                        var res = smg.SendSingleSMS("1", model.PhoneNumber, messagestring);

                        if (res != "success")
                        {
                            string EmailSubject = "OTP SMS NOT sent successfully";
                            string EMailBody = $"OTP SMS NOT sent successfully to new customer {model.CompanyName}" + ".  Error = " + res + " Number " + model.PhoneNumber;
                            sendEmail sm = new sendEmail();
                            string emalres = sm.SendEmailFB("support@nwwt.co.za", EmailSubject, EMailBody, null, null);
                            ViewBag.errorMessage = "Cell Phone number not correct. Log in and provide correct number or contact NWWT at support@nwwt.co.za";
                            return View("Error");
                            /// FB
                        }
                        confirmation = true;
                        return RedirectToAction("VerifyPhoneNumber", new { PhoneNumber = model.PhoneNumber, id = user.Id, email = Email, compName = companyName });
                    }
                    if (!confirmation)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                        return RedirectToAction("Index", "Home");
                    }
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        [AllowAnonymous]
        public async Task<ActionResult> VerifyPhoneNumber(string phoneNumber, string id, string EMAIL, string compName)
        {
            var code = await UserManager.GenerateChangePhoneNumberTokenAsync(id, phoneNumber);

            return phoneNumber == null ? View("Error") : View(new VerifyPhoneNumberViewModel { PhoneNumber = phoneNumber, userID = id, email = EMAIL, CompanyName = compName });
        }

        //
        // POST: /Manage/VerifyPhoneNumber
        [HttpPost]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyPhoneNumber(VerifyPhoneNumberViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = await UserManager.ChangePhoneNumberAsync(model.userID, model.PhoneNumber, model.Code);
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(model.userID);
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                sendEmail sm = new sendEmail();
                string EmailSubject = "FICA requirements for NWWT Silent Auctions";
                string EMailBody = $"Thank you {model.CompanyName} for registering for NWWT Silent Auctions.\r\n\r\n Please email a copy of the responsible person’s ID, Company registration certificate (if applicable), proof of address and VAT certificate (if applicable), within 7 days to avoid deregistration, to: accounts@nwwt.co.za \r\n \r\n Thank you for your support. \r\n\r\n Kind regards \r\n\r\n \r\n\r\n \r\n\r\n NWWT Silent Auctions Team";
                string res = sm.SendEmailFB(model.email, EmailSubject, EMailBody, "support@nwwt.co.za", null);

                return RedirectToAction("Index", "Home", new { Message = "Cell Phone successfully verified" });
            }
            // If we got this far, something failed, redisplay form
            ModelState.AddModelError("", "Failed to verify phone");
            return View(model);
        }
        // Fouche Verify Passsword change from SMS OTP
        //
        [AllowAnonymous]
        public async Task<ActionResult> VerifyPhoneNumberPasswordChange(string phoneNumber, string id)
        {
            var code = await UserManager.GenerateChangePhoneNumberTokenAsync(id, phoneNumber);

            return phoneNumber == null ? View("Error") : View(new VerifyPhoneNumberViewModel { PhoneNumber = phoneNumber, userID = id });
        }

        //
        // POST: /Manage/VerifyPhoneNumber
        [HttpPost]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyPhoneNumberPasswordChange(VerifyPhoneNumberViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = await UserManager.ChangePhoneNumberAsync(model.userID, model.PhoneNumber, model.Code);
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(model.userID);
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                // Fouche  

                return RedirectToAction("ResetPasswordSMS", "Account");
            }
            // If we got this far, something failed, redisplay form
            ModelState.AddModelError("", "Failed to verify phone");
            return View(model);
        }
        // 

        //  FB
        //


        // FB Admin Conirme Cell 

        [AllowAnonymous]
        //        public async Task<ActionResult> AdminConfirmCell(string phoneNumber, string id)
        public ActionResult AdminConfirmCell(string phoneNumber, string id)
        {

            ViewBag.ResultMessage = "";

            return View();
        }

        //
        // POST: /Manage/VerifyPhoneNumber
        [HttpPost]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> AdminConfirmCell(AdminConfirmCell model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = UserManager.FindByName(model.email);
            if(user == null)
            {

                ViewBag.ResultMessage = "User with Email address not found!";
                return View(model);
            }
            model.userID = user.Id;
            model.Code = await UserManager.GenerateChangePhoneNumberTokenAsync(model.userID, model.PhoneNumber);

            var result = await UserManager.ChangePhoneNumberAsync(model.userID, model.PhoneNumber, model.Code);


            if (result.Succeeded)
            {

                ViewBag.ResultMessage = "Cell number successfully confirmed!";

                return View(model);
            }
            // If we got this far, something failed, redisplay form
            ModelState.AddModelError("", "Failed to verify phone");
            return View(model);
        }


        // FB end Admin confirm cell


        // GET: /Manage/AddPhoneNumber
        [AllowAnonymous]
        public ActionResult AddPhoneNumber(string email)
        {
            ResentCodeModel resentCodeModel = new ResentCodeModel();
            resentCodeModel.email = email;
            return View(resentCodeModel);
        }

        //
        // POST: /Manage/AddPhoneNumber
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<ActionResult> AddPhoneNumber(ResentCodeModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await UserManager.FindByNameAsync(model.email);
            if (user != null)
            {
                model.userID = user.Id;
            }
            // Generate the token and send it

            //              var code = await UserManager.GenerateChangePhoneNumberTokenAsync(model.userID, model.PhoneNumber);
            bool smsNotifications = true;
            if (smsNotifications)
            {
                var smscode = await UserManager.GenerateChangePhoneNumberTokenAsync(user.Id, model.PhoneNumber);
                // FB added
                smsMGT smg = new smsMGT();
                string messagestring = "Your security code for Silent Auction is: " + smscode;
                var res = smg.SendSingleSMS("1", model.PhoneNumber, messagestring);
                if (res != "success")
                {
                    string EmailSubject = "OTP SMS NOT sent successfully";
                    string EMailBody = $"OTP SMS NOT sent successfully to customer {user.CompanyName}" + " to change phone " + ".  Error = " + res;
                    sendEmail sm = new sendEmail();
                    string emalres = sm.SendEmailFB("support@nwwt.co.za", EmailSubject, EMailBody, null, null);
                    ViewBag.errorMessage = "Cell Phone number not correct. Log in and provide correct number or contact NWWT at support@nwwt.co.za";
                    return View("Error");
                }

            }

            return RedirectToAction("VerifyPhoneNumber", new { PhoneNumber = model.PhoneNumber, id = user.Id });
        }


        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public ActionResult ConfirmOTP()
        {
            return View();

        }

        //
        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var result = await UserManager.ConfirmEmailAsync(userId, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        //
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            BidController loadDefs = new BidController();
            DefaultSetupModel dms = loadDefs.LoadDefs(0); // Get the default values 
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByNameAsync(model.Email);
                if (user == null)
                {
                    ViewBag.errorMessage = "Email address provided is not registered. Please use correct email address or register as user";
                    return View("Error");
                }

                if (dms.emailConfirmationLoginRequired == true)
                {
                    if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
                    {
                        // Don't reveal that the user does not exist or is not confirmed
                        // return View("ForgotPasswordConfirmation"); // Moet error gee. 
                        ViewBag.errorMessage = "Email address not confirmed";
                        return View("Error");
                    }
                }
                if (dms.SMSConfirmationLoginRequired == true)
                {
                    if (user == null || !(await UserManager.IsPhoneNumberConfirmedAsync(user.Id)))
                    {
                        // Don't reveal that the user does not exist or is not confirmed
                        //                        return View("ForgotPasswordConfirmation"); // Moet error gee. 
                        ViewBag.errorMessage = "Cell Phone number not confirmed";
                        return View("Error");

                    }
                }

                // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                // Send an email with this link

                //string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                //var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);

                // Fouche
                bool confirmation = false;
                // Stuur confirmation email as nodig
                if (dms.emailConfirmRegistration == true)
                {
                    string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                    //                    string callbackUrl = await SendEmailConfirmationTokenAsync(user.Id, "Confirm your account");
                    var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    ViewBag.Message = "Check your email and confirm your account, you must be confirmed "
                    + "before you can log in.";
                    confirmation = true;
                    await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");
                }
                if (dms.SMSAllFunctionality == true && dms.SMSCustRegistration == true)
                {
                    // Generate OTP
                    var smscode = await UserManager.GenerateChangePhoneNumberTokenAsync(user.Id, user.PhoneNumber);
                    // FB added
                    smsMGT smg = new smsMGT();
                    string messagestring = "Your security code for Silent Auction is: " + smscode;
                    var res = smg.SendSingleSMS("1", user.PhoneNumber, messagestring);
                    if (res != "success")
                    {
                        string EmailSubject = "OTP SMS NOT sent successfully";
                        string EMailBody = $"OTP SMS NOT sent successfully to customer {user.CompanyName}" + " Forgot Password " + ".  Error = " + res;
                        sendEmail sm = new sendEmail();
                        string emalres = sm.SendEmailFB("support@nwwt.co.za", EmailSubject, EMailBody, null, null);
                        ViewBag.errorMessage = "Cell Phone number not correct. Log in and provide correct number or contact NWWT at support@nwwt.co.za";
                        return View("Error");

                    }


                    //
                    confirmation = true;
                    return RedirectToAction("VerifyPhoneNumberPasswordChange", new { PhoneNumber = user.PhoneNumber, id = user.Id });
                }
                if (!confirmation)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                    //                    return RedirectToAction("Index", "Home");
                    return RedirectToAction("ForgotPasswordConfirmation", "Account");
                }
                return RedirectToAction("ForgotPasswordConfirmation", "Account");
            }
            // If we got this far, something failed, redisplay form
            return View(model);

        }

        //
        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        //
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            return code == null ? View("Error") : View();
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await UserManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                ViewBag.errorMessage = "Email address provided is not registered on system";
                return View("Error");

                //                return RedirectToAction("ResetPasswordConfirmation", "Account");

            }
            var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            AddErrors(result);
            return View();
        }

        // Fouche - rsete password from SMS
        //
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPasswordSMS()
        {
            return View();
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPasswordSMS(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await UserManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                //               return RedirectToAction("ResetPasswordConfirmation", "Account"); // Error view
                ViewBag.errorMessage = "Email address provided is not registered on system";
                return View("Error");

            }
            // Fouche code vir reset. 

            //            return RedirectToAction("ResetPasswordConfirmation", "Account");
            return RedirectToAction("Index", "Home");
        }




        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/SendCode
        [AllowAnonymous]
        public async Task<ActionResult> SendCode(string returnUrl, bool rememberMe)
        {
            var userId = await SignInManager.GetVerifiedUserIdAsync();
            if (userId == null)
            {
                return View("Error");
            }
            var userFactors = await UserManager.GetValidTwoFactorProvidersAsync(userId);
            var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
            return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/SendCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendCode(SendCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            // Generate the token and send it
            if (!await SignInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
            {
                return View("Error");
            }
            return RedirectToAction("VerifyCode", new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });
        }

        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // Sign in the user with this external login provider if the user already has a login
            var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
                case SignInStatus.Failure:
                default:
                    // If the user does not have an account, then prompt the user to create an account
                    ViewBag.ReturnUrl = returnUrl;
                    ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                    return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
            }
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Manage");
            }

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        private async Task<string> SendEmailConfirmationTokenAsync(string userID, string subject)
        {
            string code = await UserManager.GenerateEmailConfirmationTokenAsync(userID);
            var callbackUrl = Url.Action("ConfirmEmail", "Account",
               new { userId = userID, code = code }, protocol: Request.Url.Scheme);
            await UserManager.SendEmailAsync(userID, subject,
               "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

            return callbackUrl;
        }
        #endregion
    }

}
