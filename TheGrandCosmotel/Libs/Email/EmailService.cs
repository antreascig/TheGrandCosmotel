using SendGrid;
using System.Net;
using System.Configuration;
using System.Diagnostics;
using SendGrid.Helpers.Mail;
using Microsoft.AspNet.Identity;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using WebGames.Models;
using System.Linq;

namespace WebGames.Libs
{
    public class EmailService : IIdentityMessageService
    {
        static string _SENDGRID_KEY = null;
        private static string SENDGRID_KEY
        {
            get
            {
                if (_SENDGRID_KEY == null)
                {
                    var config = SettingsManager.GetConfig();
                    _SENDGRID_KEY = config.security.api.SENDGRID_KEY ?? "";
                }
                return _SENDGRID_KEY;
            }
        }

        public async Task SendAsync(IdentityMessage message)
        {
            await configSendGridasync(message);
        }

        // Use NuGet to install SendGrid (Basic C# client lib) 
        private async Task configSendGridasync(IdentityMessage message)
        {
            try
            {
                var myMessage = new SendGridMessage();
                myMessage.AddTo(new EmailAddress(message.Destination));
                myMessage.From = new EmailAddress("info@cosmoplay.gr", "Auto-Message");
                myMessage.Subject = message.Subject;
                myMessage.PlainTextContent = message.Body;
                myMessage.HtmlContent = message.Body;

                //var credentials = new NetworkCredential(
                //           ConfigurationManager.AppSettings["mailAccount"],
                //           ConfigurationManager.AppSettings["mailPassword"]
                //           );

                var client = new SendGridClient(SENDGRID_KEY);

                // Send the email.
                if (client != null)
                {
                    var response = await client.SendEmailAsync(myMessage);
                }
                else
                {
                    Trace.TraceError("Failed to create Web transport.");
                    await Task.FromResult(0);
                }
            }
            catch(Exception exc)
            {
                Trace.TraceError(exc.Message);
                await Task.FromResult(0);
            }

        }
    }
}