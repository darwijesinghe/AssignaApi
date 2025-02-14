using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Domain.Classes
{
    /// <summary>
    /// Domain class of user information.
    /// </summary>
    public class User
    {

        /// <summary>
        /// Primary key
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId              { get; set; }

        /// <summary>
        /// Username for the user
        /// </summary>
        [MaxLength(50)]
        public string UserName         { get; set; }

        /// <summary>
        /// First name of the user
        /// </summary>
        [MaxLength(50)]
        public string FirstName        { get; set; }

        /// <summary>
        /// Email address of the user
        /// </summary>
        [MaxLength(50)]
        public string UserMail         { get; set; }

        /// <summary>
        /// Hashed password of the user
        /// </summary>
        public byte[]? PasswordHash    { get; set; }

        /// <summary>
        /// Salt used for hashing the user's password
        /// </summary>
        public byte[]? PasswordSalt    { get; set; }

        /// <summary>
        /// Given name of the user (optional)
        /// </summary>
        [MaxLength(50)]
        public string? GivenName       { get; set; }

        /// <summary>
        /// Family name of the user (optional)
        /// </summary>
        [MaxLength(50)]
        public string? FamilyName      { get; set; }

        /// <summary>
        /// URL or path to the user's profile picture (optional)
        /// </summary>
        public string? Picture         { get; set; }

        /// <summary>
        /// Indicates whether the user's email has been verified
        /// </summary>
        public bool EmailVerified      { get; set; }

        /// <summary>
        /// User's locale or language preference (optional)
        /// </summary>
        public string? Locale          { get; set; }

        /// <summary>
        /// Token used for verifying the user's account
        /// </summary>
        public string VerifyToken      { get; set; }

        /// <summary>
        /// The expiration time of the verification token
        /// </summary>
        public DateTime ExpiresAt      { get; set; }

        /// <summary>
        /// Token used for refreshing the user's session
        /// </summary>
        public string RefreshToken     { get; set; }

        /// <summary>
        /// The expiration time of the refresh token.
        /// </summary>
        public DateTime RefreshExpires { get; set; }

        /// <summary>
        /// Token used for resetting the user's password (optional)
        /// </summary>
        public string? ResetToken      { get; set; }

        /// <summary>
        /// The expiration time of the reset token (optional)
        /// </summary>
        public DateTime? ResetExpires  { get; set; }

        /// <summary>
        /// Indicates whether the user has admin privileges
        /// </summary>
        public bool IsAdmin            { get; set; }

        /// <summary>
        /// The date and time when the user was created
        /// </summary>
        public DateTime InsertDate     { get; set; }

        // Relationship ----------------------------
        [JsonIgnore]
        public ICollection<Task> Task  { get; set; }
    }
}