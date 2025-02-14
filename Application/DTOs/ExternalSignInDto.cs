using System.ComponentModel.DataAnnotations;

namespace Application.DTOs
{
    /// <summary>
    /// External login DTO class.
    /// </summary>
    public class ExternalSignInDto
    {
        /// <summary>
        /// Provider name for the authentication service
        /// </summary>
        public string Provider    { get; set; }

        /// <summary>
        /// Access token required for authentication
        /// </summary>
        [Required(ErrorMessage = "Access token is required")]
        public string AccessToken { get; set; }

        /// <summary>
        /// Type of the token (e.g. Bearer)
        /// </summary>
        public string TokenType   { get; set; }

        /// <summary>
        /// Expiration time of the access token in seconds
        /// </summary>
        public int ExpiresIn      { get; set; }

        /// <summary>
        /// Scope of the access token, defining the permissions granted
        /// </summary>
        public string Scope       { get; set; }

        /// <summary>
        /// Username of the authenticated user
        /// </summary>
        public string AuthUser    { get; set; }

        /// <summary>
        /// Account type of the user / user role
        /// </summary>
        [Required(ErrorMessage = "Account type is required")]
        public string Role        { get; set; }

    }
}
