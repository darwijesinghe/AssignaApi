namespace Application.Response
{
    /// <summary>
    /// AUTH response class.
    /// </summary>
    public class AuthResponse
    {
        /// <summary>
        /// User name
        /// </summary>
        public string UserName     { get; set; }

        /// <summary>
        /// User role
        /// </summary>
        public string Role         { get; set; }

        /// <summary>
        /// Indicates operation is done or failed
        /// </summary>
        public bool Success        { get; set; }

        /// <summary>
        /// Response message
        /// </summary>
        public string Message      { get; set; }

        /// <summary>
        /// Auth token
        /// </summary>
        public string Token        { get; set; }

        /// <summary>
        /// Autth refresh token
        /// </summary>
        public string RefreshToken { get; set; }

        /// <summary>
        /// Password reset token
        /// </summary>
        public string ResetToken   { get; set; }
    }
}
