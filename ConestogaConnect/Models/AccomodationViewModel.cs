using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ConestogaConnect.Models
{
    public class AccomodationViewModel
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        [DisplayName("Number of Rooms")]
        public Nullable<int> Number_of_Rooms { get; set; }
        public string Rent { get; set; }
        public string Facilities { get; set; }
        [DisplayName("Pet Friendly")]
        public Nullable<bool> PetFriendly { get; set; }
        public Nullable<bool> Parking { get; set; }
        public string Floor { get; set; }
        public Nullable<bool> Furnished { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }
        [DisplayName("Posted Date")]
        public Nullable<System.DateTime> Posted_Date { get; set; }
        [DisplayName("Last Updated")]
        public Nullable<System.DateTime> Last_Updated { get; set; }
        public List<AccomodationImage> Images { get; set; }

        [NotMapped]
        public string parkingval { get; set; }
        [NotMapped]
        public string furnishedval { get; set; }
        [NotMapped]
        public string Petfriendlyval { get; set; }

        public virtual AspNetUser AspNetUser { get; set; }

        public string FirstName;

        public string LastName;

    }

   
}