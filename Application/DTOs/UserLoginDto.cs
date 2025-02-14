using System.ComponentModel.DataAnnotations;

namespace Application.DTOs
{
    /// <summary>
    /// DTO for the user login.
    /// </summary>
    public class UserLoginDto
    {
        /// <summary>
        /// User name
        /// </summary>
        [Required(ErrorMessage = "Username is required")]
        public string UserName { get; set; }

        /// <summary>
        /// Password
        /// </summary>
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
    }
}
