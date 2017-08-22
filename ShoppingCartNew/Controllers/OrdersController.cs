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
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // GET: Orders/Create
        public ActionResult Create()
        {
            var user = db.Users.Find(User.Identity.GetUserId());
            var activeCreditCards = user.CreditCards.Where(c => c.Expired.Value == false);
            ViewBag.EstimatedDelivery = System.DateTime.Now.AddDays(4);
            ViewBag.CardId = new SelectList(activeCreditCards, "Id", "CardNumber");
            ViewBag.StateId = new SelectList(db.States, "Id", "StateName");
            return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Address,City,State,Zipcode,OrderDate,CustomerId,CreditCardId,SubTotal,Shipping,Taxes,FinalTotal")] Order order, int CardId, int StateId)
        {
            var user = db.Users.Find(User.Identity.GetUserId());
            var shipping = ViewBag.CartItemsTotalCost / 10;
            var taxes = ViewBag.CartItemsTotalCost / 20;
            var total = ViewBag.CartItemsTotalCost + shipping + taxes;
            if (ModelState.IsValid)
            {
                CreditCard creditCard = db.CreditCards.Find(CardId);
                order.CustomerId = user.Id;
                order.CreditCardId = CardId;
                order.OrderDate = System.DateTime.Now;
                order.EstimatedDelivery = System.DateTime.Now.AddDays(4);
                order.SubTotal = ViewBag.CartItemsTotalCost;
                order.Shipping = shipping;
                order.Taxes = taxes;
                order.FinalTotal = total;
                order.StateId = StateId;
                db.Orders.Add(order);
                db.SaveChanges();
                return RedirectToAction("Details", new { id = order.Id });
            }
            var activeCreditCards = user.CreditCards.Where(c => c.Expired.Value == false);
            ViewBag.CardId = new SelectList(activeCreditCards, "Id", "CardNumber",user.CreditCards.Where(c => c.Id == CardId));
            ViewBag.StateId = new SelectList(db.States, "Id", "StateName",order.StateId);
            return View(order);
        }

        // GET: Orders/Edit/5
        //public ActionResult Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Order order = db.Orders.Find(id);
        //    if (order == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(order);
        //}

        //// POST: Orders/Edit/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "Id,Completed,Address,City,State,Zipcode,OrderDate,CustomerId,CardNumber,CardCVC,CardType,CardMonth,CardYear,SubTotal,Shipping,Taxes,FinalTotal")] Order order)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(order).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    return View(order);
        //}

        // GET: Orders/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Order order = db.Orders.Find(id);
        //    if (order == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(order);
        //}

        //// POST: Orders/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    Order order = db.Orders.Find(id);
        //    db.Orders.Remove(order);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

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
