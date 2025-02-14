using System.ComponentModel.DataAnnotations;

namespace Application.DTOs
{
    /// <summary>
    /// DTO for the user registration.
    /// </summary>
    public class UserRegisterDto
    {
        /// <summary>
        /// User name
        /// </summary>
        [MaxLength(50, ErrorMessage = "Max length exceeded")]
        [Required(ErrorMessage = "User Name is required")]
        public string UserName  { get; set; }

        /// <summary>
        /// User first name
        /// </summary>
        [MaxLength(50, ErrorMessage = "Max length exceeded")]
        [Required(ErrorMessage = "First name is required")]
        public string FirstName { get; set; }

        /// <summary>
        /// User's email address
        /// </summary>
        [Required(ErrorMessage = "Email address is required"), EmailAddress]
        public string Email     { get; set; }

        /// <summary>
        /// Password
        /// </summary>
        [RegularExpression(@"^(?=.*\d)(?=.*[#$^+=!*()@%&]).{5,}$",
        ErrorMessage = "Passwords must contain at least five characters, including at least 1 letter and 1 number")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Password is required")]
        public string Password  { get; set; }

        /// <summary>
        /// User role
        /// </summary>
        [Required(ErrorMessage = "User role required")]
        public string Role      { get; set; }
    }
}
