using Auctions.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Auctions.Controllers
{
    public class TestCheckBoxController : Controller
    {
        // GET: TestCheckBox
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(RegisterViewModel viewModel)
        {
            return View();
        }
    }
}