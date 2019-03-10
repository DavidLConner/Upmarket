
using System.Collections.Generic;
using MimeKit;
using System.Security.Claims;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;


namespace Upmarket2.Models
{
    public class User
    {
        private readonly Microsoft.AspNetCore.Http.IHttpContextAccessor _httpContextAccessor;
        public User(Microsoft.AspNetCore.Http.IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public static object Identity { get; internal set; }
        public static InternetAddress UserId { get; internal set; }

        public void FindUser()
        {
            var UserId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
        }
    }
    public class EmailMessage
    {

        public bool Wednesday { get; set; }

        public bool Saturday { get; set; }

        public string TextArea { get; set; }
        public IEnumerable<object> Admin { get; internal set; }
        public IEnumerable<object> User { get; internal set; }
    }
    public class EmailAddress
    {
        public string Name { get; set; }
        public string Address { get; set; }
    }

    public interface IEmailConfiguration
    {
        string SmtpServer { get; }
        int SmtpPort { get; }
        string SmtpUsername { get; set; }
        string SmtpPassword { get; set; }

        string PopServer { get; }
        int PopPort { get; }
        string PopUsername { get; }
        string PopPassword { get; }
    }

    public class EmailConfiguration : IEmailConfiguration
    {
        public string SmtpServer { get; set; }
        public int SmtpPort { get; set; }
        public string SmtpUsername { get; set; }
        public string SmtpPassword { get; set; }

        public string PopServer { get; set; }
        public int PopPort { get; set; }
        public string PopUsername { get; set; }
        public string PopPassword { get; set; }
    }
    public interface IEmailService
    {

        void Send(EmailMessage Message);

    }

    public class MailKitEmailService : IEmailService
    {
        private readonly EmailConfiguration _eConfig;

        public MailKitEmailService(EmailConfiguration config)
        {
            _eConfig = config;
        }

        public void Send(EmailMessage msg)
        {
            var message = new MimeMessage();


            //message.To.AddRange(msg.Admin.Select(x => new MailboxAddress("angstart8@gmail.com")));
            //message.From.AddRange(msg.User.Select(x => User.UserId));

            message.Body = new TextPart("plain")
            {
                Text = msg.TextArea,
            };

            using (var client = new MailKit.Net.Smtp.SmtpClient())
            {
                client.Connect(_eConfig.SmtpServer, _eConfig.SmtpPort);

                client.AuthenticationMechanisms.Remove("XOAUTH2");

                client.Authenticate(_eConfig.SmtpUsername, _eConfig.SmtpPassword);

                client.Send(message);
                client.Disconnect(true);
            }
        }
        public class Startup
        {
            public void ConfigureServices(IServiceCollection services)
            {
                EmailConfiguration config = new EmailConfiguration
                {
                    SmtpPassword = "AnotherPassword19",
                    SmtpServer = "smtp.mailgun.com",
                    SmtpUsername = "angstart8@gmail.com"
                };

                EmailAddress FromEmailAddress = new EmailAddress
                {
                    Address = "angstart8@gmail.com",
                    Name = "David Conner"
                };

                services.AddSingleton<EmailConfiguration>(config);
                services.AddTransient<IEmailService, MailKitEmailService>();
                services.AddSingleton<EmailAddress>(FromEmailAddress);
                services.AddMvc();
            }

            public void Configure(IApplicationBuilder app, IHostingEnvironment env)
            {
                app.UseMvcWithDefaultRoute();
            }
        }
    }
}



