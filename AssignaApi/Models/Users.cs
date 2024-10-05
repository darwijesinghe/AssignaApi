using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace AssignaApi.Models
{
    // data table
#pragma warning disable IDE1006 // Naming Styles
    public class Users
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int user_id { get; set; }
        [MaxLength(50)]
        public string user_name { get; set; }
        [MaxLength(50)]
        public string first_name { get; set; }
        [MaxLength(50)]
        public string user_mail { get; set; }
        public byte[]? password_hash { get; set; }
        public byte[]? password_salt { get; set; }
        [MaxLength(50)]
        public string? given_name { get; set; }
        [MaxLength(50)]
        public string? family_name { get; set; }
        public string? picture { get; set; }
        public bool email_verified { get; set; }
        public string? locale { get; set; }
        public string verify_token { get; set; }
        public DateTime expires_at { get; set; }
        public string refresh_token { get; set; }
        public DateTime refresh_expires { get; set; }
        public string? reset_token { get; set; }
        public DateTime? reset_expires { get; set; }
        public bool is_admin { get; set; }
        public DateTime insertdate { get; set; }

        // relationships
        [JsonIgnore]
        public List<Task>? task { get; set; }

    }
#pragma warning restore IDE1006 // Naming Styles

    // data transferring model -> DTO
    public class UsersDto
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string UserMail { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public string GivenName { get; set; }
        public string FamilyName { get; set; }
        public string Picture { get; set; }
        public bool EmailVerified { get; set; }
        public string Locale { get; set; }
        public string VerifyToken { get; set; }
        public DateTime ExpiresAt { get; set; }
        public string RefreshToken { get; set; }
        public DateTime RefreshExpires { get; set; }
        public string? ResetToken { get; set; }
        public DateTime? ResetExpires { get; set; }
        public bool IsAdmin { get; set; }
    }

    // user register
    public class UserRegister
    {
        [MaxLength(50, ErrorMessage = "Max length exceeded")]
        [Required(ErrorMessage = "User Name is required")]
        public string UserName { get; set; }
        [MaxLength(50, ErrorMessage = "Max length exceeded")]
        [Required(ErrorMessage = "First name is required")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Email address is required"),
        EmailAddress]
        public string Email { get; set; }
        [RegularExpression(@"^(?=.*\d)(?=.*[#$^+=!*()@%&]).{5,}$",
        ErrorMessage = "Passwords must contain at least five characters, including at least 1 letter and 1 number")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
        [Required(ErrorMessage = "User role required")]
        public string Role { get; set; }
    }

    // user login
    public class UserLogin
    {
        [Required(ErrorMessage = "Username is required")]
        public string UserName { get; set; }
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
    }

    // forgot password
    public class ForgotPassword
    {
        [EmailAddress]
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }
    }

    // reset password
    public class ResetPassword
    {
        [RegularExpression(@"^(?=.*\d)(?=.*[#$^+=!*()@%&]).{8,}$")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Confirm password is required"),
        Compare("password", ErrorMessage = "Confirm password does not match")]
        public string ConPassword { get; set; }
        [Required(ErrorMessage = "Password reset token required")]
        public string ResetToken { get; set; }
    }

    // refresh token
    public class RefreshToken
    {
        [Required(ErrorMessage = "Refresh token is required")]
        public string TokenRefresh { get; set; }
    }

    // external user signin
    public class ExternalSignIn
    {
        public string Provider { get; set; }
        [Required(ErrorMessage = "Access token is required")]
        public string AccessToken { get; set; }
        public string TokenType { get; set; }
        public int ExpiresIn { get; set; }
        public string Scope { get; set; }
        public string AuthUser { get; set; }
        [Required(ErrorMessage = "Account type is required")]
        public string Role { get; set; }
    }

    // external user signup
    public class ExternalSignUp
    {
        public string GivenName { get; set; }
        public string FamilyName { get; set; }
        public string Picture { get; set; }
        public bool EmailVerified { get; set; }
        public string Locale { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
    }
}