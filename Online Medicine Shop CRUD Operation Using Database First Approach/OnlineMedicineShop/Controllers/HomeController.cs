using OnlineMedicineShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineMedicineShop.Controllers
{
    public class HomeController : Controller
    {
        private readonly APDbContext db = new APDbContext();
        [AllowAnonymous]
        public ActionResult Index()
        {
            int numberOfVendors = db.Vendors.Count();
            ViewBag.NumberOfVendors = numberOfVendors;
            decimal InvoiceMin = db.Invoices.Min(x => x.PaymentTotal);
            ViewBag.InvoiceMin = InvoiceMin;
            decimal InvoiceMax = db.Invoices.Max(x => x.PaymentTotal);
            ViewBag.InvoiceMax = InvoiceMax;

            decimal InvoiceAvg = db.Invoices.Average(x => x.PaymentTotal);
            ViewBag.InvoiceAvg = InvoiceAvg;

            decimal InvoiceSum = db.Invoices.Sum(x => x.PaymentTotal);
            ViewBag.InvoiceSum = InvoiceSum;
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}