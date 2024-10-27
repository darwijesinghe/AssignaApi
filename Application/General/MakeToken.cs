namespace Application.General
{
    /// <summary>
    /// Represents the data required to generate a JWT token for a user
    /// Includes the user's basic details and token properties
    /// </summary>
    public class MakeToken
    {
        /// <summary>
        /// The name of the user for whom the token is being generated.
        /// This value can be used to store the user's display name or username.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The email address of the user.
        /// This could be used as a unique identifier within the token claims for authentication or validation.
        /// </summary>
        public string Mail { get; set; }

        /// <summary>
        /// The role assigned to the user, such as "Lead", "Member", or any other role-based access control.
        /// This is useful for setting authorization levels within the token.
        /// </summary>
        public string Role { get; set; }

        /// <summary>
        /// Represents the length or size of the generated JWT token in characters.
        /// This defines how long the generated token string will be.
        /// </summary>
        public int Length  { get; set; }
    }
}
