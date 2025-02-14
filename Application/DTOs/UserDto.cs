using System;

namespace Application.DTOs
{
    /// <summary>
    /// DTO for user info.
    /// </summary>
    public class UserDto
    {
        /// <summary>
        ///Primary key
        /// </summary>
        public int UserId              { get; set; }

        /// <summary>
        /// Username for the user
        /// </summary>
        public string UserName         { get; set; }

        /// <summary>
        /// First name of the user
        /// </summary>
        public string FirstName        { get; set; }

        /// <summary>
        /// Email address of the user
        /// </summary>
        public string UserMail         { get; set; }

        /// <summary>
        /// Hashed password of the user
        /// </summary>
        public byte[] PasswordHash     { get; set; }

        /// <summary>
        /// Salt used for hashing the user's password
        /// </summary>
        public byte[] PasswordSalt     { get; set; }

        /// <summary>
        /// User's given name
        /// </summary>
        public string GivenName        { get; set; }

        /// <summary>
        /// User's family name
        /// </summary>
        public string FamilyName       { get; set; }

        /// <summary>
        /// URL or path to the user's profile picture
        /// </summary>
        public string Picture          { get; set; }

        /// <summary>
        /// Indicates whether the user's email has been verified
        /// </summary>
        public bool EmailVerified      { get; set; }

        /// <summary>
        /// User's locale or language preference
        /// </summary>
        public string Locale           { get; set; }

        /// <summary>
        /// Token used for verifying the user's account
        /// </summary>
        public string VerifyToken      { get; set; }

        /// <summary>
        /// The expiration date and time of the verification token
        /// </summary>
        public DateTime ExpiresAt      { get; set; }

        /// <summary>
        /// Token used to refresh the user's session.
        /// </summary>
        public string RefreshToken     { get; set; }

        /// <summary>
        /// The expiration date and time of the refresh token
        /// </summary>
        public DateTime RefreshExpires { get; set; }

        /// <summary>
        /// Token used for resetting the user's password (optional)
        /// </summary>
        public string? ResetToken      { get; set; }

        /// <summary>
        /// The expiration date and time of the password reset token (optional)
        /// </summary>
        public DateTime? ResetExpires  { get; set; }

        /// <summary>
        /// Indicates whether the user has administrative privileges
        /// </summary>
        public bool IsAdmin            { get; set; }
    }
}
