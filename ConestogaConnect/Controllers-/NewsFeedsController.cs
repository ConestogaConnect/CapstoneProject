﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ConestogaConnect.Models;
using Microsoft.AspNet.Identity;

namespace ConestogaConnect.Controllers
{
    public class NewsFeedsController : BaseController
    {
        //private NewsFeedEntities db = new NewsFeedEntities();

        // GET: NewsFeeds
        public ActionResult Index()
        {
            return View(db.NewsFeeds.ToList());
        }

        // GET: NewsFeeds
        [Authorize(Roles = "Student,Admin")]
        public ActionResult _NewsFeedPartial()
        {
            return PartialView("_NewsFeedPartial", db.NewsFeeds.OrderByDescending(x=>x.Id).Take(10).ToList());
        }

        // GET: NewsFeeds/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NewsFeed newsFeed = db.NewsFeeds.Find(id);
            if (newsFeed == null)
            {
                return HttpNotFound();
            }
            return View(newsFeed);
        }

        // GET: NewsFeeds/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: NewsFeeds/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "title,newsDetail,AddedOn,UpdatedOn,UserId")] NewsFeed newsFeed)
        {
            if (ModelState.IsValid)
            {
                newsFeed.UserId = User.Identity.GetUserId();
                newsFeed.AddedOn = System.DateTime.Now;
                db.NewsFeeds.Add(newsFeed);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(newsFeed);
        }

        // GET: NewsFeeds/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NewsFeed newsFeed = db.NewsFeeds.Find(id);
            if (newsFeed == null)
            {
                return HttpNotFound();
            }
            return View(newsFeed);
        }

        // POST: NewsFeeds/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,title,newsDetail,AddedOn,UpdatedOn,UserId")] NewsFeed newsFeed)
        {
            if (ModelState.IsValid)
            {
                db.Entry(newsFeed).State = EntityState.Modified;
                newsFeed.UpdatedOn = System.DateTime.Now;
                newsFeed.UserId = User.Identity.GetUserId();
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(newsFeed);
        }

        // GET: NewsFeeds/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NewsFeed newsFeed = db.NewsFeeds.Find(id);
            if (newsFeed == null)
            {
                return HttpNotFound();
            }
            return View(newsFeed);
        }

        // POST: NewsFeeds/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            NewsFeed newsFeed = db.NewsFeeds.Find(id);
            db.NewsFeeds.Remove(newsFeed);
            db.SaveChanges();
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
