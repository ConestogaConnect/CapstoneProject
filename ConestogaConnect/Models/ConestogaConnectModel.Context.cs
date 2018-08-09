﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ConestogaConnect.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class ConestagoEntities : DbContext
    {
        public ConestagoEntities()
            : base("name=ConestagoEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Activity> Activities { get; set; }
        public virtual DbSet<ActivityType> ActivityTypes { get; set; }
        public virtual DbSet<AspNetRole> AspNetRoles { get; set; }
        public virtual DbSet<AspNetUserActivation> AspNetUserActivations { get; set; }
        public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUser> AspNetUsers { get; set; }
        public virtual DbSet<Discussion> Discussions { get; set; }
        public virtual DbSet<DiscussionComment> DiscussionComments { get; set; }
        public virtual DbSet<JobPosting> JobPostings { get; set; }
        public virtual DbSet<Meeting> Meetings { get; set; }
        public virtual DbSet<MeetingsStatu> MeetingsStatus { get; set; }
        public virtual DbSet<Program> Programs { get; set; }
        public virtual DbSet<SubComment> SubComments { get; set; }
        public virtual DbSet<NewsFeed> NewsFeeds { get; set; }
        public virtual DbSet<Tutor> Tutors { get; set; }
        public virtual DbSet<UserSetting> UserSettings { get; set; }
        public virtual DbSet<ActivityInterest> ActivityInterests { get; set; }
        public virtual DbSet<MeetingInterest> MeetingInterests { get; set; }
        public virtual DbSet<Book> Books { get; set; }
        public virtual DbSet<Accomodation> Accomodations { get; set; }
        public virtual DbSet<AccomodationImage> AccomodationImages { get; set; }
        public virtual DbSet<UserUISetting> UserUISettings { get; set; }
    }
}