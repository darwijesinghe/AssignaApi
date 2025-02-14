namespace Infrastructure.Configurations
{
    /// <summary>
    /// Mail configuration class.
    /// </summary>
    public class MailConfigurations
    {
        /// <summary>
        /// User name (sender email)
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Password (sender email)
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Sender email address
        /// </summary>
        public string From     { get; set; }

        /// <summary>
        /// Mail client port
        /// </summary>
        public int Port        { get; set; }

        /// <summary>
        /// Mail server. eg. smtp.gmail.com
        /// </summary>
        public string Server   { get; set; }
    }
}
