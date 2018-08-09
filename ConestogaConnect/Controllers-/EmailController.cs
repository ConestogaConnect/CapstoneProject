using ConestogaConnect.Models;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ConestogaConnect.Controllers
{
    public class EmailController : BaseController
    {
        // GET: Email
        public ActionResult Index()
        {
            var users = db.AspNetUsers.ToList();
            EmailViewModel model = new EmailViewModel();
            model.Sender = User.Identity.Name;
            model.emailList = users.Select(x => new SelectListItem { Text = x.FirstName + " " + x.LastName, Value = x.Email }).ToList();
            return View(model);
        }

        [HttpPost]
        public ActionResult Index(EmailViewModel model)
        {
            var users = db.AspNetUsers.ToList();

            if (ModelState.IsValid)
            {
                List<EmailAddress> rec = new List<EmailAddress>();
                foreach (var em in model.Recipients)
                {
                    var user = users.Where(x => x.Email == em).FirstOrDefault();
                    var name = user.FirstName + " " + user.LastName;
                    rec.Add(new EmailAddress(em, name));
                }
                var loginuser = users.Where(x => x.Email == model.Sender).FirstOrDefault();
                var loginname = loginuser.FirstName + " " + loginuser.LastName;
                SendEmailViaSendGrid(model.Subject, model.Body, rec, new EmailAddress(model.Sender, loginname));
                TempData["SuccessMessage"] = "Email sent successfully";
            }
            else
            {
                model.emailList = users.Select(x => new SelectListItem { Text = x.FirstName + " " + x.LastName, Value = x.Email }).ToList();
                return View(model);

            }
            return RedirectToAction("Index");
        }
    }
}