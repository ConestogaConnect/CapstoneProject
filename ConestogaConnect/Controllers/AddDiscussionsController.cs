using System;
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
    [Authorize(Roles = "Student,Admin")]
    public class AddDiscussionsController : BaseController
    {
       
        [Authorize(Roles = "Student,Admin")]
        // GET: AddDiscussions
        public ActionResult Index()
        {
            var discussions = db.Discussions.Include(d => d.AspNetUser);
            return View(discussions.ToList());
        }

        [Authorize(Roles = "Admin")]
        public ActionResult DisplayDiscussions()
        {
            var discussions = db.Discussions.Include(d => d.AspNetUser);
            return View(discussions.ToList());
        }

        // GET: AddDiscussions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Discussion discussion = db.Discussions.Find(id);
            if (discussion == null)
            {
                return HttpNotFound();
            }

            return View(discussion);
        }

        // GET: AddDiscussions/Create
        public ActionResult Create()
        {
            ViewBag.UserId = new SelectList(db.AspNetUsers, "Id", "Email");
            return View();
        }

        // POST: AddDiscussions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Student")]
        public ActionResult Create([Bind(Include = "Topic,UserId,Posted_Date,Last_Updated")] Discussion discussion)
        {
            if (ModelState.IsValid)
            {
                discussion.UserId = User.Identity.GetUserId();
                discussion.Posted_Date = DateTime.Now;
                discussion.Last_Updated = null;
                db.Discussions.Add(discussion);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.UserId = new SelectList(db.AspNetUsers, "Id", "Email", discussion.UserId);
            return View(discussion);
        }

        // GET: AddDiscussions/Edit/5
        [Authorize(Roles = "Student")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Discussion discussion = db.Discussions.Find(id);
            if (discussion == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserId = new SelectList(db.AspNetUsers, "Id", "Email", discussion.UserId);
            return View(discussion);
        }

        // POST: AddDiscussions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Discussion discussion)
        {
            if (ModelState.IsValid)
            {
                discussion.Last_Updated = System.DateTime.Now;
                db.Entry(discussion).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UserId = new SelectList(db.AspNetUsers, "Id", "Email", discussion.UserId);
            return View(discussion);
        }

        // GET: AddDiscussions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Discussion discussion = db.Discussions.Find(id);
            if (discussion == null)
            {
                return HttpNotFound();
            }
            return View(discussion);
        }

        // POST: AddDiscussions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Discussion discussion = db.Discussions.Find(id);
            db.Discussions.Remove(discussion);
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
