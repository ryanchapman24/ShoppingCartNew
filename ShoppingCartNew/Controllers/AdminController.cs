using ShoppingCartNew.Models;
using ShoppingCartNew.Models.Code_First;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShoppingCartNew.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Universal
    {
        // GET: Admin/Index
        public ActionResult Index()
        {
            var thisMonth = System.DateTime.Now;
            var lastMonth = System.DateTime.Now.AddMonths(-1);
            var monthsAgo2 = System.DateTime.Now.AddMonths(-2);
            var monthsAgo3 = System.DateTime.Now.AddMonths(-3);
            var monthsAgo4 = System.DateTime.Now.AddMonths(-4);
            var monthsAgo5 = System.DateTime.Now.AddMonths(-5);
            var monthsAgo6 = System.DateTime.Now.AddMonths(-6);

            var today = System.DateTime.Now;
            var yesterday = System.DateTime.Now.AddDays(-1);
            var daysAgo2 = System.DateTime.Now.AddDays(-2);
            var daysAgo3 = System.DateTime.Now.AddDays(-3);
            var daysAgo4 = System.DateTime.Now.AddDays(-4);
            var daysAgo5 = System.DateTime.Now.AddDays(-5);
            var daysAgo6 = System.DateTime.Now.AddDays(-6);

            var delItems = db.Items.Where(i => i.Deleted == true).OrderByDescending(i => i.Id);

            ViewBag.ThisMonth = thisMonth.ToString("MMMM");
            ViewBag.LastMonth = lastMonth.ToString("MMMM");
            ViewBag.MonthsAgo2 = monthsAgo2.ToString("MMMM");
            ViewBag.MonthsAgo3 = monthsAgo3.ToString("MMMM");
            ViewBag.MonthsAgo4 = monthsAgo4.ToString("MMMM");
            ViewBag.MonthsAgo5 = monthsAgo5.ToString("MMMM");
            ViewBag.MonthsAgo6 = monthsAgo6.ToString("MMMM");

            ViewBag.Today = today.ToString("dddd");
            ViewBag.Yesterday = yesterday.ToString("dddd");
            ViewBag.DaysAgo2 = daysAgo2.ToString("dddd");
            ViewBag.DaysAgo3 = daysAgo3.ToString("dddd");
            ViewBag.DaysAgo4 = daysAgo4.ToString("dddd");
            ViewBag.DaysAgo5 = daysAgo5.ToString("dddd");
            ViewBag.DaysAgo6 = daysAgo6.ToString("dddd");


            ViewBag.OrdersThisMonth = db.Orders.Where(o => o.OrderDate.Month == thisMonth.Month && o.OrderDate.Year == thisMonth.Year).Count();
            ViewBag.OrdersLastMonth = db.Orders.Where(o => o.OrderDate.Month == lastMonth.Month && o.OrderDate.Year == lastMonth.Year).Count();
            ViewBag.OrdersMonthsAgo2 = db.Orders.Where(o => o.OrderDate.Month == monthsAgo2.Month && o.OrderDate.Year == monthsAgo2.Year).Count();
            ViewBag.OrdersMonthsAgo3 = db.Orders.Where(o => o.OrderDate.Month == monthsAgo3.Month && o.OrderDate.Year == monthsAgo3.Year).Count();
            ViewBag.OrdersMonthsAgo4 = db.Orders.Where(o => o.OrderDate.Month == monthsAgo4.Month && o.OrderDate.Year == monthsAgo4.Year).Count();
            ViewBag.OrdersMonthsAgo5 = db.Orders.Where(o => o.OrderDate.Month == monthsAgo5.Month && o.OrderDate.Year == monthsAgo5.Year).Count();
            ViewBag.OrdersMonthsAgo6 = db.Orders.Where(o => o.OrderDate.Month == monthsAgo6.Month && o.OrderDate.Year == monthsAgo6.Year).Count();

            ViewBag.SalesThisMonth = new decimal(0.00);
            ViewBag.SalesLastMonth = new decimal(0.00);
            ViewBag.SalesMonthsAgo2 = new decimal(0.00);
            ViewBag.SalesMonthsAgo3 = new decimal(0.00);
            ViewBag.SalesMonthsAgo4 = new decimal(0.00);
            ViewBag.SalesMonthsAgo5 = new decimal(0.00);
            ViewBag.SalesMonthsAgo6 = new decimal(0.00);

            if (ViewBag.OrdersThisMonth > 0)
            {
                ViewBag.SalesThisMonth = Convert.ToDecimal(db.Orders.Where(o => o.OrderDate.Month == thisMonth.Month && o.OrderDate.Year == thisMonth.Year).Sum(o => o.FinalTotal));
            }
            if (ViewBag.OrdersLastMonth > 0)
            {
                ViewBag.SalesLastMonth = Convert.ToDecimal(db.Orders.Where(o => o.OrderDate.Month == lastMonth.Month && o.OrderDate.Year == lastMonth.Year).Sum(o => o.FinalTotal));
            }
            if (ViewBag.OrdersMonthsAgo2 > 0)
            {
                ViewBag.SalesMonthsAgo2 = Convert.ToDecimal(db.Orders.Where(o => o.OrderDate.Month == monthsAgo2.Month && o.OrderDate.Year == monthsAgo2.Year).Sum(o => o.FinalTotal));
            }
            if (ViewBag.OrdersMonthsAgo3 > 0)
            {
                ViewBag.SalesMonthsAgo3 = Convert.ToDecimal(db.Orders.Where(o => o.OrderDate.Month == monthsAgo3.Month && o.OrderDate.Year == monthsAgo3.Year).Sum(o => o.FinalTotal));
            }
            if (ViewBag.OrdersMonthsAgo4 > 0)
            {
                ViewBag.SalesMonthsAgo4 = Convert.ToDecimal(db.Orders.Where(o => o.OrderDate.Month == monthsAgo4.Month && o.OrderDate.Year == monthsAgo4.Year).Sum(o => o.FinalTotal));
            }
            if (ViewBag.OrdersMonthsAgo5 > 0)
            {
                ViewBag.SalesMonthsAgo5 = Convert.ToDecimal(db.Orders.Where(o => o.OrderDate.Month == monthsAgo5.Month && o.OrderDate.Year == monthsAgo5.Year).Sum(o => o.FinalTotal));
            }
            if (ViewBag.OrdersMonthsAgo6 > 0)
            {
                ViewBag.SalesMonthsAgo6 = Convert.ToDecimal(db.Orders.Where(o => o.OrderDate.Month == monthsAgo6.Month && o.OrderDate.Year == monthsAgo6.Year).Sum(o => o.FinalTotal));
            }

            ViewBag.ViewsToday = db.Views.Where(v => v.Created.Year == today.Year && v.Created.Month == today.Month && v.Created.Day == today.Day).Count();
            ViewBag.ViewsYesterday = db.Views.Where(v => v.Created.Year == yesterday.Year && v.Created.Month == yesterday.Month && v.Created.Day == yesterday.Day).Count();
            ViewBag.ViewsDaysAgo2 = db.Views.Where(v => v.Created.Year == daysAgo2.Year && v.Created.Month == daysAgo2.Month && v.Created.Day == daysAgo2.Day).Count();
            ViewBag.ViewsDaysAgo3 = db.Views.Where(v => v.Created.Year == daysAgo3.Year && v.Created.Month == daysAgo3.Month && v.Created.Day == daysAgo3.Day).Count();
            ViewBag.ViewsDaysAgo4 = db.Views.Where(v => v.Created.Year == daysAgo4.Year && v.Created.Month == daysAgo4.Month && v.Created.Day == daysAgo4.Day).Count();
            ViewBag.ViewsDaysAgo5 = db.Views.Where(v => v.Created.Year == daysAgo5.Year && v.Created.Month == daysAgo5.Month && v.Created.Day == daysAgo5.Day).Count();
            ViewBag.ViewsDaysAgo6 = db.Views.Where(v => v.Created.Year == daysAgo6.Year && v.Created.Month == daysAgo6.Month && v.Created.Day == daysAgo6.Day).Count();


            ViewBag.ItemCount = delItems.Count();
            return View(delItems.ToList());
        }

        // POST: Admin/BringBackItem
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult BringBackItem(int id)
        {
            Item item = db.Items.Find(id);
            item.Deleted = false;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}