using ConestogaConnect.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ConestogaConnect.Controllers
{
    //[System.Web.Mvc.Authorize(Roles = "Student")]
    public class HomeController : BaseController
    {
      

        [Authorize(Roles = "Student,Admin")]
        public ActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "Student")]
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        [Authorize(Roles = "Student")]
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Dashboard()
        {
            ViewBag.Message = "Admin page.";

            return View();
        }

        [Authorize(Roles = "Student")]
        public ActionResult Search(string keyword)
        {
            SearchViewModel searchViewModel = new SearchViewModel();
            
            if (!string.IsNullOrEmpty(keyword))
            {
                string src = keyword.ToLower();
                var meets = db.Meetings.Where(x=>
                 (!string.IsNullOrEmpty(x.Description) && x.Description.ToLower().Contains(src))
                || (!string.IsNullOrEmpty(x.Location) && x.Location.ToLower().Contains(src))
                || (!string.IsNullOrEmpty(x.MeetingTitle) && x.MeetingTitle.ToLower().Contains(src))
                || (x.Program !=null  && x.Program1.ProgramName.ToLower().Contains(src))
                || (!string.IsNullOrEmpty(x.Subject) && x.Subject.ToLower().Contains(src))
                || (x.Semester != null && x.Semester.ToString().ToLower().Contains(src))).ToList();

                var activities = db.Activities.Where(x =>
                (!string.IsNullOrEmpty(x.ActivityName) && x.ActivityName.ToLower().Contains(src))
                || (x.ActivityType != null && x.ActivityType.ActivityType1.ToLower().Contains(src))
                || (!string.IsNullOrEmpty(x.Description) && x.Description.ToLower().Contains(src))).ToList();

                var accomodation = db.Accomodations.Where(x =>
                (!string.IsNullOrEmpty(x.Facilities) && x.Facilities.ToLower().Contains(src))
                || (!string.IsNullOrEmpty(x.Floor) && x.Floor.ToLower().Contains(src))
                || (!string.IsNullOrEmpty(x.Rent) && x.Rent.ToLower().Contains(src))
                || (x.Number_of_Rooms != null && x.Number_of_Rooms.ToString().ToLower().Contains(src))).ToList();

                var jobs = db.JobPostings.Where(x =>
                (!string.IsNullOrEmpty(x.Experience) && x.Experience.ToLower().Contains(src))
                || (!string.IsNullOrEmpty(x.JobDescription) && x.JobDescription.ToLower().Contains(src))
                || (!string.IsNullOrEmpty(x.JobPostingNumber) && x.JobPostingNumber.ToLower().Contains(src))
                || (!string.IsNullOrEmpty(x.JobSubTitle) && x.JobSubTitle.ToLower().Contains(src))
                || (!string.IsNullOrEmpty(x.JobTitle) && x.JobTitle.ToLower().Contains(src))
                || (!string.IsNullOrEmpty(x.JobType) && x.JobType.ToLower().Contains(src))
                || (!string.IsNullOrEmpty(x.Location) && x.Location.ToLower().Contains(src))
                || (x.Salary != null && x.Salary.ToString().ToLower().Contains(src))).ToList();
                var dis = db.Discussions.Where(x => (!string.IsNullOrEmpty(x.Topic) && x.Topic.ToLower().Contains(src))).ToList();
                var discom = db.DiscussionComments.Where(x => (!string.IsNullOrEmpty(x.CommentMessage) && x.CommentMessage.ToLower().Contains(src))).ToList();
                var subcom = db.SubComments.Where(x => (!string.IsNullOrEmpty(x.CommentMessage) && x.CommentMessage.ToLower().Contains(src))).ToList();

                var tutors = db.Tutors.Where(x=>
                (!string.IsNullOrEmpty(x.Tutor_Name) && x.Tutor_Name.ToLower().Contains(src))
                || (!string.IsNullOrEmpty(x.Description) && x.Description.ToLower().Contains(src))
                || (!string.IsNullOrEmpty(x.Email) && x.Email.ToLower().Contains(src))
                || (!string.IsNullOrEmpty(x.Availability) && x.Availability.ToLower().Contains(src))
                || (!string.IsNullOrEmpty(x.Course) && x.Course.ToLower().Contains(src))
                || (!string.IsNullOrEmpty(x.Phone) && x.Phone.ToLower().Contains(src))
                || (x.Program !=null && x.Program1.ProgramName.ToLower().Contains(src))
                ).ToList();

                var books = db.Books.Where(x=> 
                (!string.IsNullOrEmpty(x.AuthorName) && x.AuthorName.ToLower().Contains(src))
                || (!string.IsNullOrEmpty(x.BookDescription) && x.BookDescription.ToLower().Contains(src))
                || (!string.IsNullOrEmpty(x.BookPath) && x.BookPath.ToLower().Contains(src))
                || (!string.IsNullOrEmpty(x.BookTitle) && x.BookTitle.ToLower().Contains(src))
                || (x.BookPrice != null && x.BookPrice.ToString().ToLower().Contains(src))
                ).ToList();


                ViewBag.search = src;
                searchViewModel.meetings = meets;
                searchViewModel.accomodations = accomodation;
                searchViewModel.activities = activities;
                searchViewModel.discussionComments = discom;
                searchViewModel.discussions = dis;
                searchViewModel.jobPostings = jobs;
                searchViewModel.subComments = subcom;
                searchViewModel.tutors = tutors;
                searchViewModel.books = books;
            }
            if (searchViewModel.meetings == null)
            {
                searchViewModel.meetings = new List<Meeting>();
            }
            if (searchViewModel.accomodations == null)
            {
                searchViewModel.accomodations = new List<Accomodation>();
            }
            if (searchViewModel.activities == null)
            {
                searchViewModel.activities = new List<Activity>();
            }
            if (searchViewModel.discussionComments == null)
            {
                searchViewModel.discussionComments = new List<DiscussionComment>();
            }
            if (searchViewModel.discussions == null)
            {
                searchViewModel.discussions = new List<Discussion>();
            }
            if (searchViewModel.jobPostings == null)
            {
                searchViewModel.jobPostings = new List<JobPosting>();
            }
            if (searchViewModel.subComments == null)
            {
                searchViewModel.subComments = new List<SubComment>();
            }
            if (searchViewModel.tutors == null)
            {
                searchViewModel.tutors = new List<Tutor>();
            }
            if (searchViewModel.books == null)
            {
                searchViewModel.books = new List<Book>();
            }
            return View(searchViewModel);
        }
    }
}