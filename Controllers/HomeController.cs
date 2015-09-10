﻿using System.Web.Mvc;
using Data.Validations;
using FluentValidation;
using Data.Entities;
using Data.Interfaces;
using Web.ViewModels;
using Microsoft.AspNet.Identity;
using StructureMap;
using Core.Services;
using System.Net.Mail;
using Utilities;
using Utilities.Logging;
using System;

namespace Web.Controllers
{

    public class HomeController : Controller
    {   
        private readonly EmailAddress _emailAddress;
        private readonly Email _email;

        public HomeController()
        {
            _emailAddress = ObjectFactory.GetInstance<EmailAddress>();
            _email = ObjectFactory.GetInstance<Email>();
        }

        public ActionResult Index(string emailAddress)
        {
            using (var uow = ObjectFactory.GetInstance<IUnitOfWork>())
            {
                DockyardAccountDO dockyardAccountDO;
                if (!String.IsNullOrEmpty(emailAddress))
                {
                    var emailAddressDO = uow.EmailAddressRepository.GetOrCreateEmailAddress(emailAddress);
                    dockyardAccountDO = uow.UserRepository.GetOrCreateUser(emailAddressDO);
                    
                    //Save incase we created..
                    uow.SaveChanges();
                }
                else
                {
                    var userID = System.Web.HttpContext.Current.User.Identity.GetUserId();
                    dockyardAccountDO = uow.UserRepository.GetByKey(userID);
                }

                var returnVM = new HomeVM {SegmentWriteKey = new ConfigRepository().Get("SegmentWriteKey")};

                if (dockyardAccountDO != null)
                {
                    if (String.IsNullOrEmpty(dockyardAccountDO.FirstName))
                        returnVM.UserName = dockyardAccountDO.LastName;
                    else if (!String.IsNullOrEmpty(dockyardAccountDO.LastName))
                        returnVM.UserName = dockyardAccountDO.FirstName + " " + dockyardAccountDO.LastName;
                    else
                        returnVM.UserName = dockyardAccountDO.FirstName;

                    returnVM.UserID = dockyardAccountDO.Id;
                    returnVM.UserEmail = dockyardAccountDO.EmailAddress.Address;
                }

                return View(returnVM);
            }
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }


        //Validate emailAddress and meetingInfo then call Generate() parameterized method in BookingRequest controller
        [HttpPost]
        public ActionResult ProcessHomePageBookingRequest(string emailAddress, string meetingInfo)
        {
           
      
                RegexUtilities.ValidateEmailAddress(emailAddress);
                if (meetingInfo.Trim().Length < 30)
                    return Json(new { Message = "Meeting information must have at least 30 characters" });

                return RedirectToAction("CreateViaHomePage", "BookingRequest", new { emailAddress = emailAddress, meetingInfo = meetingInfo });
            
           
        }


        
        //  EmailAddress  is valid then send mail .    
        // return "success" or  error 
        public ActionResult ProcessSubmittedEmail(string name, string emailId, string message)
        {
            string result = "";
            try
            {
                EmailAddressDO emailAddressDO = new EmailAddressDO(emailId);

                RegexUtilities.ValidateEmailAddress(emailAddressDO.Address);
                using (IUnitOfWork uow = ObjectFactory.GetInstance<IUnitOfWork>())
                {
                    _emailAddress.ConvertFromMailAddress(uow, new MailAddress(emailId, name));
                    string toRecipient ="info@kwasant.com";
                    string fromAddress =emailId;
                  
                   // EmailDO emailDO = email.GenerateBasicMessage(emailAddressDO, message);
                    string subject = "Customer query";
                    EmailDO emailDO = _email.GenerateBasicMessage(uow, subject, message, fromAddress, toRecipient);
                    //uow.EnvelopeRepository.ConfigurePlainEmail(emailDO);
                    uow.SaveChanges();
                }
                result = "success";
            }
            catch (ValidationException ex)
            {
                result = "You need to provide a valid Email Address.";
            }
            catch (System.Exception ex)
            {
                result = "Something went wrong with our effort to send this message. Sorry! Please try emailing your message directly to info@kwasant.com";
                Logger.GetLogger().Error("Error processing a home page email form submission.", ex);
            }
            return Content(result);
        }
        public ActionResult UserEmails()
        {
            return View();
        }
    }
}