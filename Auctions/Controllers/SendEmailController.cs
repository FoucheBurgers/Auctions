using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using Auctions.Models;

namespace Auctions.Controllers
{
    public class SendEmailController : Controller
    {
        // GET: SendEmail
        public ActionResult Index()
        {
            sendEmail sm = new sendEmail();
            string EmailSubject = "FICA requirements for NWWT Silent Auctions";
            string EMailBody = "Thank you for registering for NWWT Silent Auctions.\r\n\r\n Please email a copy of the responsible person’s ID, Company registration certificate (if applicable), proof of address and VAT certificate (if applicable), within 7 days to avoid deregistration, to: accounts@nwwt.co.za \r\n \r\n Thank you for your support. \r\n\r\n Kind regards \r\n\r\n \r\n\r\n \r\n\r\n NWWT Silent Auctions Team";
            string res = sm.SendEmailFB("fouche@burgers.cc", EmailSubject, EMailBody, null, "fouche.burgers@lantic.net");

            return View();
        }

        //        public ActionResult SendEmail(SendEmail obj)
        public string SendEmailFB(string ToEmail, string EmailSubject, string EMailBody, string CC, string BCC)
        //                    public string SendEmail(SendEmail obj)
        {
            SendEmail obj = new SendEmail();
            obj.ToEmail = ToEmail;
            obj.EmailSubject = EmailSubject;
            obj.EMailBody = EMailBody;
            obj.EmailCC = CC;
            obj.EmailBCC = BCC;

            string result = "message";
            try
            {
                //Configuring webMail class to send emails  
                //gmail smtp server  
                WebMail.SmtpServer = "smtp.gmail.com";
                //gmail port to send emails  
                WebMail.SmtpPort = 587;
                WebMail.SmtpUseDefaultCredentials = true;
                //sending emails with secure protocol  
                WebMail.EnableSsl = true;
                //EmailId used to send emails from application  
                WebMail.UserName = "fouched.burgers@gmail.com";
                WebMail.Password = "fouche2012";

                //Sender email address.  
                WebMail.From = "fouched.burgers@gmail.com";

                //Send email  
                WebMail.Send(to: obj.ToEmail, subject: obj.EmailSubject, body: obj.EMailBody, cc: obj.EmailCC, bcc: obj.EmailBCC, isBodyHtml: true);
                result = "Email Sent Successfully.";
            }
            catch (Exception)
            {
                result = "Problem while sending email, Please check details.";

            }
            return result;
        }
    }
}