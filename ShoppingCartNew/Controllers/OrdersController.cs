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
        public class Card
        {
            public int Id { get; set; }
            public string TypePlusNumber { get; set; }
        }

        // GET: Orders/Details/5
        public ActionResult Details(int? id, bool? justOrdered)
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
            ViewBag.JustOrdered = false;
            if (justOrdered == true)
            {
                ViewBag.JustOrdered = true;
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
            var cards = new List<Card>();
            foreach (var card in user.CreditCards.Where(c => c.Expired == false && c.Deleted == false))
            {
                var selection = new Card();
                selection.Id = card.Id;
                selection.TypePlusNumber = card.CardNumber + " [" + card.CardType.CardName + "]";

                cards.Add(selection);
            }
            ViewBag.EstimatedDelivery = System.DateTime.Now.AddDays(4);
            ViewBag.CreditCardId = new SelectList(cards, "Id", "TypePlusNumber");
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
            decimal count = 0;
            foreach (var cartItem in user.CartItems)
            {
                if (cartItem.Item.OnSale == true)
                {
                    count += cartItem.Item.SalePrice.Value * Convert.ToDecimal(cartItem.Count);
                }
                else
                {
                    count += cartItem.Item.Price * Convert.ToDecimal(cartItem.Count);
                }
            }
            var shipping = count / 10;
            var taxes = count / 20;
            var total = count + shipping + taxes;
            if (ModelState.IsValid)
            {
                order.CustomerId = user.Id;
                order.OrderDate = System.DateTime.Now;
                order.EstimatedDelivery = System.DateTime.Now.AddDays(4);
                order.SubTotal = count;
                order.Shipping = shipping;
                order.Taxes = taxes;
                order.FinalTotal = total;
                db.Orders.Add(order);
                db.SaveChanges();

                foreach (var cartItem in user.CartItems.ToList())
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

                return RedirectToAction("Details", new { id = order.Id, justOrdered = true });
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
