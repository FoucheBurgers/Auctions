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
    public class CustomersNumberController : Controller
    {
        private AuctionDBEntities db = new AuctionDBEntities();

        // GET: CustomersNumber
        public ActionResult Index(string searchBy, string search)
        {
            if (searchBy == "CompName")
            {
                return View(db.tblCustomers.Where(x => x.CompanyName.StartsWith(search) || search == null).ToList());
            }
            else if (searchBy == "CustomerNum")
            {
                return View(db.tblCustomers.Where(x => x.CustomerNumber.StartsWith(search) || search == null).ToList());
            }
            else if (searchBy == "eMial")
            {
                return View(db.tblCustomers.Where(x => x.eMail.StartsWith(search) || search == null).ToList());
            }
            else if (searchBy == "Phone")
            {
                return View(db.tblCustomers.Where(x => x.Phone.StartsWith(search) || search == null).ToList());
            }
            else if (searchBy == "Contact")
            {
                return View(db.tblCustomers.Where(x => x.ContactPerson.StartsWith(search) || search == null).ToList());
            }
            else
            {
                return View(db.tblCustomers.Where(x => x.CellPhone.StartsWith(search) || search == null).ToList());
            }
        }


        // GET: CustomersNumber/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CustomersNumber/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CompanyName,ContactPerson,eMail,CellPhone,Phone, CustomerNumber")] tblCustomer tblCustomer)
        {
            if (ModelState.IsValid)
            {
                db.AddCustomerNum(tblCustomer.CompanyName, tblCustomer.CustomerNumber, tblCustomer.ContactPerson, tblCustomer.eMail, tblCustomer.CellPhone, tblCustomer.Phone);
                return RedirectToAction("Index");
            }

            return View(tblCustomer);
        }

        // GET: CustomersNumber/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblCustomer tblCustomer = db.tblCustomers.Find(id);
            if (tblCustomer == null)
            {
                return HttpNotFound();
            }
            return View(tblCustomer);
        }

        // POST: CustomersNumber/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //        public ActionResult Edit([Bind(Include = "ID,CustomerID,Name,Surname,CompanyName,ContactPerson,SA_ID,CompanyID,VATNr,TaxNr,PostalAddress1,PostalAddress2,PostalAddress3,PostalCity,PostalCode,ResAddress1,ResAddress2,ResAddress3,ResCity,ResPostalCode,eMail,CellPhone,Phone,Bank,BranchName,BranchCode,BankAccountNr,Commotion,Agent,Language,Active,LinkKey,VATRegistered,CustomerNumber")] tblCustomer tblCustomer)

        public ActionResult Edit([Bind(Include = "ID, CompanyName,ContactPerson,eMail,CellPhone,Phone,CustomerNumber")] tblCustomer tblCustomer)
        {
            if (ModelState.IsValid)
            {
                db.CustomersNumber(tblCustomer.ID, tblCustomer.CustomerNumber, tblCustomer.ContactPerson, tblCustomer.eMail, tblCustomer.CellPhone, tblCustomer.Phone);

                return RedirectToAction("Index");
            }
            return View(tblCustomer);
        }
    }
}
