using System;
using MimeKit;
using MimeKit.Text;
using MailKit.Net.Smtp;
using MailKit.Security;
using Infrastructure.Configurations;
using System.Threading.Tasks;
using Application.Response;
using Application.Interfaces.Services;

namespace Infrastructure.Services
{
    /// <summary>
    /// Mail service implementation
    /// </summary>
    public class MailService : IMailService
    {
        // Mail configurations
        private MailConfigurations _config;

        public MailService(MailConfigurations mailconfig)
        {
            _config = mailconfig;
        }

        /// <summary>
        /// Sends an email to the specified recipient.
        /// </summary>
        /// <param name="to">The email address of the recipient.</param>
        /// <param name="subject">The subject of the email.</param>
        /// <param name="content">The body content of the email.</param>
        /// <returns>
        /// A <see cref="Result"/> representing the result of the email sending operation.
        /// </returns>
        public async Task<Result> SendMailAsync(string to, string subject, string content)
        {
            // returns result
            return await SetupMailAsync(to, subject, content);
        }

        /// <summary>
        /// Sets up the email with the specified recipient, subject, and content, and prepares it for sending.
        /// </summary>
        /// <param name="to">The email address of the recipient.</param>
        /// <param name="subject">The subject of the email.</param>
        /// <param name="content">The body content of the email.</param>
        /// <returns>
        /// A <see cref="Result"/> containing the result of the email setup process.
        /// </returns>
        private async Task<Result> SetupMailAsync(string to, string subject, string content)
        {
            using (var smtp = new SmtpClient())
            {
                try
                {
                    // use mailkit for send mail
                    var mail     = new MimeMessage();
                    mail.From.Add(MailboxAddress.Parse(_config.From));
                    mail.To.Add(MailboxAddress.Parse(to));
                    mail.Subject = subject;
                    mail.Body    = new TextPart(TextFormat.Html) { Text = content };

                    await smtp.ConnectAsync(_config.Server, _config.Port, SecureSocketOptions.StartTls);
                    await smtp.AuthenticateAsync(_config.UserName, _config.Password);
                    await smtp.SendAsync(mail);

                    return new Result
                    {
                        Message = "Success.",
                        Success = true
                    };

                }
                catch (Exception ex)
                {
                    return new Result
                    {
                        Message = ex.Message,
                        Success = false
                    };
                }
                finally
                {
                    await smtp.DisconnectAsync(true);
                }
            }
        }
    }
}
