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

        // POST: CartItems/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(int? id, int? quantity, string search)
        {
            int increment = 1;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Item item = db.Items.Find(id);
            if (item == null || item.Deleted == true)
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
                if (search != null)
                {
                    return RedirectToAction("SearchResults","Items", new { search = search });
                }
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
            if (search != null)
            {
                return RedirectToAction("SearchResults", "Items", new { search = search });
            }
            return Redirect(HttpContext.Request.UrlReferrer.AbsoluteUri);
        }

        // POST: CartItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id, bool? orderCreate)
        {
            var user = db.Users.Find(User.Identity.GetUserId());
            CartItem cartItem = db.CartItems.Find(id);
            if (cartItem.CustomerId != user.Id)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            db.CartItems.Remove(cartItem);
            db.SaveChanges();
            if (orderCreate != null && orderCreate == true)
            {
                if (user.CartItems.Count == 0)
                {
                    return RedirectToAction("Index");
                }
                return RedirectToAction("Create", "Orders");
            }
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
        public ActionResult UpdateCart(List<int> quantities, bool? orderCreate)
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
            if (orderCreate != null && orderCreate == true)
            {
                return RedirectToAction("Create", "Orders");
            }
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
