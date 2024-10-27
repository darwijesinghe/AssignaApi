using Application.General;
using Application.Response;

namespace Application.Interfaces.Services
{
    /// <summary>
    /// Interface defining the contract for JWT-related services.
    /// </summary>
    public interface IJwtService
    {
        /// <summary>
        /// Generates a JWT token based on the user's name, email, and role.
        /// </summary>
        /// <param name="name">The user's name.</param>
        /// <param name="mail">The user's email address.</param>
        /// <param name="role">The user's role within the system.</param>
        /// <returns>
        /// Returns a JWT token as a string.
        /// </returns>
        string GenerateJwtToken(string name, string mail, string role);

        /// <summary>
        /// Generates a random token of the specified length.
        /// </summary>
        /// <param name="length">The desired length of the random token.</param>
        /// <returns>
        /// Returns a randomly generated token as a string.
        /// </returns>
        string GenerateRandomToken(int length);

        /// <summary>
        /// Creates both a JWT token and a refresh token based on the provided data.
        /// </summary>
        /// <param name="data">The data required to generate the tokens.</param>
        /// <returns>
        /// Returns a tuple containing the JWT token, refresh token, and the expiration time.
        /// </returns>
        (string token, string refreshToken, int expireAt) MakeTokens(MakeToken data);

        /// <summary>
        /// Reads the JWT token from the current context and extracts user details.
        /// </summary>
        /// <returns>
        /// Returns an <see cref="AuthResponse"/> containing user authentication details.
        /// </returns>
        AuthResponse ReadJwtToken();
    }
}
