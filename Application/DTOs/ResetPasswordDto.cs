using System.ComponentModel.DataAnnotations;

namespace Application.DTOs
{
    /// <summary>
    /// DTO for the password reset.
    /// </summary>
    public class ResetPasswordDto
    {
        /// <summary>
        /// Password
        /// </summary>
        [RegularExpression(@"^(?=.*\d)(?=.*[#$^+=!*()@%&]).{5,}$",
        ErrorMessage = "Passwords must contain at least five characters, including at least 1 letter and 1 number")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Password is required")]
        public string Password    { get; set; }

        /// <summary>
        /// Confirm password
        /// </summary>
        [Required(ErrorMessage = "Confirm password is required"),
        Compare("Password", ErrorMessage = "Confirm password does not match")]
        public string ConPassword { get; set; }

        /// <summary>
        /// Password reset token
        /// </summary>
        [Required(ErrorMessage = "Password reset token required")]
        public string ResetToken  { get; set; }
    }
}
