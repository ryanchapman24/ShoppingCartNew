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
using System.IO;

namespace ShoppingCartNew.Controllers
{
    public class ItemsController : Universal
    {
        // GET: Items
        public ActionResult Index(int? tId)
        {
            var myItems = db.Items.Where(i => i.Deleted == false).ToList();
            if (tId != null)
            {
                ItemType itemType = db.ItemTypes.Find(tId.Value);
                myItems = db.Items.Where(i => i.ItemTypeId == tId.Value).ToList();
                ViewBag.ItemType = "(Filter: " + itemType.TypeName +  ")";
            }
            ViewBag.ItemCount = myItems.Count();
            return View(myItems.OrderByDescending(i => i.Id));
        }

        // GET: Items/SearchResults
        public ActionResult SearchResults(string search)
        {
            ViewBag.SearchAttempt = search;
            var myItems = db.Items.Where(i => i.Deleted == false).ToList();

            if (search == "")
            {
                return View(myItems.Where(i => i.Id == 0).ToList());
            }
            return View(myItems.Where(i => i.Name.Contains(search) || i.ItemType.TypeName.Contains(search) || i.Description.Contains(search)).OrderByDescending(i => i.Id).ToList());
        }

        // GET: Items/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Item item = db.Items.Find(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }

        // GET: Items/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            ViewBag.ItemTypeId = new SelectList(db.ItemTypes, "Id", "TypeName");
            return View();
        }

        // POST: Items/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Created,Updated,Name,Price,MediaURL,Description,OnSale,SalePrice,ItemTypeId")] Item item, HttpPostedFileBase image)
        {
            if (ModelState.IsValid)
            {
                var iPic = item.MediaURL;
                var defaultMedia = "/assets/img/catalog/1.png";
                if (ImageUploadValidator.IsWebFriendlyImage(image))
                {
                    //var fileName = Path.GetFileName(image.FileName);
                    //image.SaveAs(Path.Combine(Server.MapPath("~/ProfilePics/"), fileName));
                    //iPic = "/ProfilePics/" + fileName;

                    //Counter
                    var num = 0;
                    //Gets Filename without the extension
                    var fileName = Path.GetFileNameWithoutExtension(image.FileName);
                    iPic = Path.Combine("/ItemImages/", fileName + Path.GetExtension(image.FileName));
                    //Checks if iPic matches any of the current attachments, 
                    //if so it will loop and add a (number) to the end of the filename
                    while (db.Items.Any(u => u.MediaURL == iPic))
                    {
                        //Sets "filename" back to the default value
                        fileName = Path.GetFileNameWithoutExtension(image.FileName);
                        //Add's parentheses after the name with a number ex. filename(4)
                        fileName = string.Format(fileName + "(" + ++num + ")");
                        //Makes sure iPic gets updated with the new filename so it could check
                        iPic = Path.Combine("/ItemImages/", fileName + Path.GetExtension(image.FileName));
                    }
                    image.SaveAs(Path.Combine(Server.MapPath("~/ItemImages/"), fileName + Path.GetExtension(image.FileName)));
                }
                else
                {
                    iPic = defaultMedia;
                }

                item.MediaURL = iPic;
                item.Created = System.DateTime.Now;
                if (item.OnSale == false || (item.OnSale == true && item.SalePrice > item.Price))
                {
                    item.SalePrice = null;
                }
                if (item.SalePrice == null)
                {
                    item.OnSale = false;
                }
                item.Deleted = false;
                db.Items.Add(item);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ItemTypeId = new SelectList(db.ItemTypes, "Id", "TypeName", item.ItemTypeId);
            return View(item);
        }

        // GET: Items/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Item item = db.Items.Find(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            ViewBag.ItemTypeId = new SelectList(db.ItemTypes, "Id", "TypeName", item.ItemTypeId);
            return View(item);
        }

        // POST: Items/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Created,Updated,Name,Price,MediaURL,Description,OnSale,SalePrice,ItemTypeId")] Item item, string mediaURL, HttpPostedFileBase image)
        {
            if (ModelState.IsValid)
            {
                db.Items.Attach(item);
                db.Entry(item).Property("Updated").IsModified = true;
                db.Entry(item).Property("Name").IsModified = true;
                db.Entry(item).Property("Price").IsModified = true;
                db.Entry(item).Property("MediaURL").IsModified = true;
                db.Entry(item).Property("Description").IsModified = true;
                db.Entry(item).Property("OnSale").IsModified = true;
                db.Entry(item).Property("SalePrice").IsModified = true;

                var iPic = item.MediaURL;
                if (ImageUploadValidator.IsWebFriendlyImage(image))
                {
                    //var fileName = Path.GetFileName(image.FileName);
                    //image.SaveAs(Path.Combine(Server.MapPath("~/ProfilePics/"), fileName));
                    //iPic = "/ProfilePics/" + fileName;

                    //Counter
                    var num = 0;
                    //Gets Filename without the extension
                    var fileName = Path.GetFileNameWithoutExtension(image.FileName);
                    iPic = Path.Combine("/ItemImages/", fileName + Path.GetExtension(image.FileName));
                    //Checks if iPic matches any of the current attachments, 
                    //if so it will loop and add a (number) to the end of the filename
                    while (db.Items.Any(u => u.MediaURL == iPic))
                    {
                        //Sets "filename" back to the default value
                        fileName = Path.GetFileNameWithoutExtension(image.FileName);
                        //Add's parentheses after the name with a number ex. filename(4)
                        fileName = string.Format(fileName + "(" + ++num + ")");
                        //Makes sure iPic gets updated with the new filename so it could check
                        iPic = Path.Combine("/ItemImages/", fileName + Path.GetExtension(image.FileName));
                    }
                    image.SaveAs(Path.Combine(Server.MapPath("~/ItemImages/"), fileName + Path.GetExtension(image.FileName)));
                }
                else
                {
                    iPic = mediaURL;
                }

                item.MediaURL = iPic;
                item.Updated = System.DateTime.Now;
                if (item.OnSale == false || (item.OnSale == true && item.SalePrice > item.Price))
                {
                    item.SalePrice = null;
                }
                if (item.SalePrice == null)
                {
                    item.OnSale = false;
                }
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ItemTypeId = new SelectList(db.ItemTypes, "Id", "TypeName", item.ItemTypeId);
            return View(item);
        }

        // GET: Items/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Item item = db.Items.Find(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }

        // POST: Items/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Item item = db.Items.Find(id);
            item.Deleted = true;
            db.SaveChanges();
            foreach (var cart in db.CartItems.Where(c => c.ItemId == id).ToList())
            {
                db.CartItems.Remove(cart);
                db.SaveChanges();
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
