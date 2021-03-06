﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using ConestogaConnect.Models;
using Microsoft.AspNet.Identity;
using SendGrid.Helpers.Mail;

namespace ConestogaConnect.Controllers
{
    public class ActivitiesController : BaseController
    {

        // GET: Activities
        public ActionResult Index(string name, string desc, int? types, string afrom, string ato, string ufrom, string uto)
        {
            var _activities = db.Activities.ToList();
            var loginUserId = User.Identity.GetUserId();
            var _users = db.AspNetUsers.ToList();
            var _activityinterest = db.ActivityInterests.Where(x => x.UserId == loginUserId).ToList();

            if (!string.IsNullOrEmpty(name))
            {
                _activities = _activities.Where(x => x.ActivityName.ToLower().Contains(name.ToLower())).ToList();
            }
            if (!string.IsNullOrEmpty(desc))
            {
                _activities = _activities.Where(x => x.Description.ToLower().Contains(desc.ToLower())).ToList();
            }
            if (types != null && types > 0)
            {
                _activities = _activities.Where(x => x.ActivityTypeId == types).ToList();
            }
            if (!string.IsNullOrEmpty(afrom))
            {
                DateTime date = DateTime.ParseExact(afrom, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                _activities = _activities.Where(x => x.Added_On >= date).ToList();
            }
            if (!string.IsNullOrEmpty(ato))
            {
                DateTime date = DateTime.ParseExact(ato, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                _activities = _activities.Where(x => x.Added_On <= date).ToList();
            }
            if (!string.IsNullOrEmpty(ufrom))
            {
                DateTime date = DateTime.ParseExact(ufrom, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                _activities = _activities.Where(x => x.Updated_On >= date).ToList();
            }
            if (!string.IsNullOrEmpty(uto))
            {
                DateTime date = DateTime.ParseExact(uto, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                _activities = _activities.Where(x => x.Updated_On <= date).ToList();
            }
            var typeList = db.ActivityTypes.Select(x => new SelectListItem { Text = x.ActivityType1, Value = x.Id.ToString(), Selected = x.Id == types }).ToList();
            ViewBag.types = typeList;
            ViewBag.name = name;

            ViewBag.desc = desc;

            ViewBag.pdfrom = afrom;
            ViewBag.pdto = ato;
            ViewBag.ufrom = ufrom;
            ViewBag.uto = uto;
           
            var vm = _activities.Select(x => new ActivityViewModel
            {
                Id=x.Id,
                ActivityName = x.ActivityName,
                Description = x.Description,
                UserId = x.UserId,
                ShownInterest = _activityinterest.Where(t => t.ActivityId == x.Id).Count() == 0 ? false : true,
                ActivityTypeId = x.ActivityTypeId,
                ActivityType= x.ActivityType,
                Updated_On = x.Updated_On,
                Added_On = x.Added_On,
                FirstName= _users.Where(u => u.Id == x.UserId).FirstOrDefault().FirstName,
                LastName = _users.Where(u => u.Id == x.UserId).FirstOrDefault().LastName,
            }).OrderByDescending(x => x.Id).ToList();

            ViewBag.interestedUsers = db.ActivityInterests.Count();
            return View(vm);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult DisplayActivity(string name, string desc, int? types, string afrom, string ato, string ufrom, string uto)
        {
            var obj = db.Activities.ToList();
            if (!string.IsNullOrEmpty(name))
            {
                obj = obj.Where(x => x.ActivityName.ToLower().Contains(name.ToLower())).ToList();
            }
            if (!string.IsNullOrEmpty(desc))
            {
                obj = obj.Where(x => x.Description.ToLower().Contains(desc.ToLower())).ToList();
            }
            if (types != null && types > 0)
            {
                obj = obj.Where(x => x.ActivityTypeId == types).ToList();
            }
            if (!string.IsNullOrEmpty(afrom))
            {
                DateTime date = DateTime.ParseExact(afrom, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                obj = obj.Where(x => x.Added_On >= date).ToList();
            }
            if (!string.IsNullOrEmpty(ato))
            {
                DateTime date = DateTime.ParseExact(ato, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                obj = obj.Where(x => x.Added_On <= date).ToList();
            }
            if (!string.IsNullOrEmpty(ufrom))
            {
                DateTime date = DateTime.ParseExact(ufrom, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                obj = obj.Where(x => x.Updated_On >= date).ToList();
            }
            if (!string.IsNullOrEmpty(uto))
            {
                DateTime date = DateTime.ParseExact(uto, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                obj = obj.Where(x => x.Updated_On <= date).ToList();
            }
            var typeList = db.ActivityTypes.Select(x => new SelectListItem { Text = x.ActivityType1, Value = x.Id.ToString(), Selected = x.Id == types }).ToList();
            ViewBag.types = typeList;
            ViewBag.name = name;
            ViewBag.desc = desc;

            ViewBag.pdfrom = afrom;
            ViewBag.pdto = ato;
            ViewBag.ufrom = ufrom;
            ViewBag.uto = uto;

            return View(obj);
        }

        // GET: Activities/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Activity activity = db.Activities.Find(id);
            if (activity == null)
            {
                return HttpNotFound();
            }
            return View(activity);
        }

        // GET: Activities/Create
        public ActionResult Create()
        {
            var types = db.ActivityTypes.Select(x => new SelectListItem { Text = x.ActivityType1, Value = x.Id.ToString() }).ToList();
            ViewBag.types = types;
            return View();
        }

        // POST: Activities/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ActivityName,Description,ActivityTypeId")] Activity activity)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    activity.UserId = User.Identity.GetUserId();
                    activity.Added_On = DateTime.Now;
                    db.Activities.Add(activity);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (DbEntityValidationException ex)
            {
                foreach (var entityValidationErrors in ex.EntityValidationErrors)
                {
                    foreach (var validationError in entityValidationErrors.ValidationErrors)
                    {
                        Response.Write("Property: " + validationError.PropertyName + " Error: " + validationError.ErrorMessage);
                    }
                }
            }
            var types = db.ActivityTypes.Select(x => new SelectListItem { Text = x.ActivityType1, Value = x.Id.ToString() }).ToList();
            ViewBag.types = types;
            return View(activity);
        }

        // GET: Activities/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Activity activity = db.Activities.Find(id);
            if (activity == null)
            {
                return HttpNotFound();
            }
            var types = db.ActivityTypes.Select(x => new SelectListItem { Text = x.ActivityType1, Value = x.Id.ToString() }).ToList();
            ViewBag.types = types;
            return View(activity);
        }

        // POST: Activities/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ActivityName,Description,ActivityTypeId")] Activity activity)
        {
            if (ModelState.IsValid)
            {
                db.Entry(activity).State = EntityState.Modified;
                activity.Updated_On = System.DateTime.Now;
                activity.UserId = User.Identity.GetUserId();
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            var types = db.ActivityTypes.Select(x => new SelectListItem { Text = x.ActivityType1, Value = x.Id.ToString() }).ToList();
            ViewBag.types = types;
            return View(activity);
        }

        // GET: Activities/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Activity activity = db.Activities.Find(id);
            if (activity == null)
            {
                return HttpNotFound();
            }
            return View(activity);
        }

        // POST: Activities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Activity activity = db.Activities.Find(id);
            db.Activities.Remove(activity);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Student")]
        public ActionResult ShowInterest(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Activity activity = db.Activities.Find(id);
            if (activity == null)
            {
                return HttpNotFound();
            }
            var userid = User.Identity.GetUserId();
            ActivityInterest sts = db.ActivityInterests.Where(x => x.ActivityId == id && x.UserId == userid).FirstOrDefault();
            if (sts == null)
            {
                sts = new ActivityInterest();
            }
            var body = "";
            var loginUser = GetUserInfoByUserId(userid);

            if (sts.Id > 0)
            {
                db.Entry(sts).State = EntityState.Deleted;
            }
            else
            {
                sts.UserId = userid;
                sts.ActivityId = id;
                sts.AddedOn = System.DateTime.Now;
                body = loginUser.FullName + " showed interest in activity: " + activity.ActivityName;
                db.ActivityInterests.Add(sts);
            }
            db.SaveChanges();
            var preference = GetUserNotificationPreference("activity", activity.UserId);
            var userModel = GetUserInfoByUserId(sts.Activity.UserId);
            if (preference.IsEmail)
            {
                var recipients = new List<EmailAddress>();
                recipients.Add(new EmailAddress(userModel.Email, userModel.FullName));
                SendEmailViaSendGrid("Interest showed in activity", body, recipients);
            }

            if (preference.IsSms)
            {
                SendSmsViaClickaTell("+16475372344", body);
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
