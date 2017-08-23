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
            var myItems = db.Items.Where(i => i.Deleted == true).OrderByDescending(i => i.Id);
            ViewBag.ItemCount = myItems.Count();
            return View(myItems.ToList());
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
    }
}