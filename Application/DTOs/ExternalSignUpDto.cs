namespace Application.DTOs
{
    /// <summary>
    /// DTO for external signup
    /// </summary>
    public class ExternalSignUpDto
    {
        /// <summary>
        /// Given name of the user (first name)
        /// </summary>
        public string GivenName   { get; set; }

        /// <summary>
        /// Family name of the user (last name)
        /// </summary>
        public string FamilyName  { get; set; }

        /// <summary>
        /// The URL of the user's profile picture
        /// </summary>
        public string Picture     { get; set; }

        /// <summary>
        /// A value indicating whether the user's email is verified
        /// </summary>
        public bool EmailVerified { get; set; }

        /// <summary>
        /// The locale of the user, indicating the user's language and region
        /// </summary>
        public string Locale      { get; set; }

        /// <summary>
        /// Email address of the user
        /// </summary>
        public string Email       { get; set; }

        /// <summary>
        /// Role of the user within the system
        /// </summary>
        public string Role        { get; set; }
    }
}
