using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace AssignaApi.Models
{
    // data table
    public class Users
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int user_id { get; set; }
        [MaxLength(50)]
        public string user_name { get; set; } = null!;
        [MaxLength(50)]
        public string first_name { get; set; } = null!;
        [MaxLength(50)]
        public string user_mail { get; set; } = null!;
        public byte[] password_hash { get; set; } = new byte[32];
        public byte[] password_salt { get; set; } = new byte[32];
        public string verify_token { get; set; } = string.Empty;
        public string refresh_token { get; set; } = string.Empty;
        public DateTime refresh_expires { get; set; }
        public string? reset_token { get; set; }
        public DateTime? reset_expires { get; set; }
        public bool is_admin { get; set; }
        public DateTime insertdate { get; set; }

        // relationships
        [JsonIgnore]
        public List<Task>? task { get; set; }
    }

    // data transferring model -> DTO
    public class UsersDto
    {
        public int user_id { get; set; }
        public string user_name { get; set; } = null!;
        public string first_name { get; set; } = null!;
        public string user_mail { get; set; } = null!;
        public byte[] password_hash { get; set; } = null!;
        public byte[] password_salt { get; set; } = null!;
        public string verify_token { get; set; } = null!;
        public string refresh_token { get; set; } = null!;
        public DateTime refresh_expires { get; set; }
        public string? reset_token { get; set; }
        public DateTime? reset_expires { get; set; }
        public bool is_admin { get; set; }
    }

    // user register
    public class UserRegister
    {
        [MaxLength(50, ErrorMessage = "Max length exceeded")]
        [Required(ErrorMessage = "Username is required")]
        public string user_name { get; set; } = null!;
        [MaxLength(50, ErrorMessage = "Max length exceeded")]
        [Required(ErrorMessage = "First name is required")]
        public string first_name { get; set; } = null!;
        [Required(ErrorMessage = "Email address is required"),
        EmailAddress]
        public string email { get; set; } = null!;
        [RegularExpression(@"^(?=.*\d)(?=.*[#$^+=!*()@%&]).{5,}$",
        ErrorMessage = "Passwords must contain at least five characters, including at least 1 letter and 1 number")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Password is required")]
        public string password { get; set; } = null!;
        [Required(ErrorMessage = "User role required")]
        public string role { get; set; } = null!;
    }

    // user login
    public class UserLogin
    {
        [Required(ErrorMessage = "Username is required")]
        public string user_name { get; set; } = null!;
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Password is required")]
        public string password { get; set; } = null!;
    }

    // forgot password
    public class ForgotPassword
    {
        [EmailAddress]
        [Required(ErrorMessage = "Email is required")]
        public string email { get; set; } = null!;
    }

    // reset password
    public class ResetPassword
    {
        [RegularExpression(@"^(?=.*\d)(?=.*[#$^+=!*()@%&]).{8,}$")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Password is required")]
        public string password { get; set; } = null!;
        [Required(ErrorMessage = "Confirm password is required"),
        Compare("password", ErrorMessage = "Confirm password does not match")]
        public string con_password { get; set; } = null!;
        [Required(ErrorMessage = "Password reset token required")]
        public string reset_token { get; set; } = null!;
    }

    // refresh token
    public class RefreshToken
    {
        [Required(ErrorMessage = "Refresh token is required")]
        public string refresh_token { get; set; } = null!;
    }
}