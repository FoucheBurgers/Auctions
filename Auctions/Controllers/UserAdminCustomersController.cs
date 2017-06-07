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
    public class UserAdminCustomersController : Controller
    {
        private AuctionDBEntities db = new AuctionDBEntities();

        // GET: UserAdminCustomers
        public ActionResult Index()
        {
            return View(db.tblCustomers.OrderBy(t => t.CompanyName).ToList());
        }

        // GET: UserAdminCustomers/Details/5
        public ActionResult Details(int? id)
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

        // GET: UserAdminCustomers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: UserAdminCustomers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,CustomerID,Name,Surname,CompanyName,ContactPerson,SA_ID,CompanyID,VATNr,TaxNr,PostalAddress1,PostalAddress2,PostalAddress3,PostalCity,PostalCode,ResAddress1,ResAddress2,ResAddress3,ResCity,ResPostalCode,eMail,CellPhone,Phone,Bank,BranchName,BranchCode,BankAccountNr,Commotion,Agent,Language,Active,LinkKey,VATRegistered,CustomerNumber,PIN,FICA,DateLoaded")] tblCustomer tblCustomer)
        {
            if (ModelState.IsValid)
            {
                db.tblCustomers.Add(tblCustomer);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tblCustomer);
        }

        // GET: UserAdminCustomers/Edit/5
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

        // POST: UserAdminCustomers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,CustomerID,Name,Surname,CompanyName,ContactPerson,SA_ID,CompanyID,VATNr,TaxNr,PostalAddress1,PostalAddress2,PostalAddress3,PostalCity,PostalCode,ResAddress1,ResAddress2,ResAddress3,ResCity,ResPostalCode,eMail,CellPhone,Phone,Bank,BranchName,BranchCode,BankAccountNr,Commotion,Agent,Language,Active,LinkKey,VATRegistered,CustomerNumber,PIN,FICA,DateLoaded")] tblCustomer tblCustomer)
        {
            if (ModelState.IsValid)
            {
                db.UserAdminCustomer(tblCustomer.ID, tblCustomer.Phone, tblCustomer.PIN, tblCustomer.FICA, tblCustomer.Active);
                return RedirectToAction("Index");
            }
            return View(tblCustomer);
        }

        // GET: UserAdminCustomers/Delete/5
        public ActionResult Delete(int? id)
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

        // POST: UserAdminCustomers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tblCustomer tblCustomer = db.tblCustomers.Find(id);
            db.tblCustomers.Remove(tblCustomer);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
