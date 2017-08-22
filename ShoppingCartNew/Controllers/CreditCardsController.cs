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
    public class CreditCardsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: CreditCards
        //public ActionResult Index()
        //{
        //    var user = db.Users.Find(User.Identity.GetUserId());

        //    return View(user.CreditCards.ToList());
        //}

        // GET: CreditCards/Details/5
        //public ActionResult Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    CreditCard creditCard = db.CreditCards.Find(id);
        //    if (creditCard == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(creditCard);
        //}

        // GET: CreditCards/Create
        //public ActionResult Create()
        //{
        //    ViewBag.CardTypeId = new SelectList(db.CardTypes, "Id", "CardName");
        //    ViewBag.MonthId = new SelectList(db.Months, "Id", "MonthName");
        //    ViewBag.YearId = new SelectList(db.Years, "Id", "YearName");
        //    return View();
        //}

        // POST: CreditCards/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,CardTypeId,CardNumber,CVC,MonthId,YearId,Address,City,StateId,Zipcode,CustomerId")] CreditCard creditCard)
        {
            var user = db.Users.Find(User.Identity.GetUserId());

            if (ModelState.IsValid)
            {
                creditCard.CustomerId = user.Id;
                db.CreditCards.Add(creditCard);
                db.SaveChanges();
                return RedirectToAction("Index","Manage");
            }

            ViewBag.CardTypeId = new SelectList(db.CardTypes, "Id", "CardName", creditCard.CardTypeId);
            ViewBag.StateId = new SelectList(db.States, "Id", "StateName", creditCard.StateId);
            ViewBag.MonthId = new SelectList(db.Months, "Id", "MonthName", creditCard.MonthId);
            ViewBag.YearId = new SelectList(db.Years, "Id", "YearName", creditCard.YearId);

            return RedirectToAction("Index", "Manage");
        }

        // GET: CreditCards/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CreditCard creditCard = db.CreditCards.Find(id);
            if (creditCard == null)
            {
                return HttpNotFound();
            }

            ViewBag.CardTypeId = new SelectList(db.CardTypes, "Id", "CardName", creditCard.CardTypeId);
            ViewBag.StateId = new SelectList(db.States, "Id", "StateName", creditCard.StateId);
            ViewBag.MonthId = new SelectList(db.Months, "Id", "MonthName", creditCard.MonthId);
            ViewBag.YearId = new SelectList(db.Years, "Id", "YearName", creditCard.YearId);
            return View(creditCard);
        }

        // POST: CreditCards/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,CardTypeId,CardNumber,CVC,MonthId,YearId,Address,City,StateId,Zipcode,CustomerId")] CreditCard creditCard)
        {
            if (ModelState.IsValid)
            {
                db.Entry(creditCard).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CardTypeId = new SelectList(db.CardTypes, "Id", "CardName", creditCard.CardTypeId);
            ViewBag.StateId = new SelectList(db.States, "Id", "StateName", creditCard.StateId);
            ViewBag.MonthId = new SelectList(db.Months, "Id", "MonthName", creditCard.MonthId);
            ViewBag.YearId = new SelectList(db.Years, "Id", "YearName", creditCard.YearId);
            return View(creditCard);
        }

        // GET: CreditCards/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    CreditCard creditCard = db.CreditCards.Find(id);
        //    if (creditCard == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(creditCard);
        //}

        // POST: CreditCards/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CreditCard creditCard = db.CreditCards.Find(id);
            db.CreditCards.Remove(creditCard);
            db.SaveChanges();
            return RedirectToAction("Index","Manage");
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
