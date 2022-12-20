using System;
using System.Threading.Tasks;
using MimeKit;
using MimeKit.Text;
using MailKit.Net.Smtp;
using MailKit.Security;
using AssignaApi.Response;

namespace AssignaApi.Services
{
    public interface IMailService
    {
        Task<Result> SendMailAsync(string to, string subject, string content);
    }

    // configuration properties
    public class MailConfigurations
    {
        public string userName { get; set; } = string.Empty;
        public string password { get; set; } = string.Empty;
        public string from { get; set; } = string.Empty;
        public int port { get; set; }
        public string server { get; set; } = string.Empty;
    }

    public class MailService : IMailService
    {
        public MailConfigurations _config { get; }
        public MailService(MailConfigurations mailconfig)
        {
            _config = mailconfig;
        }

        // mail send method
        public async Task<Result> SendMailAsync(string to, string subject, string content)
        {
            return await SetupMailAsync(to, subject, content);
        }

        // create mail massage
        private async Task<Result> SetupMailAsync(string to, string subject, string content)
        {
            using (var smtp = new SmtpClient())
            {
                try
                {
                    // use mailkit for send mail
                    var mail = new MimeMessage();
                    mail.From.Add(MailboxAddress.Parse(_config.from));
                    mail.To.Add(MailboxAddress.Parse(to));
                    mail.Subject = subject;
                    mail.Body = new TextPart(TextFormat.Html) { Text = content };

                    await smtp.ConnectAsync(_config.server, _config.port, SecureSocketOptions.StartTls);
                    await smtp.AuthenticateAsync(_config.userName, _config.password);
                    await smtp.SendAsync(mail);

                    return new Result
                    {
                        message = "Ok",
                        success = true
                    };

                }
                catch (Exception ex)
                {
                    return new Result
                    {
                        message = ex.Message,
                        success = false
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