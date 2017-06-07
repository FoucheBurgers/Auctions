using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Auctions.Models;
using System.Web.Helpers;
using System.Net.Mime;
using System.Net.Mail;

namespace Auctions
{
    public class sendEmail
    {
        public string SendEmailFB(string ToEmail, string EmailSubject, string EMailBody, string CC, string BCC)
        //                    public string SendEmail(SendEmail obj)
        {
            SendEmail obj = new SendEmail();
            obj.ToEmail = ToEmail;
            obj.EmailSubject = EmailSubject;
            obj.EMailBody = EMailBody;
            obj.EmailCC = CC;
            obj.EmailBCC = BCC;
            //            string AttachedFile;
            //            var attachfile = { "~imigae/doc" };

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

                //Send email  Die een wek.
                WebMail.Send(to: obj.ToEmail, subject: obj.EmailSubject, body: obj.EMailBody, cc: obj.EmailCC, bcc: obj.EmailBCC, isBodyHtml: false);

//                var AttachedFile = "~/Documents/TCS.pdf";
//                var attachfile = new string[] { AttachedFile };

//                WebMail.Send(to: obj.ToEmail, subject: obj.EmailSubject, body: obj.EMailBody, filesToAttach: attachfile );
                //                WebMail.Send(to: obj.ToEmail, subject: obj.EmailSubject, body: obj.EMailBody);

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