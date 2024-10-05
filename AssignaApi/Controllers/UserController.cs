using AssignaApi.Helpers;
using AssignaApi.Interfaces;
using AssignaApi.Models;
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
        // services
        private readonly IDataService _dataService;

        private readonly Helper _helper;

        public UserController(IDataService dataService, Helper helper)
        {
            _dataService = dataService;
            _helper = helper;
        }

        // user registration
        [HttpPost("register")]
        public async Task<JsonResult> UserRegister([FromBody] UserRegister data)
        {
            // validations
            if (!ModelState.IsValid)
            {
                return new JsonResult(new
                {
                    message = "Required data is not found.",
                    success = false
                });
            }

            // check user existance
            var mails = _dataService.AllUsers().Select(x => x.UserMail);
            if (mails.Contains(data.Email))
            {
                return new JsonResult(new
                {
                    message = "Email already exist.",
                    success = false
                });
            }

            // create user
            var result = await _dataService.UserRegisterAsync(data);
            if (result.success)
            {
                return new JsonResult(new
                {
                    message = "Ok",
                    success = true
                });
            }
            else
            {
                return new JsonResult(new
                {
                    message = "Server error.",
                    success = false
                });
            }
        }

        // user login
        [HttpPost("login")]
        public JsonResult UserLogin([FromBody] UserLogin data)
        {
            // validations
            if (!ModelState.IsValid)
            {
                return new JsonResult(new
                {
                    message = "Required data is not found.",
                    success = false
                });
            }

            // check user existance
            var user = _dataService.AllUsers().FirstOrDefault
            (
                x => x.UserName == data.UserName
            );

            if (user is null)
            {
                return new JsonResult(new
                {
                    message = "User is not found.",
                    success = false
                });
            }

            // check user password
            if (!_helper.VerifyPassword(data.Password, user.PasswordHash, user.PasswordSalt))
            {
                return new JsonResult(new
                {
                    message = "Username or password is incorrect.",
                    success = false
                });
            }

            return new JsonResult(new
            {
                message = "Ok",
                success = true,
                token = user.VerifyToken,
                refresh_token = user.RefreshToken
            });
        }

        // forgot password
        [HttpPost("forgot-password")]
        public async Task<JsonResult> ForgotPassword([FromBody] ForgotPassword data)
        {
            // validations
            if (!ModelState.IsValid)
            {
                return new JsonResult(new
                {
                    message = "Required data is not found.",
                    success = false
                });
            }

            // check user existance
            var user = _dataService.AllUsers().FirstOrDefault
            (
                x => x.UserMail == data.Email
            );

            if (user is null)
            {
                return new JsonResult(new
                {
                    message = "User is not found.",
                    success = false
                });
            }

            var result = await _dataService.ForgotTokenAsync(data);
            if (result.success)
            {
                return new JsonResult(new
                {
                    message = "Ok",
                    success = true,
                    result.reset_token
                });
            }
            else
            {
                return new JsonResult(new
                {
                    message = "Server error.",
                    success = false
                });
            }
        }

        // reset password
        [HttpPost("reset-password")]
        public async Task<JsonResult> ResetPassword([FromBody] ResetPassword data)
        {
            // validations
            if (!ModelState.IsValid)
            {
                return new JsonResult(new
                {
                    message = "Required data is not found.",
                    success = false
                });
            }

            // check user and reset token is valid or not
            var user = _dataService.AllUsers().FirstOrDefault
            (
                x => x.ResetToken == data.ResetToken
            );

            if (user is null || user.ResetExpires < DateTime.Now.ToUniversalTime())
            {
                return new JsonResult(new
                {
                    message = "Reset token is expired.",
                    success = false
                });
            }

            var result = await _dataService.ResetPasswordAsync(data);
            if (result.success)
            {
                return new JsonResult(new
                {
                    message = "Ok",
                    success = true
                });
            }
            else
            {
                return new JsonResult(new
                {
                    message = "Server error.",
                    success = false
                });
            }
        }

        // refresh token
        [HttpPost("refresh-token")]
        public async Task<JsonResult> RefreshToken([FromBody] RefreshToken data)
        {
            // validations
            if (!ModelState.IsValid)
            {
                return new JsonResult(new
                {
                    message = "Required data is not found.",
                    success = false
                });
            }

            // check user and refresh token is valid or not
            var user = _dataService.AllUsers().FirstOrDefault
            (
                x => x.RefreshToken == data.TokenRefresh
            );


            if (user is null || user.RefreshExpires < DateTime.Now)
            {
                return new JsonResult(new
                {
                    message = "Refresh token is expired.",
                    success = false
                });
            }

            // check verify token is expired or not
            if (user.ExpiresAt > DateTime.Now)
            {
                return new JsonResult(new
                {
                    message = "Verify token is still not expired.",
                    success = false
                });
            }

            var result = await _dataService.ResetVerifyTokenAsync(data);
            if (result.success)
            {
                return new JsonResult(new
                {
                    message = "Ok",
                    success = true,
                    result.token,
                    result.refresh_token
                });
            }
            else
            {
                return new JsonResult(new
                {
                    message = "Ok",
                    success = true
                });
            }
        }

        // team members
        [HttpGet("members")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,
        Roles = Roles.lead)]
        public JsonResult TeamMembers()
        {
            var result = _dataService.TeamMembers();
            return new JsonResult(new
            {
                message = "Ok",
                success = true,
                data = result
            });
        }

        // external login
        [HttpPost("external-login")]
        public async Task<JsonResult> ExternalLogin([FromBody] ExternalSignIn data)
        {
            // validations
            if (!ModelState.IsValid)
            {
                return new JsonResult(new
                {
                    message = "Required data is not found.",
                    success = false
                });
            }

            // get external user info
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

        // google signin
        private async Task<JsonResult> GoogleLogin(ExternalSignIn data)
        {
            // get external user info
            var info = await _dataService.GoogleUserInfomation(data.AccessToken);
            if (string.IsNullOrEmpty(info.email))
            {
                return new JsonResult(new
                {
                    message = "Sign in information not found.",
                    success = false
                });
            }

            // sign in if user already has an account
            var result = _dataService.ExternalSignIn(info.email);
            if (result.success)
            {
                return new JsonResult(new
                {
                    message = "Ok",
                    success = true,
                    result.token,
                    result.refresh_token
                });
            }
            else
            {
                // create account if user not exist
                var mails = _dataService.AllUsers().Select(x => x.UserMail);
                if (!mails.Contains(info.email))
                {
                    // return user when no role provided
                    // we need a user role type to assign to the new user. That can capture from the URl route when the user is signing up

                    if (data.Role != Roles.member & data.Role != Roles.lead)
                    {
                        return new JsonResult(new
                        {
                            message = "Account type does not match with correct account type.",
                            success = false
                        });
                    }

                    // sign up user
                    result = await _dataService.ExternalSignUp(new ExternalSignUp
                    {
                        GivenName = info.given_name,
                        FamilyName = info.family_name,
                        Picture = info.picture,
                        EmailVerified = info.email_verified,
                        Locale = info.locale,
                        Email = info.email,
                        Role = data.Role
                    });

                    if (!result.success)
                    {
                        return new JsonResult(new
                        {
                            message = "Error occurred during the sign up process.",
                            success = false
                        });
                    }
                }

                // sign in user
                result = _dataService.ExternalSignIn(info.email);
                if (result.success)
                {
                    return new JsonResult(new
                    {
                        message = "Ok",
                        success = true,
                        result.token,
                        result.refresh_token
                    });
                }
                else
                {
                    return new JsonResult(new
                    {
                        message = "Error occurred during the sign in process.",
                        success = false
                    });
                }

            }
        }
    }
}