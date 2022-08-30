using ElSaiys.Settings;
using ElSaiys.Services.Interfaces;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ElSaiys.Services
{
    public class MailingSrvice : IMailingSrvice
    {
        private readonly MailSettings _mailRequest;

        public MailingSrvice(IOptions<MailSettings> mailRequest)
        {
            _mailRequest = mailRequest.Value;
        }

        public async Task SendEmailAsync(string mailTo, string subject, string body, IList<IFormFile> attachments = null)
        {
            var email = new MimeMessage
            {
                Sender = MailboxAddress.Parse(_mailRequest.Email),
                Subject = subject
            };

            email.To.Add(MailboxAddress.Parse(mailTo));

            var builder = new BodyBuilder();

            if (attachments != null)
            {
                byte[] fileBytes;
                
                foreach (var file in attachments)
                {
                    if (file.Length > 0)
                    {
                        using var ms = new MemoryStream();
                        file.CopyTo(ms);
                        fileBytes = ms.ToArray();

                        builder.Attachments.Add(file.FileName, fileBytes, ContentType.Parse(file.ContentType));
                    }
                }
            }

            builder.HtmlBody = body;
            email.Body = builder.ToMessageBody();
            email.From.Add(new MailboxAddress(_mailRequest.DisplayName, _mailRequest.Email));

            using var smtp = new SmtpClient();
            smtp.Connect(_mailRequest.Host, _mailRequest.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(_mailRequest.Email, _mailRequest.Password);

            await smtp.SendAsync(email);

            smtp.Disconnect(true);
        }
    }
}