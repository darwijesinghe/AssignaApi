using Application.Configurations;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Application.Response;
using Domain.Classes;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    /// <summary>
    /// Repository implementation for IUserRepository.
    /// </summary>
    public class UserRepository : IUserRepository
    {
        // Services
        private DataContext          _context { get; }
        private readonly HttpClient  _httpClient;
        private readonly IJwtService _jwtService;
        private readonly JwtConfig   _jwtConfig;

        public UserRepository(DataContext context, IJwtService jwtService, IOptions<JwtConfig> jwtConfig, IHttpClientFactory httpClientFactory)
        {
            _context    = context;
            _httpClient = httpClientFactory.CreateClient("GlobalClient");
            _jwtService = jwtService;
            _jwtConfig  = jwtConfig.Value;
        }

        /// <summary>
        /// Holds the HTTP response message
        /// </summary>
        private HttpResponseMessage? _response { get; set; }

        /// <summary>
        /// Registers a new user.
        /// </summary>
        /// <param name="data">The user registration data.</param>
        /// <returns>
        /// An <see cref="AuthResponse"/> result.
        /// </returns>
        public async Task<AuthResponse> UserRegisterAsync(User data)
        {
            try
            {
                // adds data to context
                await _context.User.AddAsync(data);

                // saves changes to the context
                await _context.SaveChangesAsync();

                return new AuthResponse
                {
                    Message = "Ok.",
                    Success = true
                };
            }
            catch (Exception ex)
            {
                return new AuthResponse
                {
                    Message = ex.Message,
                    Success = false
                };
            }
        }

        /// <summary>
        /// Retrieves a collection of all users.
        /// </summary>
        /// <returns>
        /// A collection of <see cref="User"/> representing all users.
        /// </returns>
        public async Task<IEnumerable<User>> AllUsers()
        {
            try
            {
                // returns all users
                return await _context.User.ToListAsync();
            }
            catch (Exception ex)
            {
                return Enumerable.Empty<User>();
            }
        }

        /// <summary>
        /// Stores the forgot password token for a user.
        /// </summary>
        /// <param name="data">The data containing the forgot password information.</param>
        /// <returns>
        /// An <see cref="AuthResponse"/> result.
        /// </returns>
        public async Task<AuthResponse> ForgotTokenAsync(User data)
        {
            try
            {
                // retrieves the user
                var user = _context.User.FirstOrDefault(x => x.UserMail == data.UserMail);
                if (user is null)
                    return new AuthResponse { Message = "User not found.", Success = false };

                user.ResetToken   = data.ResetToken;
                user.ResetExpires = data.ResetExpires;

                // saves changes to the context
                await _context.SaveChangesAsync();

                return new AuthResponse
                {
                    Message    = "Ok.",
                    Success    = true,
                    ResetToken = user.ResetToken
                };
            }
            catch (Exception ex)
            {
                return new AuthResponse
                {
                    Message = ex.Message,
                    Success = false
                };
            }
        }

        /// <summary>
        /// Resets the user's password.
        /// </summary>
        /// <param name="data">The data containing the reset password information.</param>
        /// <returns>
        /// A <see cref="Result"/> indicating the outcome of the password reset.
        /// </returns>
        public async Task<Result> ResetPasswordAsync(User data)
        {
            try
            {
                // retrieves the user
                var user = _context.User.FirstOrDefault(x => x.ResetToken == data.ResetToken);
                if (user is null)
                    return new Result { Message = "User not found.", Success = false };

                user.ResetToken   = null;
                user.ResetExpires = null;
                user.PasswordHash = data.PasswordHash;
                user.PasswordSalt = data.PasswordSalt;

                // saves changes to the context
                await _context.SaveChangesAsync();

                return new Result
                {
                    Message = "Ok.",
                    Success = true
                };
            }
            catch (Exception ex)
            {
                return new Result
                {
                    Message = ex.Message,
                    Success = false
                };
            }
        }

        /// <summary>
        /// Resets the verification token and refresh token.
        /// </summary>
        /// <param name="data">The data containing the refresh token information.</param>
        /// <returns>
        /// An <see cref="AuthResponse"/> result.
        /// </returns>
        public async Task<AuthResponse> ResetVerifyTokenAsync(User data)
        {
            try
            {
                // retrieves the user
                var user = _context.User.FirstOrDefault(x => x.RefreshToken == data.RefreshToken);
                if (user is null)
                    return new AuthResponse { Message = "User not found.", Success = false };

                // generates the token
                string jwtToken = _jwtService.GenerateJwtToken(user.UserName, user.UserMail, (user.IsAdmin ? Role.Lead : Role.Member));

                // new refresh token
                string refreshToken = _jwtService.GenerateRandomToken(100);

                // new verification token data
                user.VerifyToken    = jwtToken;
                user.ExpiresAt      = DateTime.Now.AddMinutes(_jwtConfig.ExpirationInMinutes);
                user.RefreshToken   = refreshToken;
                user.RefreshExpires = DateTime.Now.AddMonths(1);

                // saves changes to the context
                await _context.SaveChangesAsync();

                return new AuthResponse
                {
                    Message      = "Ok.",
                    Success      = true,
                    Token        = user.VerifyToken,
                    RefreshToken = user.RefreshToken
                };
            }
            catch (Exception ex)
            {
                return new AuthResponse
                {
                    Message = ex.Message,
                    Success = false
                };
            }
        }

        /// <summary>
        /// Retrieves Google user information based on the provided access token.
        /// </summary>
        /// <param name="accessToken">The access token obtained from Google.</param>
        /// <returns>
        /// A <see cref="GoogleResponse"/> containing the user information.
        /// </returns>
        public async Task<GoogleResponse> GoogleUserInfomation(string accessToken)
        {
            try
            {
                // access token
                var header = new AuthenticationHeaderValue("Bearer", accessToken);
                _httpClient.DefaultRequestHeaders.Authorization = header;

                // calling to the endpoint
                using (_response = await _httpClient.GetAsync("https://www.googleapis.com/oauth2/v3/userinfo"))
                {
                    // checks the response status
                    if (_response.StatusCode != HttpStatusCode.OK)
                        return new GoogleResponse { Error = _response.ReasonPhrase };

                    // reads the response
                    string result = await _response.Content.ReadAsStringAsync();
                    if (string.IsNullOrEmpty(result)) 
                        return new GoogleResponse { Error = "Response is empty or null." };

                    // deserializing
                    var info = JsonConvert.DeserializeObject<GoogleResponse>(result);

                    // returns the response
                    return new GoogleResponse
                    {
                        Sub           = info.Sub,
                        Name          = info.Name,
                        GivenName     = info.GivenName,
                        FamilyName    = info.FamilyName,
                        Picture       = info.Picture,
                        Email         = info.Email,
                        EmailVerified = info.EmailVerified,
                        Locale        = info.Locale
                    };
                }
            }
            catch (Exception ex)
            {
                return new GoogleResponse { Error = ex.Message };
            }
        }

        /// <summary>
        /// Performs an external sign-in for a user based on their email address.
        /// </summary>
        /// <param name="email">The email of the user attempting to sign in.</param>
        /// <returns>
        /// An <see cref="AuthResponse"/> representing the outcome of the external sign-in operation.
        /// </returns>
        public AuthResponse ExternalSignIn(string email)
        {
            try
            {
                // retrieves the user
                var user = _context.User.FirstOrDefault(x => x.UserMail == email);
                if (user is null)
                    return new AuthResponse { Message = "User not found.", Success = false };

                // returns response
                return new AuthResponse
                {
                    Message      = "Ok.",
                    Success      = true,
                    Token        = user.VerifyToken,
                    RefreshToken = user.RefreshToken
                };
            }
            catch (Exception ex)
            {
                return new AuthResponse
                {
                    Message = ex.Message,
                    Success = false
                };
            }
        }

        /// <summary>
        /// Performs an external sign-up for a user.
        /// </summary>
        /// <param name="data">The data containing the external sign-up information.</param>
        /// <returns>
        /// An <see cref="AuthResponse"/> result.
        /// </returns>
        public async Task<AuthResponse> ExternalSignUp(User data)
        {
            try
            {
                if (data is null)
                    return new AuthResponse { Message = "Required data not found.", Success = false };

                // adds data to context
                await _context.User.AddAsync(data);

                // saves changes to the context
                await _context.SaveChangesAsync();

                return new AuthResponse
                {
                    Message = "Ok.",
                    Success = true
                };
            }
            catch (Exception ex)
            {
                return new AuthResponse
                {
                    Message = ex.Message,
                    Success = false
                };
            }
        }
    }
}
