using Application.Configurations;
using Application.DTOs;
using Application.Helpers;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Application.Response;
using Domain.Classes;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Services
{
    /// <summary>
    /// Service implementation of IUserService.
    /// </summary>
    public class UserService : IUserService
    {
        // Services
        private readonly IUserRepository _userRepository;
        private readonly IJwtService     _jwtService;
        private readonly JwtConfig       _jwtConfig;

        public UserService(IUserRepository userRepository, IJwtService jwtService, IOptions<JwtConfig> jwtConfig)
        {
            _userRepository = userRepository;
            _jwtService     = jwtService;
            _jwtConfig      = jwtConfig.Value;
        }

        /// <summary>
        /// Registers a new user.
        /// </summary>
        /// <param name="data">The user registration data.</param>
        /// <returns>
        /// An <see cref="AuthResponse"/> result.
        /// </returns>
        public async Task<AuthResponse> UserRegisterAsync(UserRegisterDto data)
        {
            try
            {
                if (data is null)
                    return new AuthResponse { Message = "Required data not found.", Success = false };

                // gets password hash and salt
                Helper.PasswordHash(data.Password, out byte[] passwordHash, out byte[] passwordSalt);

                // generates the token
                string jwtToken     = _jwtService.GenerateJwtToken(data.UserName, data.Email, data.Role);

                // new refresh token
                string refreshToken = _jwtService.GenerateRandomToken(100);

                // user data
                var user = new User()
                {
                    UserName       = data.UserName,
                    FirstName      = data.FirstName,
                    UserMail       = data.Email,
                    PasswordHash   = passwordHash,
                    PasswordSalt   = passwordSalt,
                    IsAdmin        = (data.Role == Role.Lead),
                    VerifyToken    = jwtToken,
                    ExpiresAt      = DateTime.Now.AddMinutes(_jwtConfig.ExpirationInMinutes),
                    RefreshToken   = refreshToken,
                    RefreshExpires = DateTime.Now.AddMonths(1)
                };

                // returns result
                return await _userRepository.UserRegisterAsync(user);
            }
            catch (Exception ex)
            {
                return new AuthResponse { Message = ex.Message, Success = false };
            }
        }

        /// <summary>
        /// Retrieves a list of all users.
        /// </summary>
        /// <returns>
        /// A list of <see cref="UserDto"/> representing all users.
        /// </returns>
        public async Task<List<UserDto>> AllUsers()
        {
            try
            {
                // gets all users
                var users = await _userRepository.AllUsers();

                if (!users.HasValue())
                    return new List<UserDto>();

                // returns converted data
                return users.Select(u => new UserDto
                {
                    UserId         = u.UserId,
                    UserName       = u.UserName,
                    FirstName      = u.FirstName,
                    UserMail       = u.UserMail,
                    PasswordHash   = u.PasswordHash,
                    PasswordSalt   = u.PasswordSalt,
                    GivenName      = u.GivenName,
                    FamilyName     = u.FamilyName,
                    EmailVerified  = u.EmailVerified,
                    Locale         = u.Locale,
                    Picture        = u.Picture,
                    VerifyToken    = u.VerifyToken,
                    ExpiresAt      = u.ExpiresAt,
                    RefreshToken   = u.RefreshToken,
                    RefreshExpires = u.RefreshExpires,
                    ResetToken     = u.ResetToken,
                    ResetExpires   = u.RefreshExpires,
                    IsAdmin        = u.IsAdmin
                })
                .OrderBy(o => o.UserId)
                .ToList();
            }
            catch (Exception ex)
            {
                return new List<UserDto>();
            }
        }

        /// <summary>
        /// Stores the forgot password token for a user.
        /// </summary>
        /// <param name="data">The data containing the forgot password information.</param>
        /// <returns>
        /// An <see cref="AuthResponse"/> result.
        /// </returns>
        public async Task<AuthResponse> ForgotTokenAsync(ForgotPasswordDto data)
        {
            try
            {
                if (data is null)
                    return new AuthResponse { Message = "Required data not found.", Success = false };

                // reset data
                var user = new User()
                {
                    UserMail     = data.Email,
                    ResetToken   = _jwtService.GenerateRandomToken(100),
                    ResetExpires = DateTime.Now.AddDays(1)
                };

                // returns result
                return await _userRepository.ForgotTokenAsync(user);
            }
            catch (Exception ex)
            {
                return new AuthResponse { Message = ex.Message, Success = false };
            }
        }

        /// <summary>
        /// Resets the user's password.
        /// </summary>
        /// <param name="data">The data containing the reset password information.</param>
        /// <returns>
        /// A <see cref="Result"/> indicating the outcome of the password reset.
        /// </returns>
        public async Task<Result> ResetPasswordAsync(ResetPasswordDto data)
        {
            try
            {
                if (data is null)
                    return new Result { Message = "Required data not found.", Success = false };

                // gets password hash and salt
                Helper.PasswordHash(data.Password, out byte[] passwordHash, out byte[] passwordSalt);

                // password reset data
                var user = new User()
                {
                    ResetToken   = null,
                    ResetExpires = null,
                    PasswordHash = passwordHash,
                    PasswordSalt = passwordSalt
                };

                // returns result
                return await _userRepository.ResetPasswordAsync(user);
            }
            catch (Exception ex)
            {
                return new Result { Message = ex.Message, Success = false };
            }
        }

        /// <summary>
        /// Resets the verification token and refresh token.
        /// </summary>
        /// <param name="data">The data containing the refresh token information.</param>
        /// <returns>
        /// An <see cref="AuthResponse"/> result.
        /// </returns>
        public async Task<AuthResponse> ResetVerifyTokenAsync(RefreshTokenDto data)
        {
            try
            {
                if (data is null)
                    return new AuthResponse { Message = "Required data not found.", Success = false };

                // data to reset the token
                var user = new User()
                {
                    RefreshToken = data.TokenRefresh
                };

                // returns result
                return await _userRepository.ResetVerifyTokenAsync(user);
            }
            catch (Exception ex)
            {
                return new AuthResponse { Message = ex.Message, Success = false };
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
                if (string.IsNullOrEmpty(accessToken))
                    return new GoogleResponse { Error = "Required value not found." };

                // returns result
                return await _userRepository.GoogleUserInfomation(accessToken);
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
                if (string.IsNullOrEmpty(email))
                    return new AuthResponse { Message = "Required value not found.", Success = false };

                // returns result
                return _userRepository.ExternalSignIn(email);
            }
            catch (Exception ex)
            {
                return new AuthResponse { Message = ex.Message, Success = false };
            }
        }

        /// <summary>
        /// Performs an external sign-up for a user.
        /// </summary>
        /// <param name="data">The data containing the external sign-up information.</param>
        /// <returns>
        /// An <see cref="AuthResponse"/> result.
        /// </returns>
        public async Task<AuthResponse> ExternalSignUp(ExternalSignUpDto data)
        {
            try
            {
                if(data is null)
                return new AuthResponse { Message = "Required data not found.", Success = false };

                // generates the token
                string jwtToken = _jwtService.GenerateJwtToken(data.GivenName, data.Email, data.Role);

                // new refresh token
                string refreshToken = _jwtService.GenerateRandomToken(100);

                // user data
                var user = new User()
                {
                    UserName       = data.GivenName,
                    FirstName      = data.GivenName,
                    UserMail       = data.Email,
                    IsAdmin        = (data.Role == Role.Lead),
                    GivenName      = data.GivenName,
                    FamilyName     = data.FamilyName,
                    EmailVerified  = data.EmailVerified,
                    Locale         = data.Locale,
                    Picture        = data.Picture,
                    VerifyToken    = jwtToken,
                    ExpiresAt      = DateTime.Now.AddMinutes(_jwtConfig.ExpirationInMinutes),
                    RefreshToken   = refreshToken,
                    RefreshExpires = DateTime.Now.AddMonths(1)
                };

                // returns result
                return await _userRepository.ExternalSignUp(user);
            }
            catch (Exception ex)
            {
                return new AuthResponse { Message = ex.Message, Success = false };
            }
        }
    }
}
