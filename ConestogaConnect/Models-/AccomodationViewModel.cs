using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace ConestogaConnect.Models
{
    public class AccomodationViewModel
    {
        public IEnumerable<Accomodation> Accomodation { get; set; }
        public IEnumerable<AccomodationImage> AccomodationImage { get; set; }

        public int Id { get; set; }
        public string UserId { get; set; }
        public Nullable<int> Number_of_Rooms { get; set; }
        public string Rent { get; set; }
        public string Facilities { get; set; }
        public Nullable<bool> PetFriendly { get; set; }
        public Nullable<bool> Parking { get; set; }
        public string Floor { get; set; }
        public Nullable<bool> Furnished { get; set; }
        public Nullable<System.DateTime> Posted_Date { get; set; }
        public Nullable<System.DateTime> Last_Updated { get; set; }
        public string Image_Id { get; set; }

        //public List postedImages;

        //public virtual AccomodationImage AccomodationImage { get; set; }
    }
}