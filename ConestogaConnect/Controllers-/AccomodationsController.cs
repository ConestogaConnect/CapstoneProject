using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ConestogaConnect.Models;
using Microsoft.AspNet.Identity;
using ConestogaConnect.ViewModels;

namespace ConestogaConnect.Controllers
{
    public class AccomodationsController : BaseController
    {

        // GET: Accomodations
        public ActionResult Index(int? rooms, decimal? rent, string facilities, int? floors, string pdfrom, string pdto, string ufrom, string uto, bool? petfriendly, bool? parking, bool? furnished)
        {
            var obj = db.Accomodations.ToList();

           

            if (rooms != null && rooms > 0)
            {
                obj = obj.Where(x => x.Number_of_Rooms == rooms).ToList();
            }
            if (rent != null && rent > 0)
            {
                obj = obj.Where(x => x.Rent == rent.ToString()).ToList();
            }
            if (floors != null && floors > 0)
            {
                obj = obj.Where(x => x.Floor == floors.ToString()).ToList();
            }
            if (!string.IsNullOrEmpty(facilities))
            {
               obj = obj.Where(x => x.Facilities.ToLower().Contains(facilities.ToLower())).ToList();
            }
            if (petfriendly != null)
            {
                obj = obj.Where(x => x.PetFriendly == petfriendly).ToList();
            }
            if (parking != null)
            {
                obj = obj.Where(x => x.Parking == parking).ToList();
            }
            if (furnished != null)
            {
                obj = obj.Where(x => x.Furnished == furnished).ToList();
            }
            if (!string.IsNullOrEmpty(pdfrom))
            {
                DateTime date = DateTime.ParseExact(pdfrom, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                obj = obj.Where(x => x.Posted_Date >= date).ToList();
            }
            if (!string.IsNullOrEmpty(pdto))
            {
                DateTime date = DateTime.ParseExact(pdto,"dd/MM/yyyy", CultureInfo.InvariantCulture);
                obj = obj.Where(x => x.Posted_Date <= date).ToList();
            }
            if (!string.IsNullOrEmpty(ufrom))
            {
                DateTime date = DateTime.ParseExact(ufrom, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                obj = obj.Where(x => x.Last_Updated >= date).ToList();
            }
            if (!string.IsNullOrEmpty(uto))
            {
                DateTime date = DateTime.ParseExact(uto, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                obj = obj.Where(x => x.Last_Updated <= date).ToList();
            }
            ViewBag.rooms = rooms;
            ViewBag.rent = rent;
            ViewBag.facilities = facilities;
            ViewBag.floors = floors;
            ViewBag.pdfrom = pdfrom;
            ViewBag.pdto = pdto;
            ViewBag.ufrom = ufrom;
            ViewBag.uto = uto;
            ViewBag.petfriendly = petfriendly;
            ViewBag.parking = parking;
            ViewBag.furnished = furnished;

            var vm = obj.Select(x => new AccomodationViewModel
            {
                UserId = x.UserId,
                Posted_Date = x.Posted_Date,
                Number_of_Rooms = x.Number_of_Rooms,
                Rent = x.Rent,
                PetFriendly = x.PetFriendly,
                Parking = x.Parking,
                Floor = x.Floor,
                Furnished=x.Furnished,

                //postedImages = db.AccomodationImages.Where(t => t.AccomodationId == x.Id).FirstOrDefault() == null ? "" : db.AccomodationImages.Where(t => t.AccomodationId == x.Id).ToList(),
                
            }).OrderByDescending(x => x.Id).ToList();
            //ViewBag.postedImages = db.AccomodationImages.Select(x => new SelectListItem { Text = x.ImageId.ToString(), Value = x.ImageTitle }).ToList();

            return View(obj.ToList());
            //return View(viewModel);
        }

        //Display Accomodations-Admin
        [Authorize(Roles = "Admin")]
        public ActionResult DisplayAccomodations(int? rooms, decimal? rent, string facilities, int? floors, string pdfrom, string pdto, string ufrom, string uto, bool? petfriendly, bool? parking, bool? furnished)
        {
            var obj = db.Accomodations.ToList();
            if (rooms != null && rooms > 0)
            {
                obj = obj.Where(x => x.Number_of_Rooms == rooms).ToList();
            }
            if (rent != null && rent > 0)
            {
                obj = obj.Where(x => x.Rent == rent.ToString()).ToList();
            }
            if (floors != null && floors > 0)
            {
                obj = obj.Where(x => x.Floor == floors.ToString()).ToList();
            }
            if (!string.IsNullOrEmpty(facilities))
            {
                obj = obj.Where(x => x.Facilities.ToLower().Contains(facilities.ToLower())).ToList();
            }
            if (petfriendly != null)
            {
                obj = obj.Where(x => x.PetFriendly == petfriendly).ToList();
            }
            if (parking != null)
            {
                obj = obj.Where(x => x.Parking == parking).ToList();
            }
            if (furnished != null)
            {
                obj = obj.Where(x => x.Furnished == furnished).ToList();
            }
            if (!string.IsNullOrEmpty(pdfrom))
            {
                DateTime date = DateTime.ParseExact(pdfrom, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                obj = obj.Where(x => x.Posted_Date >= date).ToList();
            }
            if (!string.IsNullOrEmpty(pdto))
            {
                DateTime date = DateTime.ParseExact(pdto, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                obj = obj.Where(x => x.Posted_Date <= date).ToList();
            }
            if (!string.IsNullOrEmpty(ufrom))
            {
                DateTime date = DateTime.ParseExact(ufrom, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                obj = obj.Where(x => x.Last_Updated >= date).ToList();
            }
            if (!string.IsNullOrEmpty(uto))
            {
                DateTime date = DateTime.ParseExact(uto, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                obj = obj.Where(x => x.Last_Updated <= date).ToList();
            }
            ViewBag.rooms = rooms;
            ViewBag.rent = rent;
            ViewBag.facilities = facilities;
            ViewBag.floors = floors;
            ViewBag.pdfrom = pdto;
            ViewBag.pdto = pdto;
            ViewBag.ufrom = ufrom;
            ViewBag.uto = uto;
            ViewBag.petfriendly = petfriendly;
            ViewBag.parking = parking;
            ViewBag.furnished = furnished;
            return View(obj);
        }

        // GET: Accomodations/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Accomodation accomodation = db.Accomodations.Find(id);
            if (accomodation == null)
            {
                return HttpNotFound();
            }
            return View(accomodation);
        }

        // GET: Accomodations/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Accomodations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(HttpPostedFileBase image,[Bind(Include = "UserId,Number_of_Rooms,Rent,Facilities,PetFriendly,Parking,Floor,Furnished,Posted_Date,Image_Id,Image,ImageIds")] Accomodation accomodation)
        {
            if (ModelState.IsValid)
            {
                accomodation.UserId = User.Identity.GetUserId();
                accomodation.Posted_Date = DateTime.Now;
                db.Accomodations.Add(accomodation);
                db.SaveChanges();
                var accomodationId = accomodation.Id;


                var ImageIds = accomodation.Image_Id;

                string[] array = ImageIds.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                

                // int ImageId=0;
                foreach (string ImageId in array) {
                    int ImageId1 = Int32.Parse(ImageId);
                    AccomodationImage accomodationImages = db.AccomodationImages.Find(ImageId1);

                    db.Entry(accomodationImages).State = EntityState.Modified;
                    accomodationImages.AccomodationId = accomodationId;
                    db.SaveChanges();

                }
                //Console.WriteLine(accomodation.Image_Id);


                return RedirectToAction("Index");
            }

            return View(accomodation);
        }

        // GET: Accomodations/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Accomodation accomodation = db.Accomodations.Find(id);
            if (accomodation == null)
            {
                return HttpNotFound();
            }
            return View(accomodation);
        }

        // POST: Accomodations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,UserId,Number_of_Rooms,Rent,Facilities,PetFriendly,Parking,Floor,Furnished,Posted_Date,Last_Updated,Image_Id")] Accomodation accomodation)
        {
            if (ModelState.IsValid)
            {
                db.Entry(accomodation).State = EntityState.Modified;
                accomodation.Last_Updated = System.DateTime.Now;
                accomodation.UserId = User.Identity.GetUserId();
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(accomodation);
        }

        // GET: Accomodations/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Accomodation accomodation = db.Accomodations.Find(id);
            if (accomodation == null)
            {
                return HttpNotFound();
            }
            return View(accomodation);
        }

        // POST: Accomodations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Accomodation accomodation = db.Accomodations.Find(id);
            db.Accomodations.Remove(accomodation);
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

        public JsonResult ImageUpload(AccomodationImage model)
        {
            // OurDbContext db = new OurDbContext();
            int ImageId = 0;
            var file = model.ImageFile;
            byte[] imagebyte = null;
            if (file != null)
            {
                var parsedFileName = Path.GetFileName(file.FileName);
                file.SaveAs(Server.MapPath("/UploadImage/" + parsedFileName));
                BinaryReader reader = new BinaryReader(file.InputStream);
                imagebyte = reader.ReadBytes(file.ContentLength);
                AccomodationImage img = new AccomodationImage();
                img.ImageTitle = parsedFileName;
                img.ImageByte = imagebyte;
                img.ImagePath = "/UploadImage/" + parsedFileName;

                db.AccomodationImages.Add(img);
                db.SaveChanges();

                ImageId = db.AccomodationImages.OrderByDescending(p => p.ImageId).FirstOrDefault().ImageId;

                //return x.ID;

                //ImageId = db.AccomodationImages.Last().ImageId;
                //ImageId = myNewObject.ImageId;
            }
            return Json(ImageId, JsonRequestBehavior.AllowGet);
        }
        public ActionResult DisplayingImage(int imgid)
        {
            //OurDbContext db = new OurDbContext();
            var img = db.AccomodationImages.SingleOrDefault(x => x.ImageId == imgid);
            return File(img.ImageByte, "image/jpg");
        }



    }
}
