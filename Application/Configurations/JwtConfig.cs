namespace Application.Configurations
{
    /// <summary>
    /// Represents the configuration settings required for JWT (JSON Web Token) generation and validation.
    /// These values are typically read from a configuration file like appsettings.json.
    /// </summary>
    public class JwtConfig
    {
        /// <summary>
        /// The secret key used to sign the JWT.
        /// This should be a long, unique, and secure string that is used for token encryption and validation.
        /// </summary>
        public string Secret           { get; set; }

        /// <summary>
        /// The issuer of the JWT.
        /// This is typically the server or application that generates the token (e.g. "https://yourapp.com").
        /// </summary>
        public string Issuer           { get; set; }

        /// <summary>
        /// The audience for the JWT.
        /// This represents the intended recipient of the token, usually the application or service consuming the token.
        /// </summary>
        public string Audience         { get; set; }

        /// <summary>
        /// The duration in minutes for which the token will remain valid.
        /// Once the token expires, it can no longer be used and must be refreshed or a new token generated.
        /// </summary>
        public int ExpirationInMinutes { get; set; }
    }
}
