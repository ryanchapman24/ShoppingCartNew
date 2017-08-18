using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using System.Data.Entity;

namespace ShoppingCartNew.Models
{
    public class Universal : Controller
    {
        public ApplicationDbContext db = new ApplicationDbContext();

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (User.Identity.IsAuthenticated)
            {
                var user = db.Users.Find(User.Identity.GetUserId());
                ViewBag.FirstName = user.FirstName;
                ViewBag.LastName = user.LastName;
                ViewBag.FullName = user.FullName;

                ViewBag.CartItems = db.CartItems.Where(c => c.CustomerId == user.Id).ToList();
                decimal count = 0;
                foreach (var cartItem in db.CartItems.Where(c => c.CustomerId == user.Id).ToList())
                {
                    if (cartItem.Item.OnSale == true)
                    {
                        count += cartItem.Item.SalePrice.Value;
                    }
                    else
                    {
                        count += cartItem.Item.Price;
                    }
                }
                ViewBag.CartItemsTotalCost = count;

                base.OnActionExecuting(filterContext);
            }
        }
    }
}