using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using MimeKit;
using MailKit.Net.Smtp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Database01.Services
{
    public class EmailSender : IEmailSender
    {
        public string HtmlMessage { get; set; }
        public IConfiguration Configuration { get; protected set; }
        public EmailSender(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public Task SendEmailAsync(string email, string subject, string text)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(Configuration["EmailSender:FromName"], Configuration["EmailSender:From"]));
            message.To.Add(new MailboxAddress(email, email));
            message.Subject = subject;

            var bodyBuilder = new BodyBuilder();
            if (HtmlMessage != "") bodyBuilder.HtmlBody = HtmlMessage; // pokud máme HTML zprávu, tak ji připojíme
            bodyBuilder.TextBody = text;

            message.Body = bodyBuilder.ToMessageBody();

            Int32.TryParse(Configuration["EmailSender:Port"], out int port);
            using var client = new SmtpClient();
            client.ServerCertificateValidationCallback = (s, c, h, e) => true; // "vždyověření" certifikátu :)
            client.Connect(Configuration["EmailSender:Server"], port, MailKit.Security.SecureSocketOptions.StartTlsWhenAvailable);
            client.Authenticate(Configuration["EmailSender:Username"], Configuration["EmailSender:Password"]);
            client.Send(message);
            client.Disconnect(true);

            return Task.FromResult(0);
        }
    }
}