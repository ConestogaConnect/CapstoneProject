﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using ConestogaConnect.Models;
using Microsoft.AspNet.Identity;
using SendGrid.Helpers.Mail;

namespace ConestogaConnect.Controllers
{
    public class MeetingsController : BaseController
    {
       
        [Authorize(Roles = "Student,Admin")]
        // GET: Meetings
        public ActionResult Index(string mtitle, string desc, string sub, string loc, int? program, int? sem, string add, string mfrom, string mto, string afrom, string ato)
        {
            var _meetings = db.Meetings.ToList();
            var loginuserid = User.Identity.GetUserId();
            var _meetingstatus = db.MeetingsStatus.Where(x => x.UserId == loginuserid).ToList();
			var _meetinginterest = db.MeetingInterests.Where(x => x.UserId == loginuserid).ToList();
            if (program != null && program > 0)
            {
                _meetings = _meetings.Where(x => x.Program == program).ToList();
            }
            if (sem != null && sem > 0)
            {
                _meetings = _meetings.Where(x => x.Semester == sem).ToList();
            }
            
            if (!string.IsNullOrEmpty(mtitle))
            {
                _meetings = _meetings.Where(x => x.MeetingTitle.ToLower().Contains(mtitle.ToLower())).ToList();
            }
            if (!string.IsNullOrEmpty(desc))
            {
                _meetings = _meetings.Where(x => x.Description.ToLower().Contains(desc.ToLower())).ToList();
            }
            if (!string.IsNullOrEmpty(sub))
            {
                _meetings = _meetings.Where(x => x.Subject.ToLower().Contains(sub.ToLower())).ToList();
            }
            if (!string.IsNullOrEmpty(loc))
            {
                _meetings = _meetings.Where(x => x.Location.ToLower().Contains(loc.ToLower())).ToList();
            }
            if (!string.IsNullOrEmpty(add))
            {
                _meetings = _meetings.Where(x => x.AddedBy.ToLower().Contains(add.ToLower())).ToList();
            }

            if (!string.IsNullOrEmpty(mfrom))
            {
                DateTime date = DateTime.ParseExact(mfrom, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                _meetings = _meetings.Where(x => x.MeetingDateTime >= date).ToList();
            }
            if (!string.IsNullOrEmpty(mto))
            {
                DateTime date = DateTime.ParseExact(mto, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                _meetings = _meetings.Where(x => x.MeetingDateTime <= date).ToList();
            }
            if (!string.IsNullOrEmpty(afrom))
            {
                DateTime date = DateTime.ParseExact(afrom, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                _meetings = _meetings.Where(x => x.AddedOn >= date).ToList();
            }
            if (!string.IsNullOrEmpty(ato))
            {
                DateTime date = DateTime.ParseExact(ato, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                _meetings = _meetings.Where(x => x.AddedOn <= date).ToList();
            }
            var programs = db.Programs.ToList();
            var usersettings = db.UserSettings.ToList();
            
            var vm = _meetings.Select(x => new MeetingViewModel
            {
                AddedBy = x.AddedBy,
                AddedOn = x.AddedOn,
                Description = x.Description,
                Id = x.Id,
                IsAccepted = (bool)(_meetingstatus.Where(t => t.MeetingId == x.Id).Count() > 0? _meetingstatus.Where(t => t.MeetingId == x.Id).FirstOrDefault().IsAccepted: false),
				ShownInterest = _meetinginterest.Where(t => t.MeetingId == x.Id).Count() > 0 ? true : false,
                InterestedUsers = x.MeetingInterests.Count(),
                Location = x.Location,
                MeetingDateTime = x.MeetingDateTime,
                MeetingTitle = x.MeetingTitle,
                ProgramName = programs.Where(t => t.Id == x.Program).FirstOrDefault() == null? "": programs.Where(t=>t.Id == x.Program).FirstOrDefault().ProgramName,
                IsEmail = usersettings.Where(t => t.UserId == x.UserId).FirstOrDefault()== null? true :usersettings.Where(t => t.UserId == x.UserId).FirstOrDefault().IsEmail,
                IsSms = usersettings.Where(t => t.UserId == x.UserId).FirstOrDefault() == null ? false : usersettings.Where(t => t.UserId == x.UserId).FirstOrDefault().IsSms,
                Semester = x.Semester,
                Subject = x.Subject,
                UpdatedBy = x.UpdatedBy,
                UpdatedOn = x.UpdatedOn,
            }).OrderByDescending(x => x.Id).ToList();
            ViewBag.program = programs.Select(x => new SelectListItem { Text = x.ProgramName, Value = x.Id.ToString(), Selected = x.Id == program }).ToList();
            

            ViewBag.mtitle = mtitle;
            ViewBag.sub = sub;
            ViewBag.desc = desc;
            ViewBag.loc = loc;
          //  ViewBag.program = program;
            ViewBag.sem = sem;
            ViewBag.add = add;
            ViewBag.pdfrom = mfrom;
            ViewBag.pdto = mto;
            ViewBag.ufrom = afrom;
            ViewBag.uto = ato;
            return View(vm);
        }

        [Authorize(Roles = "Student,Admin")]
        // GET: Meetings/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Meeting meeting = db.Meetings.Find(id);
            if (meeting == null)
            {
                return HttpNotFound();
            }
            return View(meeting);
        }

        [Authorize(Roles = "Student")]
        // GET: Meetings/Create
        public ActionResult Create()
        {
            var pro = db.Programs.Select(x => new SelectListItem { Text = x.ProgramName, Value = x.Id.ToString() }).ToList();
            ViewBag.programList = pro;
            return View();
        }

        [Authorize(Roles = "Student")]
        // POST: Meetings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,MeetingTitle,Description,Subject,MeetingDateTime,Location,Program,Semester,AddedBy,AddedOn,UpdatedBy,UpdatedOn,UserId")] Meeting meeting)
        {
            if (ModelState.IsValid)
            {
                meeting.UserId = User.Identity.GetUserId();
                //Console.WriteLine(meeting.UserId);
                meeting.AddedBy = User.Identity.Name;
                meeting.AddedOn = DateTime.Now;
                db.Meetings.Add(meeting);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            var pro = db.Programs.Select(x => new SelectListItem { Text = x.ProgramName, Value = x.Id.ToString() }).ToList();
            ViewBag.programList = pro;
            return View(meeting);
        }

        [Authorize(Roles = "Student")]
        // GET: Meetings/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Meeting meeting = db.Meetings.Find(id);
            if (meeting == null)
            {
                return HttpNotFound();
            }
            var pro = db.Programs.Select(x => new SelectListItem { Text = x.ProgramName, Value = x.Id.ToString() }).ToList();
            ViewBag.programList = pro;
            return View(meeting);
        }

        [Authorize(Roles = "Student")]
        public async Task<ActionResult> Accept(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Meeting meeting = db.Meetings.Find(id);
            if (meeting == null)
            {
                return HttpNotFound();
            }

            var UserId = User.Identity.GetUserId();
            MeetingsStatu sts = db.MeetingsStatus.Where(x => x.MeetingId == id && x.UserId == UserId).FirstOrDefault();
            if (sts == null)
            {
                sts = new MeetingsStatu();
            }
            var body = "";

            sts.UserId = UserId;
            sts.MeetingId = id;
            sts.IsAccepted = sts.IsAccepted == null || sts.IsAccepted == false ? true : false;
            var loginUser = GetUserInfoByUserId(UserId);
            if (sts.IsAccepted == true)
            {
                sts.AcceptedOn = DateTime.Now;
                body = meeting.MeetingTitle + " meeting is accepted by " + loginUser.FullName;
            }
            else
            {
                sts.RejectedOn = DateTime.Now;
                body = meeting.MeetingTitle + " meeting is rejected by " + loginUser.FullName;
            }
            if (sts.Id > 0)
            {
                db.Entry(sts).State = EntityState.Modified;
            }
            else
            {
                db.MeetingsStatus.Add(sts);
            }
            db.SaveChanges();

            var preference = GetUserNotificationPreference("meeting",meeting.UserId);
            var userModel = GetUserInfoByUserId(sts.Meeting.UserId);


            if (preference.IsSms)
            {
                SendSmsViaClickaTell("+16475372344", body);
            }

            if (preference.IsEmail)
            {
                var recipients = new List<EmailAddress>();
                recipients.Add(new EmailAddress(userModel.Email, userModel.FullName));
                SendEmailViaSendGrid("Meeting "+ (sts.IsAccepted == true ? "Accepted" : "Rejected"), body, recipients);
            }


            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Student")]
        public ActionResult ShowInterest(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Meeting meeting = db.Meetings.Find(id);
            if (meeting == null)
            {
                return HttpNotFound();
            }
            var userid = User.Identity.GetUserId();
            MeetingInterest sts = db.MeetingInterests.Where(x => x.MeetingId == id && x.UserId == userid).FirstOrDefault();
            if (sts == null)
            {
                sts = new MeetingInterest();
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
                sts.MeetingId = id;
                sts.AddedOn = DateTime.Now;
                body = loginUser.FullName  + " showed interest in meeting:" + meeting.MeetingTitle;
                db.MeetingInterests.Add(sts);
            }
            db.SaveChanges();
            var preference = GetUserNotificationPreference("meeting",meeting.UserId);
            var userModel = GetUserInfoByUserId(sts.Meeting.UserId);
            if (preference.IsEmail)
            {
                var recipients = new List<EmailAddress>();
                recipients.Add(new EmailAddress("ankit110989@gmail.com", userModel.FullName));
                SendEmailViaSendGrid("Interest showed in meeting", body, recipients);
            }

            if (preference.IsSms)
            {
                SendSmsViaClickaTell(userModel.PhoneNumber, body);
            }
            return RedirectToAction("Index");
        }


        // POST: Meetings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Student")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,MeetingTitle,Description,Subject,MeetingDateTime,Location,Program,Semester,AddedBy,AddedOn,UpdatedBy,UpdatedOn")] Meeting meeting)
        {
            if (ModelState.IsValid)
            {
                meeting.AddedBy = User.Identity.Name;
                meeting.UpdatedBy = User.Identity.Name;
                meeting.UpdatedOn = DateTime.Now;
                db.Entry(meeting).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            var pro = db.Programs.Select(x => new SelectListItem { Text = x.ProgramName, Value = x.Id.ToString() }).ToList();
            ViewBag.programList = pro;
            return View(meeting);
        }

        [Authorize(Roles = "Student,Admin")]
        // GET: Meetings/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Meeting meeting = db.Meetings.Find(id);
            if (meeting == null)
            {
                return HttpNotFound();
            }
            return View(meeting);
        }

        // POST: Meetings/Delete/5
        [Authorize(Roles = "Student,Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Meeting meeting = db.Meetings.Find(id);
            db.Meetings.Remove(meeting);
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
