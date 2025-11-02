using Application.Configurations;
using Application.DTOs;
using Application.Helpers;
using Application.Interfaces.Services;
using Infrastructure.Configurations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AssignaApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        // Services
        private readonly IUserService _userService;
        private readonly IMailService _mailService;
        private readonly ITaskService _taskService;
        private readonly ClientApp    _clientApp;

        public UserController(IUserService userService, IMailService mailService, ITaskService taskService, ClientApp clientApp)
        {
            _userService = userService;
            _mailService = mailService;
            _taskService = taskService;
            _clientApp   = clientApp;
        }

        /// <summary>
        /// Registers a new user to the system.
        /// </summary>
        /// <param name="data">The user registration data.</param>
        /// <returns>
        /// A <see cref="JsonResult"/> containing the task data.
        /// </returns>
        [HttpPost("register")]
        public async Task<JsonResult> UserRegister([FromBody] UserRegisterDto data)
        {
            // validations
            if (!ModelState.IsValid)
                return new JsonResult(new
                {
                    message = "Required data is not found.",
                    success = false
                });

            // checks the user existence
            var mails = (await _userService.AllUsers())?.Select(x => x.UserMail);
            if (mails.HasValue() && mails.Contains(data.Email))
                return new JsonResult(new
                {
                    message = "Email already exist.",
                    success = false
                });

            // creates a new user
            var result = await _userService.UserRegisterAsync(data);
            if (result.Success)
                return new JsonResult(new
                {
                    message = "Ok.",
                    success = true
                });

            return new JsonResult(new
            {
                message = "Server error.",
                success = false
            });
        }

        /// <summary>
        /// Logins user to the system.
        /// </summary>
        /// <param name="data">The user login data.</param>
        /// <returns>
        /// A <see cref="JsonResult"/> containing the task data.
        /// </returns>
        [HttpPost("login")]
        public async Task<JsonResult> UserLogin([FromBody] UserLoginDto data)
        {
            // validations
            if (!ModelState.IsValid)
                return new JsonResult(new
                {
                    message = "Required data is not found.",
                    success = false
                });

            // checks the user existence
            var user = (await _userService.AllUsers())?.FirstOrDefault(x => x.UserName == data.UserName);
            if (user is null)
                return new JsonResult(new
                {
                    message = "User is not found.",
                    success = false
                });

            // checks the user password
            if (!Helper.VerifyPassword(data.Password, user.PasswordHash, user.PasswordSalt))
                return new JsonResult(new
                {
                    message = "Username or password is incorrect.",
                    success = false
                });

            // gets the result
            var result = await _userService.ResetVerifyTokenAsync(new RefreshTokenDto { TokenRefresh = user.RefreshToken });
            if (!result.Success)
                return new JsonResult(new
                {
                    message = "Token updating process has failed. Please contact the admin.",
                    success = true
                });

            return new JsonResult(new
            {
                message = "Ok.",
                success = true,
                result.Token,
                result.RefreshToken
            });
        }

        /// <summary>
        /// Sends password reset link.
        /// </summary>
        /// <param name="email">The user email address.</param>
        /// <returns>
        /// A <see cref="JsonResult"/> containing the task data.
        /// </returns>
        [HttpGet("send-reset-link")]
        public async Task<JsonResult> SendResetLink(string email)
        {
            if(string.IsNullOrWhiteSpace(email))
                return new JsonResult(new
                {
                    message = "Required data is not found.",
                    success = false
                });

            // checks the user existence
            var user = (await _userService.AllUsers())?.FirstOrDefault(x => x.UserMail == email);
            if (user is null)
                return new JsonResult(new
                {
                    message = "User is not found.",
                    success = false
                });

            // gets the reset token and prepare the redirect link
            var result = await _userService.ForgotTokenAsync(new ForgotPasswordDto { Email = email });
            if (result.Success)
            {
                // URL encode the token to be safe
                string resetLink = $"{_clientApp.PasswordResetUrl}={Uri.EscapeDataString(result.ResetToken)}";

                // sends the email
                await _mailService.SendMailAsync(email, "Password Reset", $"Click <a href='{resetLink}'>here</a> to reset your password.");

                return new JsonResult(new
                {
                    message = "Reset link sent to your email.",
                    success = true
                });
            }

            return new JsonResult(new
            {
                message = "Server error.",
                success = false
            });
        }

        /// <summary>
        /// Sends the password reset token.
        /// </summary>
        /// <param name="data">Forgot password data.</param>
        /// <returns>
        /// A <see cref="JsonResult"/> containing the task data.
        /// </returns>
        [HttpPost("forgot-password")]
        public async Task<JsonResult> ForgotPassword([FromBody] ForgotPasswordDto data)
        {
            // validations
            if (!ModelState.IsValid)
                return new JsonResult(new
                {
                    message = "Required data is not found.",
                    success = false
                });

            // checks the user existence
            var user = (await _userService.AllUsers())?.FirstOrDefault(x => x.UserMail == data.Email);
            if (user is null)
                return new JsonResult(new
                {
                    message = "User is not found.",
                    success = false
                });

            // gets the result
            var result = await _userService.ForgotTokenAsync(data);
            if (result.Success)
                return new JsonResult(new
                {
                    message = "Ok.",
                    success = true,
                    result.ResetToken
                });

            return new JsonResult(new
            {
                message = "Server error.",
                success = false
            });
        }

        /// <summary>
        /// Resets the user password.
        /// </summary>
        /// <param name="data">Reset password data.</param>
        /// <returns>
        /// A <see cref="JsonResult"/> containing the task data.
        /// </returns>
        [HttpPost("reset-password")]
        public async Task<JsonResult> ResetPassword([FromBody] ResetPasswordDto data)
        {
            // validations
            if (!ModelState.IsValid)
                return new JsonResult(new
                {
                    message = "Required data is not found.",
                    success = false
                });

            // checks the user and reset token is valid or not
            var user = (await _userService.AllUsers())?.FirstOrDefault(x => x.ResetToken == data.ResetToken);
            if (user is null || user.ResetExpires < DateTime.Now.ToUniversalTime())
                return new JsonResult(new
                {
                    message = "Reset token is expired.",
                    success = false
                });

            // gets the result
            var result = await _userService.ResetPasswordAsync(data);
            if (result.Success)
            {
                // sends the email
                await _mailService.SendMailAsync(user.UserMail, "Password Reset", "Your password reset successfully.");

                return new JsonResult(new
                {
                    message = "Ok.",
                    success = true
                });
            }

            return new JsonResult(new
            {
                message = "Server error.",
                success = false
            });
        }

        /// <summary>
        /// Sends the refresh token.
        /// </summary>
        /// <param name="data">Token refresh data.</param>
        /// <returns>
        /// A <see cref="JsonResult"/> containing the task data.
        /// </returns>
        [HttpPost("refresh-token")]
        public async Task<JsonResult> RefreshToken([FromBody] RefreshTokenDto data)
        {
            // validations
            if (!ModelState.IsValid)
                return new JsonResult(new
                {
                    message = "Required data is not found.",
                    success = false
                });

            // check the user is valid or not
            var user = (await _userService.AllUsers())?.FirstOrDefault(x => x.RefreshToken == data.TokenRefresh);
            if (user is null)
                return new JsonResult(new
                {
                    message = "No user is found.",
                    success = false
                });

            // checks the verify token is expired or not
            if (user.ExpiresAt > DateTime.Now)
                return new JsonResult(new
                {
                    message = "Verify token is still not expired.",
                    success = false
                });

            // checks the refresh token is expired or not
            if (user.RefreshExpires < DateTime.Now)
                return new JsonResult(new
                {
                    message = "Refresh token is expired.",
                    success = false
                });

            // gets the result
            var result = await _userService.ResetVerifyTokenAsync(data);
            if (!result.Success)
                return new JsonResult(new
                {
                    message = "Token updating process has failed. Please contact the admin.",
                    success = false
                });

            return new JsonResult(new
            {
                message = "Ok.",
                success = true,
                result.Token,
                result.RefreshToken
            });
        }

        /// <summary>
        /// Retrieves all team members.
        /// </summary>
        /// <returns>
        /// A <see cref="JsonResult"/> containing the task data.
        /// </returns>
        [HttpGet("members")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = Role.Lead)]
        public async Task<JsonResult> TeamMembers()
        {
            // gets all team-members
            var result = await _taskService.TeamMembers();

            return new JsonResult(new
            {
                message = "Ok.",
                success = true,
                data    = result
            });
        }

        /// <summary>
        /// Directs request to the corresponding external sign in provider.
        /// </summary>
        /// <param name="data">External login data.</param>
        /// <returns>
        /// A <see cref="JsonResult"/> containing the task data.
        /// </returns>
        [HttpPost("external-login")]
        public async Task<JsonResult> ExternalLogin([FromBody] ExternalSignInDto data)
        {
            // validations
            if (!ModelState.IsValid)
                return new JsonResult(new
                {
                    message = "Required data is not found.",
                    success = false
                });

            // directs to the provider
            switch (data.Provider)
            {
                case "Google":
                    return await GoogleLogin(data);
                default:
                    return new JsonResult(new
                    {
                        message = "Sign in provider is not found.",
                        success = false
                    });
            }          
        }

        /// <summary>
        /// External signing process.
        /// </summary>
        /// <param name="data">External login data.</param>
        /// <returns>
        /// A <see cref="JsonResult"/> containing the task data.
        /// </returns>
        private async Task<JsonResult> GoogleLogin(ExternalSignInDto data)
        {
            // gets external user info
            var info = await _userService.GoogleUserInfomation(data.AccessToken);
            if (string.IsNullOrEmpty(info.Email))
                return new JsonResult(new
                {
                    message = "Sign in information not found.",
                    success = false
                });

            // signs in if the user already has an account
            var result = _userService.ExternalSignIn(info.Email);
            if (result.Success)
            {
                return new JsonResult(new
                {
                    message = "Ok.",
                    success = true,
                    result.Token,
                    result.RefreshToken
                });
            }
            else
            {
                // creates account if the user not exist
                var mails = (await _userService.AllUsers())?.Select(x => x.UserMail);
                if (mails.HasValue() && !mails.Contains(info.Email))
                {
                    // returns the user when no role is provided
                    // we need a user role type to assign to the new user. that can capture from the URl route when the user is signing up

                    if (data.Role != Role.Member & data.Role != Role.Lead)
                        return new JsonResult(new
                        {
                            message = "Account type does not match with correct account type.",
                            success = false
                        });

                    // gets the result of signup
                    result = await _userService.ExternalSignUp(new ExternalSignUpDto
                    {
                        GivenName     = info.GivenName ?? info.Name,
                        FamilyName    = info.FamilyName,
                        Picture       = info.Picture,
                        EmailVerified = info.EmailVerified,
                        Locale        = info.Locale,
                        Email         = info.Email,
                        Role          = data.Role
                    });

                    if (!result.Success)
                        return new JsonResult(new
                        {
                            message = "Error occurred during the sign up process.",
                            success = false
                        });
                }

                // gets the result of sign-in
                result = _userService.ExternalSignIn(info.Email);
                if (result.Success)
                    return new JsonResult(new
                    {
                        message = "Ok.",
                        success = true,
                        result.Token,
                        result.RefreshToken
                    });

                return new JsonResult(new
                {
                    message = "Error occurred during the sign in process.",
                    success = false
                });
            }
        }

        //[HttpGet("verify-email")]
        //public async Task<IActionResult> VerifyEmail(string token)
        //{
        //    var user = await _context.Users.FirstOrDefaultAsync(u => u.EmailVerificationToken == token);

        //    if (user == null)
        //        return BadRequest("Invalid or expired token.");

        //    user.IsEmailVerified = true;
        //    user.EmailVerifiedAt = DateTime.UtcNow;
        //    user.EmailVerificationToken = null;

        //    await _context.SaveChangesAsync();
        //    return Ok("Email successfully verified!");
        //}

        /// <summary>
        /// Gets the each task count.
        /// </summary>
        /// <param name="email">The user email address.</param>
        /// <returns>
        /// A <see cref="JsonResult"/> containing the task data.
        /// </returns>
        [HttpGet("task-count")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<JsonResult> TaskCount(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return new JsonResult(new
                {
                    message = "Required data is not found.",
                    success = false
                });

            // checks the user existence
            var user = (await _userService.AllUsers())?.FirstOrDefault(x => x.UserMail == email);
            if (user is null)
                return new JsonResult(new
                {
                    message = "User is not found.",
                    success = false
                });

            // gets the tasks counts
            var counts = await _userService.TaskCount();
            if(counts is null)
                return new JsonResult(new
                {
                    message = "No data found.",
                    success = false
                });

            return new JsonResult(new
            {
                message = "Ok.",
                success = true,
                data    = counts
            });
        }
    }
}