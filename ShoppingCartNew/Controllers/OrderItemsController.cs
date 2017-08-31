using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ShoppingCartNew.Models;
using ShoppingCartNew.Models.Code_First;

namespace ShoppingCartNew.Controllers
{
    [Authorize]
    public class OrderItemsController : Universal
    {
        // GET: OrderItems
        public ActionResult Index()
        {
            var OrderItems = db.OrderItems.Include(o => o.Item).Include(o => o.Order);
            return View(OrderItems.ToList());
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
