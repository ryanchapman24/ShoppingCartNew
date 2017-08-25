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
            ViewBag.ThisMonth = thisMonth.ToString("MMMM");
            ViewBag.LastMonth = lastMonth.ToString("MMMM");
            ViewBag.MonthsAgo2 = monthsAgo2.ToString("MMMM");
            ViewBag.MonthsAgo3 = monthsAgo3.ToString("MMMM");
            ViewBag.MonthsAgo4 = monthsAgo4.ToString("MMMM");
            ViewBag.MonthsAgo5 = monthsAgo5.ToString("MMMM");
            ViewBag.MonthsAgo6 = monthsAgo6.ToString("MMMM");

            var today = System.DateTime.Now;
            var yesterday = System.DateTime.Now.AddDays(-1);
            var daysAgo2 = System.DateTime.Now.AddDays(-2);
            var daysAgo3 = System.DateTime.Now.AddDays(-3);
            var daysAgo4 = System.DateTime.Now.AddDays(-4);
            var daysAgo5 = System.DateTime.Now.AddDays(-5);
            var daysAgo6 = System.DateTime.Now.AddDays(-6);
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

            ViewBag.PurchasesToday = 0;
            ViewBag.PurchasesYesterday = 0;
            ViewBag.PurchasesDaysAgo2 = 0;
            ViewBag.PurchasesDaysAgo3 = 0;
            ViewBag.PurchasesDaysAgo4 = 0;
            ViewBag.PurchasesDaysAgo5 = 0;
            ViewBag.PurchasesDaysAgo6 = 0;

            if (db.OrderItems.Where(i => i.Order.OrderDate.Year == today.Year && i.Order.OrderDate.Month == today.Month && i.Order.OrderDate.Day == today.Day).Count() > 0)
            {
                ViewBag.PurchasesToday = db.OrderItems.Where(i => i.Order.OrderDate.Year == today.Year && i.Order.OrderDate.Month == today.Month && i.Order.OrderDate.Day == today.Day).Sum(i => i.Quantity);
            }
            if (db.OrderItems.Where(i => i.Order.OrderDate.Year == yesterday.Year && i.Order.OrderDate.Month == yesterday.Month && i.Order.OrderDate.Day == yesterday.Day).Count() > 0)
            {
                ViewBag.PurchasesYesterday = db.OrderItems.Where(i => i.Order.OrderDate.Year == yesterday.Year && i.Order.OrderDate.Month == yesterday.Month && i.Order.OrderDate.Day == yesterday.Day).Sum(i => i.Quantity);
            }
            if (db.OrderItems.Where(i => i.Order.OrderDate.Year == daysAgo2.Year && i.Order.OrderDate.Month == daysAgo2.Month && i.Order.OrderDate.Day == daysAgo2.Day).Count() > 0)
            {
                ViewBag.PurchasesDaysAgo2 = db.OrderItems.Where(i => i.Order.OrderDate.Year == daysAgo2.Year && i.Order.OrderDate.Month == daysAgo2.Month && i.Order.OrderDate.Day == daysAgo2.Day).Sum(i => i.Quantity);
            }
            if (db.OrderItems.Where(i => i.Order.OrderDate.Year == daysAgo3.Year && i.Order.OrderDate.Month == daysAgo3.Month && i.Order.OrderDate.Day == daysAgo3.Day).Count() > 0)
            {
                ViewBag.PurchasesDaysAgo3 = db.OrderItems.Where(i => i.Order.OrderDate.Year == daysAgo3.Year && i.Order.OrderDate.Month == daysAgo3.Month && i.Order.OrderDate.Day == daysAgo3.Day).Sum(i => i.Quantity);
            }
            if (db.OrderItems.Where(i => i.Order.OrderDate.Year == daysAgo4.Year && i.Order.OrderDate.Month == daysAgo4.Month && i.Order.OrderDate.Day == daysAgo4.Day).Count() > 0)
            {
                ViewBag.PurchasesDaysAgo4 = db.OrderItems.Where(i => i.Order.OrderDate.Year == daysAgo4.Year && i.Order.OrderDate.Month == daysAgo4.Month && i.Order.OrderDate.Day == daysAgo4.Day).Sum(i => i.Quantity);
            }
            if (db.OrderItems.Where(i => i.Order.OrderDate.Year == daysAgo5.Year && i.Order.OrderDate.Month == daysAgo5.Month && i.Order.OrderDate.Day == daysAgo5.Day).Count() > 0)
            {
                ViewBag.PurchasesDaysAgo5 = db.OrderItems.Where(i => i.Order.OrderDate.Year == daysAgo5.Year && i.Order.OrderDate.Month == daysAgo5.Month && i.Order.OrderDate.Day == daysAgo5.Day).Sum(i => i.Quantity);
            }
            if (db.OrderItems.Where(i => i.Order.OrderDate.Year == daysAgo6.Year && i.Order.OrderDate.Month == daysAgo6.Month && i.Order.OrderDate.Day == daysAgo6.Day).Count() > 0)
            {
                ViewBag.PurchasesDaysAgo6 = db.OrderItems.Where(i => i.Order.OrderDate.Year == daysAgo6.Year && i.Order.OrderDate.Month == daysAgo6.Month && i.Order.OrderDate.Day == daysAgo6.Day).Sum(i => i.Quantity);
            }

            var viewList =  db.Views.GroupBy(v => v.ItemId).Select(group => new { ItemId = group.Key, Count = group.Count() }).OrderByDescending(v => v.Count).ToArray();
            ViewBag.ItemViewedName1 = db.Items.Find(viewList[0].ItemId).Name;
            ViewBag.ItemViewedName2 = db.Items.Find(viewList[1].ItemId).Name;
            ViewBag.ItemViewedName3 = db.Items.Find(viewList[2].ItemId).Name;
            ViewBag.ItemViewedName4 = db.Items.Find(viewList[3].ItemId).Name;
            ViewBag.ItemViewedName5 = db.Items.Find(viewList[4].ItemId).Name;
            ViewBag.ItemViewedName6 = db.Items.Find(viewList[5].ItemId).Name;
            ViewBag.ItemViewedName7 = db.Items.Find(viewList[6].ItemId).Name;
            ViewBag.ItemViewedName8 = db.Items.Find(viewList[7].ItemId).Name;
            ViewBag.ItemViewedName9 = db.Items.Find(viewList[8].ItemId).Name;
            ViewBag.ItemViewedName10 = db.Items.Find(viewList[9].ItemId).Name;

            ViewBag.ItemViewed1 = viewList[0].Count;
            ViewBag.ItemViewed2 = viewList[1].Count;
            ViewBag.ItemViewed3 = viewList[2].Count;
            ViewBag.ItemViewed4 = viewList[3].Count;
            ViewBag.ItemViewed5 = viewList[4].Count;
            ViewBag.ItemViewed6 = viewList[5].Count;
            ViewBag.ItemViewed7 = viewList[6].Count;
            ViewBag.ItemViewed8 = viewList[7].Count;
            ViewBag.ItemViewed9 = viewList[8].Count;
            ViewBag.ItemViewed10 = viewList[9].Count;

            var buyList = db.OrderItems.GroupBy(o => o.ItemId).Select(group => new { ItemId = group.Key, Quantity = group.Sum( g => g.Quantity) }).OrderByDescending(o => o.Quantity).ToArray();
            ViewBag.ItemBoughtName1 = db.Items.Find(buyList[0].ItemId).Name;
            ViewBag.ItemBoughtName2 = db.Items.Find(buyList[1].ItemId).Name;
            ViewBag.ItemBoughtName3 = db.Items.Find(buyList[2].ItemId).Name;
            ViewBag.ItemBoughtName4 = db.Items.Find(buyList[3].ItemId).Name;
            ViewBag.ItemBoughtName5 = db.Items.Find(buyList[4].ItemId).Name;
            ViewBag.ItemBoughtName6 = db.Items.Find(buyList[5].ItemId).Name;
            ViewBag.ItemBoughtName7 = db.Items.Find(buyList[6].ItemId).Name;
            ViewBag.ItemBoughtName8 = db.Items.Find(buyList[7].ItemId).Name;
            ViewBag.ItemBoughtName9 = db.Items.Find(buyList[8].ItemId).Name;
            ViewBag.ItemBoughtName10 = db.Items.Find(buyList[9].ItemId).Name;

            ViewBag.ItemBought1 = buyList[0].Quantity;
            ViewBag.ItemBought2 = buyList[1].Quantity;
            ViewBag.ItemBought3 = buyList[2].Quantity;
            ViewBag.ItemBought4 = buyList[3].Quantity;
            ViewBag.ItemBought5 = buyList[4].Quantity;
            ViewBag.ItemBought6 = buyList[5].Quantity;
            ViewBag.ItemBought7 = buyList[6].Quantity;
            ViewBag.ItemBought8 = buyList[7].Quantity;
            ViewBag.ItemBought9 = buyList[8].Quantity;
            ViewBag.ItemBought10 = buyList[9].Quantity;

            var delItems = db.Items.Where(i => i.Deleted == true).OrderByDescending(i => i.Id);
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