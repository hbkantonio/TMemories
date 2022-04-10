using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Attachment = SendGrid.Helpers.Mail.Attachment;

namespace Transversal.Helpers
{
    public class EmailSenderHelper
    {
        private readonly IConfiguration _configuration;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly string _alias;
        private readonly string _fromMailAddress;
        private readonly bool _useSendGrid;
        private readonly string _apiKeySendGrid;
        private readonly string _hostSmtp;
        private readonly string _passwordSmtp;

        public EmailSenderHelper(IConfiguration configuration, IHostingEnvironment hostingEnvironment)
        {
            _configuration = configuration;
            _hostingEnvironment = hostingEnvironment;
            _alias = _configuration["EmailSender:Alias"];
            _fromMailAddress = _configuration["EmailSender:FromMailAddress"];
            _useSendGrid = bool.Parse(_configuration["EmailSender:UseSendGrid"]);
            _apiKeySendGrid = _configuration["EmailSender:ApiKeySendGrid"];
            _hostSmtp = _configuration["EmailSender:HostSmtp"];
            _passwordSmtp = _configuration["EmailSender:PasswordSmtp"];
        }

        public async Task SendMail(string toMailAddress, string subject, string body)
        {
            if (_useSendGrid)
                await SendMailSendGrid(toMailAddress, subject, body);
            else
                await SendMailSmtp(toMailAddress, subject, body);
        }

        public string GetTemplateBody(string templateName)
        {
            string body;
            string pathTemplates = Path.Combine(_hostingEnvironment.ContentRootPath, "Templates");
            using (StreamReader sr = new StreamReader(Path.Combine(pathTemplates, templateName)))
            {
                body = sr.ReadToEnd();
            }
            return body;
        }

        private async Task SendMailSmtp(string toMailAddress, string subject, string body)
        {
            MailMessage message = new MailMessage();
            SmtpClient smtp = new SmtpClient();
            message.From = new MailAddress(_fromMailAddress);
            message.To.Add(new MailAddress(toMailAddress));
            message.Subject = subject;
            message.IsBodyHtml = true; //to make message body as html  
            message.Body = body;
            smtp.Port = 587;
            smtp.Host = _hostSmtp; //for gmail host  
            smtp.EnableSsl = true;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new NetworkCredential(_fromMailAddress, _passwordSmtp);
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            await smtp.SendMailAsync(message);
        }

        private async Task SendMailSendGrid(string toMailAddress, string subject, string body, List<Attachment> attachments = null, string ccEmailAddress = null)
        {
            SendGridClient client = new SendGridClient(_apiKeySendGrid);

            EmailAddress from = new EmailAddress(_fromMailAddress, _alias);
            EmailAddress to = new EmailAddress(toMailAddress, "");
            string plainTextContent = body;
            string htmlContent = body;
            SendGridMessage msg;
            msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);

            if (attachments != null)
            {
                if (attachments.Count > 0)
                {
                    msg.AddAttachments(attachments);
                }
            }

            //add CC
            if (!string.IsNullOrEmpty(ccEmailAddress))
            {
                EmailAddress cc = new EmailAddress(ccEmailAddress, "");
                msg.AddCc(cc);
            }

            await client.SendEmailAsync(msg);
        }
    }
}
