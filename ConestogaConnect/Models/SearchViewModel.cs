using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ConestogaConnect.Models
{
    public class SearchViewModel
    {
        public List<Meeting> meetings { get; set; }
        public List<Accomodation> accomodations { get; set; }
        public List<Activity> activities { get; set; }
        public List<JobPosting> jobPostings { get; set; }
        public List<Discussion> discussions { get; set; }
        public List<DiscussionComment> discussionComments { get; set; }
        public List<SubComment> subComments { get; set; }
        public List<Tutor> tutors { get; set; }
        public List<Book> books { get; set; }
    }

    
}