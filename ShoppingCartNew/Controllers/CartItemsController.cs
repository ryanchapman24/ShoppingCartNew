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
    public class CartItemsController : Universal
    {
        // GET: CartItems
        public ActionResult Index()
        {
            var user = db.Users.Find(User.Identity.GetUserId());

            return View(user.CartItems.OrderBy(i => i.Id));
        }

        // GET: CartItems/Details/5
        //public ActionResult Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    CartItem cartItem = db.CartItems.Find(id);
        //    if (cartItem == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(cartItem);
        //}

        // GET: CartItems/Create
        //public ActionResult Create()
        //{
        //    return View();
        //}

        // POST: CartItems/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(int? id, int? quantity)
        {
            int increment = 1;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Item item = db.Items.Find(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            if (quantity != null) {
                increment = quantity.Value;
            }
            var user = db.Users.Find(User.Identity.GetUserId());
            if (db.CartItems.Where(i => i.CustomerId == user.Id).Any(i => i.ItemId == id.Value))
            {
                var existingCartItem = db.CartItems.Where(i => i.CustomerId == user.Id).FirstOrDefault(i => i.ItemId == id.Value);
                existingCartItem.Count += increment;
                db.SaveChanges();
                return Redirect(HttpContext.Request.UrlReferrer.AbsoluteUri);
            }
            else
            {
                CartItem cartItem = new CartItem();
                cartItem.Count = increment;
                cartItem.ItemId = id.Value;
                cartItem.Created = System.DateTime.Now;
                cartItem.CustomerId = user.Id;
                db.CartItems.Add(cartItem);
                db.SaveChanges();
            }

            return Redirect(HttpContext.Request.UrlReferrer.AbsoluteUri);
        }

        // GET: CartItems/Edit/5
        //public ActionResult Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    CartItem cartItem = db.CartItems.Find(id);
        //    if (cartItem == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(cartItem);
        //}

        // POST: CartItems/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "Id,ItemId,Count,CustomerId,Created")] CartItem cartItem)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(cartItem).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    return View(cartItem);
        //}

        // GET: CartItems/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    CartItem cartItem = db.CartItems.Find(id);
        //    if (cartItem == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(cartItem);
        //}

        // POST: CartItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var user = db.Users.Find(User.Identity.GetUserId());
            CartItem cartItem = db.CartItems.Find(id);
            if (cartItem.CustomerId != user.Id)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            db.CartItems.Remove(cartItem);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // POST: CartItems/EmptyCart
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EmptyCart()
        {
            var user = db.Users.Find(User.Identity.GetUserId());

            var myCartItems = db.CartItems.Where(i => i.CustomerId == user.Id);
            foreach (var item in myCartItems)
            {
                db.CartItems.Remove(item);
            }
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateCart(List<int> quantities)
        {
            var user = db.Users.Find(User.Identity.GetUserId());

            var myCartItems = user.CartItems.OrderBy(i => i.Id).ToArray();
            var number = 0;
            foreach (var quantity in quantities)
            {
                var cartItem = myCartItems[number];
                cartItem.Count = quantity;
                db.SaveChanges();
                number++;
            }

            //foreach (var val in quantities)
            //{
            //    int index = val.IndexOf("-");
            //    string itemId = val.Substring(0, index);
            //    string quantity = val.Substring(index + 1, val.Length);
            //    int iId = Convert.ToInt32(itemId);
            //    int q = Convert.ToInt32(quantity);
            //    var item = db.CartItems.Find(iId);
            //    item.Count = q;
            //    db.SaveChanges();
            //}

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
