using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MailKit.Net.Smtp;
using MailKit;
using MimeKit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;


namespace Upmarket2.Controllers
{
    public class CalendarController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public class HomeController : Controller
        {
            private EmailAddress FromAndToEmailAddress;
            private IEmailService EmailService;
            public HomeController(EmailAddress _fromAddress,
                IEmailService _emailService)
            {
                FromAndToEmailAddress = _fromAddress;
                EmailService = _emailService;
            }

            [HttpGet]
            public ViewResult Index()
            {
                return View();
            }

            [HttpPost]
            public IActionResult Index(EmailMessage model)
            {
                if (ModelState.IsValid)
                {
                    EmailMessage msgToSend = new EmailMessage
                    {
                        FromAddresses = new List<EmailAddress> { FromAndToEmailAddress },
                        ToAddresses = new List<EmailAddress> { FromAndToEmailAddress },

                    };

                    EmailService.Send(msgToSend);
                    ViewBag.Message = "Thanks! Your message has been sent.";
                    return View();
                }
                else
                {
                    ViewBag.Message = "Thanks! Your message has been sent.";
                    return View();
                }

            }
        }
    }
}



    