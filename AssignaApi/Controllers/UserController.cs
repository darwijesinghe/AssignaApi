using System;
using System.Linq;
using System.Threading.Tasks;
using AssignaApi.Helpers;
using AssignaApi.Interfaces;
using AssignaApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace AssignaApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        // services
        private readonly IDataService _dataService;
        private readonly Helper _helper;
        private readonly JwtConfig _jwtConfig;
        public UserController(IDataService dataService, Helper helper,
        IOptions<JwtConfig> jwtConfig)
        {
            _dataService = dataService;
            _helper = helper;
            _jwtConfig = jwtConfig.Value;
        }

        // user registration
        [HttpPost("register")]
        public async Task<JsonResult> UserRegister([FromBody] UserRegister data)
        {

            // validate incoming data
            if (!ModelState.IsValid)
            {
                return new JsonResult(new
                {
                    message = "Required data not valid",
                    success = false
                });
            }

            // check user existance
            var mails = _dataService.AllUsers().Select(x => x.user_mail);
            if (mails.Contains(data.email))
            {
                return new JsonResult(new
                {
                    message = "Email already exist",
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
                    message = "Server error",
                    success = false
                });
            }

        }

        // user login
        [HttpPost("login")]
        public JsonResult UserLogin([FromBody] UserLogin data)
        {
            // validate incoming data
            if (!ModelState.IsValid)
            {
                return new JsonResult(new
                {
                    message = "Required data not valid",
                    success = false
                });
            }

            // check user existance
            var user = _dataService.AllUsers().FirstOrDefault
            (
                x => x.user_name == data.user_name
            );

            if (user is null)
            {
                return new JsonResult(new
                {
                    message = "User is not found",
                    success = false
                });
            }

            // check user password
            if (!_helper.VerifyPassword(data.password, user.password_hash, user.password_salt))
            {
                return new JsonResult(new
                {
                    message = "Username or password is incorrect",
                    success = false
                });
            }

            return new JsonResult(new
            {
                message = "Ok",
                success = true,
                token = user.verify_token,
                refresh_token = user.refresh_token
            });

        }

        // forgot password
        [HttpPost("forgot-password")]
        public async Task<JsonResult> ForgotPassword([FromBody] ForgotPassword data)
        {
            // validate incoming data
            if (!ModelState.IsValid)
            {
                return new JsonResult(new
                {
                    message = "Required data not valid",
                    success = false
                });
            }

            // check user existance
            var user = _dataService.AllUsers().FirstOrDefault
            (
                x => x.user_mail == data.email
            );

            if (user is null)
            {
                return new JsonResult(new
                {
                    message = "User is not found",
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
                    reset_token = result.reset_token
                });
            }
            else
            {
                return new JsonResult(new
                {
                    message = "Server error",
                    success = false
                });
            }

        }

        // reset password
        [HttpPost("reset-password")]
        public async Task<JsonResult> ResetPassword([FromBody] ResetPassword data)
        {
            // validate incoming data
            if (!ModelState.IsValid)
            {
                return new JsonResult(new
                {
                    message = "Required data not valid",
                    success = false
                });
            }

            // check user and reset token is valid or not
            var user = _dataService.AllUsers().FirstOrDefault
            (
                x => x.reset_token == data.reset_token
            );

            if (user is null || user.reset_expires < DateTime.Now.ToUniversalTime())
            {
                return new JsonResult(new
                {
                    message = "Token is not valid",
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
                    message = "Server error",
                    success = false
                });
            }
        }

        // refresh token
        [HttpPost("refresh-token")]
        public async Task<JsonResult> RefreshToken([FromBody] RefreshToken data)
        {
            // validate incoming data
            if (!ModelState.IsValid)
            {
                return new JsonResult(new
                {
                    message = "Required data not valid",
                    success = false
                });
            }

            // check user and reset token is valid or not
            var user = _dataService.AllUsers().FirstOrDefault
            (
                x => x.refresh_token == data.refresh_token
            );

            if (user is null || user.refresh_expires < DateTime.Now.ToUniversalTime())
            {
                return new JsonResult(new
                {
                    message = "Token is not valid",
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
                    token = result.token,
                    refresh_token = result.refresh_token
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

    }
}