using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.EntityFramework; //add
using Auctions.Models;
using Microsoft.AspNet.Identity;

namespace Auctions.Controllers
{
    public class UserAdminUserController : Controller
    {
        // GET: UserAdminUser
        private ApplicationDbContext context = new ApplicationDbContext();

        public ActionResult Index()
        {
            return View(context.Users.ToList());
        }

        // GET: /Roles/Edit/5
        public ActionResult Edit(string UserName)
        {
            ApplicationUser user = context.Users.Where(u => u.UserName.Equals(UserName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();

            return View(user);
        }

        //
        // POST: /Roles/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Microsoft.AspNet.Identity.EntityFramework.IdentityUser role)
        {
            try
            {
                context.Entry(role).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();

                //var store = new UserStore<ApplicationUser>(new ApplicationDbContext());
                //var manager = new UserManager(store);
                //// then after updating the user by calling
                //manager.UpdateAsync(user);
                //// then you go to the context

                //var ctx = store.Context;
                //// then
                //ctx.saveChanges();


                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

    }
}