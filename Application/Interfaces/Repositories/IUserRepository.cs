using Application.Response;
using Domain.Classes;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Interfaces.Repositories
{
    /// <summary>
    /// Interface for user related db operations.
    /// </summary>
    public interface IUserRepository
    {
        /// <summary>
        /// Registers a new user.
        /// </summary>
        /// <param name="data">The user registration data.</param>
        /// <returns>
        /// An <see cref="AuthResponse"/> result.
        /// </returns>
        Task<AuthResponse> UserRegisterAsync(User data);

        /// <summary>
        /// Retrieves a collection of all users
        /// </summary>
        /// <returns>
        /// A collection of <see cref="User"/> representing all users
        /// </returns>
        Task<IEnumerable<User>> AllUsers();

        /// <summary>
        /// Stores the forgot password token for a user.
        /// </summary>
        /// <param name="data">The data containing the forgot password information.</param>
        /// <returns>
        /// An <see cref="AuthResponse"/> result.
        /// </returns>
        Task<AuthResponse> ForgotTokenAsync(User data);

        /// <summary>
        /// Resets the user's password.
        /// </summary>
        /// <param name="data">The data containing the reset password information.</param>
        /// <returns>
        /// A <see cref="Result"/> indicating the outcome of the password reset.
        /// </returns>
        Task<Result> ResetPasswordAsync(User data);

        /// <summary>
        /// Resets the verification token and refresh token.
        /// </summary>
        /// <param name="data">The data containing the refresh token information.</param>
        /// <returns>
        /// An <see cref="AuthResponse"/> result.
        /// </returns>
        Task<AuthResponse> ResetVerifyTokenAsync(User data);

        /// <summary>
        /// Retrieves Google user information based on the provided access token.
        /// </summary>
        /// <param name="accessToken">The access token obtained from Google.</param>
        /// <returns>
        /// A <see cref="GoogleResponse"/> containing the user information.
        /// </returns>
        Task<GoogleResponse> GoogleUserInfomation(string accessToken);

        /// <summary>
        /// Performs an external sign-in for a user based on their email address.
        /// </summary>
        /// <param name="email">The email of the user attempting to sign in.</param>
        /// <returns>
        /// An <see cref="AuthResponse"/> representing the outcome of the external sign-in operation.
        /// </returns>
        AuthResponse ExternalSignIn(string email);

        /// <summary>
        /// Performs an external sign-up for a user.
        /// </summary>
        /// <param name="data">The data containing the external sign-up information.</param>
        /// <returns>
        /// An <see cref="AuthResponse"/> result.
        /// </returns>
        Task<AuthResponse> ExternalSignUp(User data);
    }
}
