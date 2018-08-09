using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace ConestogaConnect.Models
{
    public class ActivityViewModel
    {

        public int Id { get; set; }
        public string UserId { get; set; }
        public string ActivityName { get; set; }
        public string Description { get; set; }
        public int ActivityTypeId { get; set; }
        public System.DateTime Added_On { get; set; }
        public Nullable<System.DateTime> Updated_On { get; set; }
        public virtual ActivityType ActivityType { get; set; }
        public Nullable<System.DateTime> UpdatedOn { get; set; }
        public bool ShownInterest { get; set; }
        public int InterestedUsers { get; set; }

        public string FirstName;

        public string LastName;


    }
}