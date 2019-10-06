using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using System.Net.Mail;
using ViewModel;
using CaptchaMvc.HtmlHelpers;

namespace CompanyWebsite.Controllers
{
    public class ContactUsController : Controller
    {
        // GET: ContactUs
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SendEmail(ContactUsViewModel contactUsViewModel)
        {
            if(this.IsCaptchaValid("invalid captcha input"))
            {
                SendEmailToAdmin(contactUsViewModel);
                return View("EmailSent");
            }
            else
            {
                ViewBag.ErrorMessage = "* Invalid captca input.";

                return View("Index");
            }
        }

        private void SendEmailToAdmin(ContactUsViewModel contactUsViewModel)
        {
            var toEmail = ConfigurationManager.AppSettings["ContactUsEmailAddress"];
            var emailSubject = ConfigurationManager.AppSettings["ContactUsEmailSubject"];

            var mailMessage = new MailMessage(contactUsViewModel.Email, toEmail);
            mailMessage.Subject = emailSubject;
            mailMessage.Body = contactUsViewModel.Message;

            var smtpServer = ConfigurationManager.AppSettings["SmtpServer"];
            var smtpPort = int.Parse(ConfigurationManager.AppSettings["SmtpPort"]);

            var smtpClient = new SmtpClient(smtpServer, smtpPort);

            smtpClient.Send(mailMessage);
        }
    }
}