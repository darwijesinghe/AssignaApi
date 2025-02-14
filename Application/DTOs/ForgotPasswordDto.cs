using System.ComponentModel.DataAnnotations;

namespace Application.DTOs
{
    /// <summary>
    /// DTO for the forgot password.
    /// </summary>
    public class ForgotPasswordDto
    {
        /// <summary>
        /// Email address
        /// </summary>
        [EmailAddress]
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }
    }
}
