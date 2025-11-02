namespace Infrastructure.Configurations
{
    /// <summary>
    /// Frontend client app settings values.
    /// </summary>
    public class ClientApp
    {
        /// <summary>
        /// Client app base URL.
        /// </summary>
        public string BaseUrl          { get; set; }

        /// <summary>
        /// Password reset URL.
        /// </summary>
        public string PasswordResetUrl { get; set; }
    }
}
