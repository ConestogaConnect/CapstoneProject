using ConestogaConnect.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using System.Data.Entity;

namespace ConestogaConnect.Controllers
{
    [Authorize]
    public class UserController : BaseController
    {
        // GET: User
        public ActionResult UserSettings()
        {
            var userid = User.Identity.GetUserId();
            List<UserSetting> settings = db.UserSettings.Where(x => x.UserId == userid).ToList();
            if (settings.Count == 0)
            {
                settings = new List<UserSetting>();
                settings.Add(new UserSetting { UserId = userid, Action = "Comment" });
                settings.Add(new UserSetting { UserId = userid, Action = "Meeting" });
                settings.Add(new UserSetting { UserId = userid, Action = "Discussion" });
            }
            return View(settings);
        }

        [HttpPost]
        public ActionResult UserSettings(List<UserSetting> settings)
        {
            try
            {
                foreach (var setting in settings)
                {
                    if (setting.Id > 0)
                    {
                        db.Entry(setting).State = EntityState.Modified;
                    }
                    else
                    {
                        db.Entry(setting).State = EntityState.Added;
                    }
                }
                db.SaveChanges();
                return Json("");
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }

        }

    }
}