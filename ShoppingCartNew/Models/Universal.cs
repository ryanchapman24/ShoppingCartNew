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

        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (User.Identity.IsAuthenticated)
            {
                var user = db.Users.Find(User.Identity.GetUserId());

                ViewBag.FirstName = user.FirstName;
                ViewBag.LastName = user.LastName;
                ViewBag.FullName = user.FullName;

                ViewBag.TotalCartItems = user.CartItems.Sum(c => c.Count);
                ViewBag.CartItems = user.CartItems.OrderBy(c => c.Id).ToList();
                ViewBag.ItemTypes = db.ItemTypes.Where(t => t.Id < 7).OrderBy(t => t.TypeName).ToList();
                decimal count = 0;
                foreach (var cartItem in user.CartItems)
                {
                    if (cartItem.Item.OnSale == true)
                    {
                        count += cartItem.Item.SalePrice.Value * cartItem.Count;
                    }
                    else
                    {
                        count += cartItem.Item.Price * cartItem.Count;
                    }
                }
                ViewBag.CartItemsTotalCost = count;

                var latestItems = new List<Item>();
                var myItems = db.Items.Where(i => i.Deleted == false).OrderByDescending(i => i.Created).ToList();
                int Litem = 0;
                foreach (var latestItem in myItems)
                {
                    latestItems.Add(latestItem);
                    Litem++;
                    if (Litem == 20) { break; };
                }
                ViewBag.Latest = latestItems;
                var saleItems = db.Items.Where(i => i.Deleted == false && i.OnSale == true).ToList();
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
            else
            {
                ViewBag.ItemTypes = db.ItemTypes.Where(t => t.Id < 7).OrderBy(t => t.TypeName).ToList();

                var latestItems = new List<Item>();
                var myItems = db.Items.OrderByDescending(i => i.Created).ToList();
                int Litem = 0;
                foreach (var latestItem in myItems)
                {
                    latestItems.Add(latestItem);
                    Litem++;
                    if (Litem == 20) { break; };
                }
                ViewBag.Latest = latestItems;
                var saleItems = db.Items.Where(i => i.OnSale == true).ToList();
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

            base.OnActionExecuted(filterContext);
        }
    }
}