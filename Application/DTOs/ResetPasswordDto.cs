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
        [RegularExpression(@"^(?=.*\d)(?=.*[#$^+=!*()@%&]).{8,}$")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Password is required")]
        public string Password    { get; set; }

        /// <summary>
        /// Confirm password
        /// </summary>
        [Required(ErrorMessage = "Confirm password is required"),
        Compare("password", ErrorMessage = "Confirm password does not match")]
        public string ConPassword { get; set; }

        /// <summary>
        /// Password reset token
        /// </summary>
        [Required(ErrorMessage = "Password reset token required")]
        public string ResetToken  { get; set; }
    }
}
