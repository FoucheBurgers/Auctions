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
    [Authorize(Roles = "Administrator")]
    public class FlexiBuyerNoesController : Controller
    {
        private AuctionDBEntities db = new AuctionDBEntities();
        // GET: FlexiBuyerNoes
        public ActionResult Index()
        {
            return View(db.ltRollDescriptions.ToList());
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Index(RollAdminModel bid)
        //{
        //    int lot = bid.AuctionID;
        //    return View();
        //}
        public ActionResult Load(int? AuctionID)
        {
            using (AuctionDBEntities db = new AuctionDBEntities()) // make sure to get the latest info from DB and not the immage of DB.  
            {
                foreach (var flex in db.FlexiBuyerNoes.Where(x => x.EMAIL != "" && x.EMAIL != null || (x.CEL != "" && x.CEL != null)).ToList())
                {
                    tblCustomer customer = new tblCustomer();
                    tblCustomer customerFind = new tblCustomer();
                    customerFind = db.tblCustomers.FirstOrDefault(i => i.eMail == flex.EMAIL && i.eMail != "" && i.eMail != null);
                    if (customerFind == null) // Record met email bestaan nie. kyk of cell phone record bestaan. 
                    {
                        customerFind = db.tblCustomers.FirstOrDefault(i => i.Phone == flex.CEL);
                    }

                    if (customerFind != null) // Update details  
                    {
                        string a = flex.BSNR;
                    }
                    else // Add detail 
                    {
                        customer.CustomerID = flex.EMAIL;
                        customer.eMail = flex.EMAIL;
                        customer.CompanyName = flex.CUST_NAME;
                        customer.ContactPerson = flex.QFIND;
                        customer.Phone = flex.CEL;
                        customer.CellPhone = flex.TEL;
                        customer.VATNr = flex.VATNR;
                        if (flex.VATNR != "" && flex.VATNR != null)
                        {
                            customer.VATRegistered = true;
                        }
                        else
                        {
                            customer.VATRegistered = false;
                        }
                        customer.Active = true;
                        db.AddCustomerFlex(customer.CustomerID, customer.CompanyName, customer.ContactPerson, customer.eMail, customer.CellPhone, customer.Phone, customer.VATNr, customer.VATRegistered, customer.Active);
                    }
                } // end of flex 
                foreach (var flexBN in db.FlexiBuyerNoes.Where(x => x.EMAIL != "" && x.EMAIL != null || (x.CEL != "" && x.CEL != null)).ToList())
                {

                    // Kry customer ID
                    tblCustomer customerFindBN = new tblCustomer();

                    customerFindBN = db.tblCustomers.FirstOrDefault(i => i.eMail == flexBN.EMAIL && i.eMail != "" && i.eMail != null);
                    if (customerFindBN == null) // Record met email bestaan nie. Kyk of cell phone record bestaan. 
                    {
                        customerFindBN = db.tblCustomers.FirstOrDefault(i => i.Phone == flexBN.CEL);
                    }

                    if (customerFindBN != null) // Update details  // Behoort almal te gekry het
                    {
                        BuyerNo buyerNoAfter = db.BuyerNoes.FirstOrDefault(i => i.RollID == AuctionID && i.BuyerNumber == flexBN.BSNR);
                        if (buyerNoAfter != null) // Lot bestaan reeds 
                        {
                            // Ignore
                        }
                        else
                        {
                            BuyerNo buyerNo = new BuyerNo();

                            buyerNo.BuyerNumber = flexBN.BSNR;
                            buyerNo.CustomerID = customerFindBN.ID;
                            buyerNo.RollID = AuctionID;

                            db.BuyerNoes.Add(buyerNo);
                            db.SaveChanges();
                        }
                    }
                } // end of flec bn
//                return RedirectToAction("Index");
            } // end of using 
            TempData["msg"] = "<script>alert('Load completed');</script>";

            return RedirectToAction("Index");
        } // end of load 
    }
}
