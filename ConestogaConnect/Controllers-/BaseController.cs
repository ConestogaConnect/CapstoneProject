﻿using ConestogaConnect.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using System.Net.Mail;
using System.Net;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using System.Threading.Tasks;
using SendGrid;
using System.Configuration;
using SendGrid.Helpers.Mail;
using System.IO;
using RestSharp;
using System.Xml.Linq;
using System.Xml;

namespace ConestogaConnect.Controllers
{
    public class BaseController : Controller
    {
        protected ConestagoEntities db;

        public BaseController()
        {
            db = new ConestagoEntities();
        }

        public UserViewModel GetUserInfoByUserId(string userId)
        {
            UserViewModel user = new UserViewModel();
            var model = db.AspNetUsers.Where(x => x.Id == userId).FirstOrDefault();
            if (user != null)
            {
                user.FullName = model.FirstName + " " + model.LastName;
                user.Email = model.Email;
                user.PhoneNumber = model.PhoneNumber;
                return user;
            }
            return null;
        }


        public PreferenceViewModel GetUserNotificationPreference(string action,string userID)
        {
            PreferenceViewModel model = new PreferenceViewModel();
            model.IsEmail = false;
            model.IsSms = false;
            var userid = User.Identity.GetUserId();
           
            UserSetting settings = db.UserSettings.Where(x => x.UserId == userID && x.Action.ToLower() == action.ToLower()).FirstOrDefault();
            if (settings != null)
            {
                model.IsSms = settings.IsSms;
                model.IsEmail = settings.IsEmail;
            }
            return model;
        }

        // Twilio Account info
        //  Url : https://www.twilio.com
        // phonenumber : +17723205098
        //public void sendMessageOverPhone(string phonenumder, string body)
        //{
        //    try
        //    {
        //        const string accountSid = "AC3f58449067ed6773be4a4cee93d7cfed";
        //        const string authToken = "e3e015e9d2586a2b6cc792318cfec746";

        //        TwilioClient.Init(accountSid, authToken);

        //        var message = MessageResource.Create(
        //            body: body,
        //            from: new Twilio.Types.PhoneNumber("+16475372344"), // from twilio account
        //            to: new Twilio.Types.PhoneNumber(phonenumder)
        //        );
        //    }
        //    catch (Exception ex)
        //    {
                
        //    }
        //}

        public string SendEmailViaSendGrid(string subject, string body, List<EmailAddress> recipients, EmailAddress sender = null)
        {
            try
            {
                var apiKey = ConfigurationManager.AppSettings["SendGridAPIKey"];
                var client = new SendGridClient(apiKey);
                body = body.Replace(Environment.NewLine, "<br />");
                if (sender == null)
                {
                    sender = new EmailAddress();
                    sender.Email = "admin@conestago.com";
                    sender.Name = "Admin";

                    string bodyGreeting = "Hello " + (!string.IsNullOrEmpty(recipients[0].Name) ? recipients[0].Name : "") + "<br /><br />";
                    string bodyEnding = "<br /><br />Thanks<br />Contestago Support";
                    body = bodyGreeting + body + bodyEnding;
                }
                var msg = MailHelper.CreateSingleEmailToMultipleRecipients(sender, recipients, subject, "", body);
                List<Task> tasks = new List<Task>();
                tasks.Add(Task.Factory.StartNew(() => {
                    client.SendEmailAsync(msg);
                }));
                return "success";
            }
            catch (Exception ex) { }
            return "failure";
        }

        public string SendSmsViaClickaTell(string phoneNumber, string body)
        {
            try
            {
                if (!string.IsNullOrEmpty(phoneNumber) && !string.IsNullOrEmpty(body))
                {
                    var client = new RestClient("https://platform.clickatell.com");
                    // client.Authenticator = new HttpBasicAuthenticator(username, password);

                    var request = new RestRequest("messages/http/send", Method.GET);
                    request.AddParameter("apiKey", "ny4ZO3J5TN6d0sanwYd05A=="); // adds to POST or URL querystring based on Method
                    request.AddParameter("to", phoneNumber); //+16475372344
                    request.AddParameter("content", body);
                    // execute the request
                    IRestResponse response = client.Execute(request);
                    var content = response.Content;
                    if (IsStringXML(content))
                    {
                        XmlDocument xml = new XmlDocument();
                        xml.LoadXml(content);
                        XmlNode node = xml.SelectSingleNode("SendMessagesResponse/ErrorDescription");
                        return node.InnerText;
                    }
                    return "success";
                }
                return "Invalid PhoneNumber/Content";
            }
            catch (Exception ex)
            {
                return "failure";
            }

            
        }

        private bool IsStringXML(string str)
        {
            if (!string.IsNullOrEmpty(str) && str.TrimStart().StartsWith("<"))
            {
                return true;
            }
            return false;
        }

        public async Task<string> SendEmail(string subject, string body, List<EmailAddress> recipients, EmailAddress sender)
        {
            try
            {
                var apiKey = ConfigurationManager.AppSettings["SendGridAPIKey"];
                var client = new SendGridClient(apiKey);
                body = body.Replace(Environment.NewLine, "<br />");
                //   var from = new EmailAddress("test@example.com", "Example User");
                //   var to = new EmailAddress("test@example.com", "Example User");
                //var plainTextContent = "and easy to do anywhere, even with C#";
                //var htmlContent = "<strong>and easy to do anywhere, even with C#</strong>";
                var msg = MailHelper.CreateSingleEmailToMultipleRecipients(sender, recipients, subject, "", body);
                var response = await client.SendEmailAsync(msg);
                return "success";
            }
            catch (Exception ex) { }
            return "failure";
        }
    }
}