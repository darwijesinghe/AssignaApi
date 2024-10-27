using Application.Response;
using System.Threading.Tasks;

namespace Application.Interfaces.Services
{
    /// <summary>
    /// Interface for mail service
    /// </summary>
    public interface IMailService
    {
        /// <summary>
        /// Sends an email to the specified recipient.
        /// </summary>
        /// <param name="to">The email address of the recipient.</param>
        /// <param name="subject">The subject of the email.</param>
        /// <param name="content">The body content of the email.</param>
        /// <returns>
        /// A <see cref="Result"/> representing the result of the email sending operation.
        /// </returns>
        Task<Result> SendMailAsync(string to, string subject, string content);
    }
}
