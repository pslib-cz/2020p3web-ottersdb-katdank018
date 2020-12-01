using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Identity.Client;
using Microsoft.Graph.Auth;
using Microsoft.Graph;

namespace Database01.Services
{
    public class EmailSender : IEmailSender
    {
        readonly IConfiguration _configuration;
        public EmailSender(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(string email, string subject, string text)
        {
            /* create message header + body */
            var message = new Message
            {
                Subject = subject,
                Body = new ItemBody
                {
                    ContentType = BodyType.Html,
                    Content = text
                },
                ToRecipients = new List<Recipient>()
                {
                    new Recipient { EmailAddress = new EmailAddress { Address = email } }
                }
            };

            /* make MS Graph API connection */
            string[] scopes = new string[] { "https://graph.microsoft.com/.default" };

            IConfidentialClientApplication confidentialClientApplication = ConfidentialClientApplicationBuilder
                .Create(_configuration["EmailSender:ClientId"])
                .WithTenantId(_configuration["EmailSender:TenantId"])
                .WithClientSecret(_configuration["EmailSender:ClientSecret"])
                .Build();

            await confidentialClientApplication.AcquireTokenForClient(scopes)
                .ExecuteAsync()
                .ConfigureAwait(false);

            var authProvider = new ClientCredentialProvider(confidentialClientApplication);
            var graphClient = new GraphServiceClient(authProvider);

            /* send email over Grap API Mail.Send */
            await graphClient.Users[_configuration["EmailSender:UserId"]]
                .SendMail(message, false)
                .Request()
                .PostAsync();
        }
    }
}