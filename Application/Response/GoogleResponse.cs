namespace Application.Response
{
    /// <summary>
    /// Google AUTH response class.
    /// </summary>
    public class GoogleResponse
    {
        /// <summary>
        /// Unique identifier for the user
        /// </summary>
        public string Sub         { get; set; }

        /// <summary>
        /// Full name of the user
        /// </summary>
        public string Name        { get; set; }

        /// <summary>
        /// First (given) name of the user
        /// </summary>
        public string GivenName   { get; set; }

        /// <summary>
        /// Last (family) name of the user
        /// </summary>
        public string FamilyName  { get; set; }

        /// <summary>
        /// URL of the user's profile picture
        /// </summary>
        public string Picture     { get; set; }

        /// <summary>
        /// Email address of the user
        /// </summary>
        public string Email       { get; set; }

        /// <summary>
        /// Indicates if the email is verified
        /// </summary>
        public bool EmailVerified { get; set; }

        /// <summary>
        /// Language/region preference of the user
        /// </summary>
        public string Locale      { get; set; }

        /// <summary>
        /// Error message, if any
        /// </summary>
        public string Error       { get; set; }

    }
}
