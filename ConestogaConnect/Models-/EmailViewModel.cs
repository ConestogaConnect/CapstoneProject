using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ConestogaConnect.Models
{
    public class EmailViewModel
    {
        [Required]
        public string Subject { get; set; }
        [Required]
        public string Sender { get; set; }
        [Required]
        public List<string> Recipients { get; set; }
        [Required]
        public string Body { get; set; }

        public List<SelectListItem> emailList { get; set; }


    }

    public class PreferenceViewModel
    {
        public bool IsSms { get; set; }
        public bool IsEmail { get; set; }
    }

    public class UserViewModel {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
    }
}