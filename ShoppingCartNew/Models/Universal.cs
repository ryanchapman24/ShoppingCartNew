using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using System.Data.Entity;
using ShoppingCartNew.Models.Code_First;

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

                ViewBag.CartItems = user.CartItems.ToList();
                ViewBag.ItemTypes = db.ItemTypes.AsNoTracking().OrderBy(t => t.TypeName).ToList();
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
                ViewBag.CartItemsTotalCost = count;

                var latestItems = new List<Item>();
                var myItems = db.Items.AsNoTracking().Where(i => i.Deleted == false).OrderByDescending(i => i.Created).ToList();
                int Litem = 0;
                foreach (var latestItem in myItems)
                {
                    latestItems.Add(latestItem);
                    Litem++;
                    if (Litem == 20) { break; };
                }
                ViewBag.Latest = latestItems;
                var saleItems = db.Items.AsNoTracking().Where(i => i.Deleted == false && i.OnSale == true).ToList();
                if (saleItems.Count() > 0)
                {
                    var biggestSale = saleItems.FirstOrDefault();
                    foreach (var item in saleItems)
                    {
                        if (item.SalePercent < biggestSale.SalePercent)
                        {
                            biggestSale = item;
                        }
                    }
                    ViewBag.BiggestSale = biggestSale;
                }

                base.OnActionExecuting(filterContext);
            }
            else
            {
                ViewBag.ItemTypes = db.ItemTypes.AsNoTracking().OrderBy(t => t.TypeName).ToList();

                var latestItems = new List<Item>();
                var myItems = db.Items.AsNoTracking().OrderByDescending(i => i.Created).ToList();
                int Litem = 0;
                foreach (var latestItem in myItems)
                {
                    latestItems.Add(latestItem);
                    Litem++;
                    if (Litem == 20) { break; };
                }
                ViewBag.Latest = latestItems;
                var saleItems = db.Items.AsNoTracking().Where(i => i.OnSale == true).ToList();
                if (saleItems.Count() > 0)
                {
                    var biggestSale = saleItems.FirstOrDefault();
                    foreach (var item in saleItems)
                    {
                        if (item.SalePercent < biggestSale.SalePercent)
                        {
                            biggestSale = item;
                        }
                    }
                    ViewBag.BiggestSale = biggestSale;
                }
            }
        }
    }
}