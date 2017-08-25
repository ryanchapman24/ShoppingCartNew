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
using Microsoft.AspNet.Identity;

namespace ShoppingCartNew.Controllers
{
    [Authorize]
    public class OrdersController : Universal
    {
        // GET: Orders
        public ActionResult Index()
        {
            return View(db.Orders.ToList());
        }

        // GET: Orders/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = db.Users.Find(User.Identity.GetUserId());
            Order order = db.Orders.Find(id);
            if (order == null || order.CustomerId != user.Id)
            {
                return HttpNotFound();
            }
            ViewBag.AddressString = order.Address + ", " + order.City + ", " + order.State.Abbreviation + " " + order.Zipcode;
            return View(order);
        }

        // GET: Orders/Create
        public ActionResult Create()
        {
            var user = db.Users.Find(User.Identity.GetUserId());
            if (user.CartItems.Count == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (user.CreditCards.Where(c => c.Expired == false && c.Deleted == false).Count() == 0)
            {
                return RedirectToAction("Index", "Manage", new { oC = true });
            }
            var activeCreditCards = user.CreditCards.Where(c => c.Expired.Value == false);
            ViewBag.EstimatedDelivery = System.DateTime.Now.AddDays(4);
            ViewBag.CreditCardId = new SelectList(activeCreditCards, "Id", "CardNumber");
            ViewBag.StateId = new SelectList(db.States, "Id", "StateName");
            ViewBag.StateId = new SelectList(db.States, "Id", "StateName");
            return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Address,City,State,Zipcode,OrderDate,CustomerId,StateId,CreditCardId,SubTotal,Shipping,Taxes,FinalTotal")] Order order)
        {
            var user = db.Users.Find(User.Identity.GetUserId());
            var shipping = ViewBag.CartItemsTotalCost / 10;
            var taxes = ViewBag.CartItemsTotalCost / 20;
            var total = ViewBag.CartItemsTotalCost + shipping + taxes;
            if (ModelState.IsValid)
            {
                order.CustomerId = user.Id;
                order.OrderDate = System.DateTime.Now;
                order.EstimatedDelivery = System.DateTime.Now.AddDays(4);
                order.SubTotal = ViewBag.CartItemsTotalCost;
                order.Shipping = shipping;
                order.Taxes = taxes;
                order.FinalTotal = total;
                db.Orders.Add(order);
                db.SaveChanges();

                var myCart = db.CartItems.Where(c => c.CustomerId == user.Id).ToList();
                foreach (var cartItem in myCart)
                {
                    OrderItem orderItem = new OrderItem();
                    orderItem.OrderId = order.Id;
                    orderItem.ItemId = cartItem.ItemId;
                    orderItem.Quantity = cartItem.Count;
                    if (cartItem.Item.OnSale == true)
                    {
                        orderItem.UnitPrice = cartItem.Item.SalePrice.Value;
                    }
                    else
                    {
                        orderItem.UnitPrice = cartItem.Item.Price;
                    }
                    orderItem.TotalPrice = cartItem.UnitTotal;
                    db.OrderItems.Add(orderItem);
                    db.CartItems.Remove(cartItem);
                    db.SaveChanges();
                }

                return RedirectToAction("Details", new { id = order.Id });
            }
            var activeCreditCards = user.CreditCards.Where(c => c.Expired.Value == false);
            ViewBag.CreditCardId = new SelectList(activeCreditCards, "Id", "CardNumber",order.CreditCardId);
            ViewBag.StateId = new SelectList(db.States, "Id", "StateName",order.StateId);
            return View(order);
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
