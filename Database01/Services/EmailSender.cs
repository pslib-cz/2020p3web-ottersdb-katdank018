using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using MimeKit;
using MailKit.Net.Smtp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Util.Store;
using Google.Apis.Auth.OAuth2;
using System.Threading;
using Google.Apis.Util;
using MailKit.Security;

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

        public async Task SendEmailAsync(string email, string subject, string text)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(Configuration["EmailSender:FromName"], Configuration["EmailSender:From"]));
            message.To.Add(new MailboxAddress(email, email));
            message.Subject = subject;

            var bodyBuilder = new BodyBuilder();
            if (HtmlMessage != "") bodyBuilder.HtmlBody = HtmlMessage; // pokud máme HTML zprávu, tak ji připojíme
            bodyBuilder.TextBody = text;
            bodyBuilder.HtmlBody = text;

            message.Body = bodyBuilder.ToMessageBody();

            const string GMailAccount = "katerina.dankova@pslib.cz";

            var clientSecrets = new ClientSecrets
            {
                ClientId = "324607673470-bi32g8ajvpjo2strovk1p4tt8652cpkr.apps.googleusercontent.com",
                ClientSecret = "E2U_ftF02cFYFpJmBURXxBuL"
            };

            var codeFlow = new GoogleAuthorizationCodeFlow(new GoogleAuthorizationCodeFlow.Initializer
            {
                DataStore = new FileDataStore("CredentialCacheFolder", false),
                Scopes = new[] { "https://mail.google.com/" },
                ClientSecrets = clientSecrets
            });

            // Note: For a web app, you'll want to use AuthorizationCodeWebApp instead.
            var codeReceiver = new LocalServerCodeReceiver();
            var authCode = new AuthorizationCodeInstalledApp(codeFlow, codeReceiver);

            var credential = await authCode.AuthorizeAsync(GMailAccount, CancellationToken.None);

            if (credential.Token.IsExpired(SystemClock.Default))
                await credential.RefreshTokenAsync(CancellationToken.None);

            var oauth2 = new SaslMechanismOAuth2(credential.UserId, credential.Token.AccessToken);


            using (var client = new SmtpClient())
            {
                await client.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.SslOnConnect);
                await client.AuthenticateAsync(oauth2);
                client.Send(message);
                await client.DisconnectAsync(true);
            }
        }
    }
}