﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ConestogaConnect.Models;
using Microsoft.AspNet.Identity;

namespace ConestogaConnect.Controllers
{
    [Authorize(Roles = "Student,Admin")]
    public class TutorsController : BaseController
    {
        //private TutorEntities db = new TutorEntities();

        // GET: Tutors- Admin Page
        public ActionResult Index(string tname,string crs, string avl, string desc, int? program, string email, string phn, string afrom, string ato, string ufrom, string uto)
        {
            SendSmsViaClickaTell("+919888114646","");
            var _objlist = db.Tutors.ToList();
            if (program != null && program > 0)
            {
                _objlist = _objlist.Where(x => x.Program == program).ToList();
            }
            if (!string.IsNullOrEmpty(tname))
            {
                _objlist = _objlist.Where(x => x.Tutor_Name.ToLower().Contains(tname.ToLower())).ToList();
            }

            if (!string.IsNullOrEmpty(crs))
            {
                _objlist = _objlist.Where(x => x.Course.ToLower().Contains(crs.ToLower())).ToList();
            }
            if (!string.IsNullOrEmpty(desc))
            {
                _objlist = _objlist.Where(x => x.Description.ToLower().Contains(desc.ToLower())).ToList();
            }
            if (!string.IsNullOrEmpty(email))
            {
                _objlist = _objlist.Where(x => x.Email.ToLower().Contains(email.ToLower())).ToList();
            }
            if (!string.IsNullOrEmpty(phn))
            {
                _objlist = _objlist.Where(x => x.Phone.ToLower().Contains(phn.ToLower())).ToList();
            }

            if (!string.IsNullOrEmpty(afrom))
            {
                DateTime date = DateTime.ParseExact(afrom, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                _objlist = _objlist.Where(x => x.AddedOn >= date).ToList();
            }
            if (!string.IsNullOrEmpty(ato))
            {
                DateTime date = DateTime.ParseExact(ato, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                _objlist = _objlist.Where(x => x.AddedOn <= date).ToList();
            }
            if (!string.IsNullOrEmpty(ufrom))
            {
                DateTime date = DateTime.ParseExact(ufrom, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                _objlist = _objlist.Where(x => x.UpdatedOn >= date).ToList();
            }
            if (!string.IsNullOrEmpty(uto))
            {
                DateTime date = DateTime.ParseExact(uto, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                _objlist = _objlist.Where(x => x.UpdatedOn <= date).ToList();
            }
            var programs = db.Programs.ToList();
            ViewBag.program = programs.Select(x => new SelectListItem { Text = x.ProgramName, Value = x.Id.ToString(), Selected = x.Id == program }).ToList();
            return View(_objlist);
        }

        // GET: Tutors
        public ActionResult DisplayTutor(string tname, string crs, string avl, string desc, int? program, string email, string phn, string afrom, string ato, string ufrom, string uto)
        {
            var _objlist = db.Tutors.OrderByDescending(x => x.Id).ToList();
            if (program != null && program > 0)
            {
                _objlist = _objlist.Where(x => x.Program == program).ToList();
            }
            if (!string.IsNullOrEmpty(tname))
            {
                _objlist = _objlist.Where(x => x.Tutor_Name.ToLower().Contains(tname.ToLower())).ToList();
            }

            if (!string.IsNullOrEmpty(crs))
            {
                _objlist = _objlist.Where(x => x.Course.ToLower().Contains(crs.ToLower())).ToList();
            }
            if (!string.IsNullOrEmpty(desc))
            {
                _objlist = _objlist.Where(x => x.Description.ToLower().Contains(desc.ToLower())).ToList();
            }
            if (!string.IsNullOrEmpty(email))
            {
                _objlist = _objlist.Where(x => x.Email.ToLower().Contains(email.ToLower())).ToList();
            }
            if (!string.IsNullOrEmpty(phn))
            {
                _objlist = _objlist.Where(x => x.Phone.ToLower().Contains(phn.ToLower())).ToList();
            }

            if (!string.IsNullOrEmpty(afrom))
            {
                DateTime date = DateTime.ParseExact(afrom, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                _objlist = _objlist.Where(x => x.AddedOn >= date).ToList();
            }
            if (!string.IsNullOrEmpty(ato))
            {
                DateTime date = DateTime.ParseExact(ato, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                _objlist = _objlist.Where(x => x.AddedOn <= date).ToList();
            }
            if (!string.IsNullOrEmpty(ufrom))
            {
                DateTime date = DateTime.ParseExact(ufrom, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                _objlist = _objlist.Where(x => x.UpdatedOn >= date).ToList();
            }
            if (!string.IsNullOrEmpty(uto))
            {
                DateTime date = DateTime.ParseExact(uto, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                _objlist = _objlist.Where(x => x.UpdatedOn <= date).ToList();
            }
            var programs = db.Programs.ToList();
            ViewBag.program = programs.Select(x => new SelectListItem { Text = x.ProgramName, Value = x.Id.ToString(), Selected = x.Id == program }).ToList();
            return View(_objlist);
        }

        // GET: Tutors/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tutor tutor = db.Tutors.Find(id);
            if (tutor == null)
            {
                return HttpNotFound();
            }
            return View(tutor);
        }

        // GET: Tutors/Create
        public ActionResult Create()
        {
            var pro = db.Programs.Select(x => new SelectListItem { Text = x.ProgramName, Value = x.Id.ToString() }).ToList();
            ViewBag.programList = pro;
            return View();
        }

        // POST: Tutors/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Tutor_Name,Program,Course,Availability,Description,AddedOn,UpdatedOn,Email,Phone,UserId")] Tutor tutor)
        {
            if (ModelState.IsValid)
            {
                tutor.UserId = User.Identity.GetUserId();
                tutor.AddedOn = DateTime.Now;
                db.Tutors.Add(tutor);
                db.SaveChanges();

                return RedirectToAction("DisplayTutor");
            }

            var pro = db.Programs.Select(x => new SelectListItem { Text = x.ProgramName, Value = x.Id.ToString() }).ToList();
            ViewBag.programList = pro;

            return View(tutor);
        }

        // GET: Tutors/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tutor tutor = db.Tutors.Find(id);
            if (tutor == null)
            {
                return HttpNotFound();
            }

            var pro = db.Programs.Select(x => new SelectListItem { Text = x.ProgramName, Value = x.Id.ToString() }).ToList();
            ViewBag.programList = pro;

            return View(tutor);
        }

        // POST: Tutors/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Tutor tutor)
        {
            if (ModelState.IsValid)
            {
                tutor.UpdatedOn = DateTime.Now;
                db.Entry(tutor).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            var pro = db.Programs.Select(x => new SelectListItem { Text = x.ProgramName, Value = x.Id.ToString() }).ToList();
            ViewBag.programList = pro;

            return View(tutor);
        }

        // GET: Tutors/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tutor tutor = db.Tutors.Find(id);
            if (tutor == null)
            {
                return HttpNotFound();
            }
            return View(tutor);
        }

        // POST: Tutors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Tutor tutor = db.Tutors.Find(id);
            db.Tutors.Remove(tutor);
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
