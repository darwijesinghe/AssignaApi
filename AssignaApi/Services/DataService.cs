using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using AssignaApi.Data;
using AssignaApi.Helpers;
using AssignaApi.Interfaces;
using AssignaApi.Models;
using AssignaApi.Response;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Task = AssignaApi.Models.Task;

namespace AssignaApi.Services
{
    public class DataService : IDataService
    {
        // variables
        private const string empty = "";

        // services
        private DataContext Context { get; }
        private readonly Helper _helper;
        private readonly JwtHelpers _jwtHelpers;
        private readonly JwtConfig _jwtConfig;
        public DataService(DataContext context, Helper helper,
        JwtHelpers jwtHelpers, IOptions<JwtConfig> jwtConfig)
        {
            Context = context;
            _helper = helper;
            _jwtHelpers = jwtHelpers;
            _jwtConfig = jwtConfig.Value;
        }

        // new user register
        public async Task<AuthResult> UserRegisterAsync(UserRegister data)
        {
            // password hash
            _helper.PasswordHash(
                data.Password,
                out byte[] passwordHash,
                out byte[] passwordSalt
            );

            // generate the token
            string jwtToken = _jwtHelpers.GenerateJwtToken
            (
                name: data.UserName,
                mail: data.Email,
                role: data.Role
            );

            // refresh token
            string refToken = _jwtHelpers.GenerateRandomToken(100);

            // expire time
            var minutes = TimeSpan.Parse(_jwtConfig.Expire).Minutes;

            var user = new Users()
            {
                user_name = data.UserName ?? empty,
                first_name = data.FirstName ?? empty,
                user_mail = data.Email ?? empty,
                password_hash = passwordHash,
                password_salt = passwordSalt,
                is_admin = (data.Role == Roles.lead),
                verify_token = jwtToken,
                expires_at = DateTime.Now.AddMinutes(minutes),
                refresh_token = refToken,
                refresh_expires = DateTime.Now.AddMonths(1)
            };

            Context.users.Add(user);

            try
            {
                await Context.SaveChangesAsync();

                return new AuthResult
                {
                    message = "Ok",
                    success = true
                };
            }
            catch (Exception ex)
            {
                return new AuthResult
                {
                    message = ex.Message,
                    success = false
                };
            }
        }

        // application users
        public List<UsersDto> AllUsers()
        {
            var users = Context.users
            .Select(x => new UsersDto
            {
                UserId = x.user_id,
                UserName = x.user_name,
                FirstName = x.first_name,
                UserMail = x.user_mail,
                PasswordHash = x.password_hash ?? new byte[32],
                PasswordSalt = x.password_salt ?? new byte[32],
                GivenName = x.given_name ?? empty,
                FamilyName = x.family_name ?? empty,
                EmailVerified = x.email_verified,
                Locale = x.locale ?? empty,
                Picture = x.picture ?? empty,
                VerifyToken = x.verify_token,
                ExpiresAt = x.expires_at,
                RefreshToken = x.refresh_token,
                RefreshExpires = x.refresh_expires,
                ResetToken = x.reset_token,
                ResetExpires = x.reset_expires,
                IsAdmin = x.is_admin
            })
            .OrderBy(x => x.UserId)
            .ToList();

            return users;
        }

        // save password reset token
        public async Task<AuthResult> ForgotTokenAsync(ForgotPassword data)
        {
            // user retrieve
            var user = Context.users.FirstOrDefault
            (
                x => x.user_mail == data.Email
            );

            user.reset_token = _jwtHelpers.GenerateRandomToken(100);
            user.reset_expires = DateTime.Now.AddDays(1);

            try
            {
                await Context.SaveChangesAsync();
                return new AuthResult
                {
                    message = "Ok",
                    success = true,
                    reset_token = user.reset_token
                };
            }
            catch (Exception ex)
            {
                return new AuthResult
                {
                    message = ex.Message,
                    success = false
                };
            }
        }

        // reset password
        public async Task<Result> ResetPasswordAsync(ResetPassword data)
        {
            // user retrieve
            var user = Context.users.FirstOrDefault
            (
                x => x.reset_token == data.ResetToken
            );

            // password hash
            _helper.PasswordHash(
                data.Password,
                out byte[] passwordHash,
                out byte[] passwordSalt
            );

            user.reset_token = null;
            user.reset_expires = null;
            user.password_hash = passwordHash;
            user.password_salt = passwordSalt;

            try
            {
                await Context.SaveChangesAsync();

                return new Result
                {
                    message = "Ok",
                    success = true,
                };
            }
            catch (Exception ex)
            {
                return new Result
                {
                    message = ex.Message,
                    success = false
                };
            }
        }

        // reset verify token and refresh token
        public async Task<AuthResult> ResetVerifyTokenAsync(RefreshToken data)
        {
            // user retrieve
            var user = Context.users.FirstOrDefault
            (
                x => x.refresh_token == data.TokenRefresh
            );

            // generate the token
            string jwtToken = _jwtHelpers.GenerateJwtToken
            (
                name: user.user_name,
                mail: user.user_mail,
                role: (user.is_admin == true) ? Roles.lead : Roles.member
            );

            // refresh token
            string refToken = _jwtHelpers.GenerateRandomToken(100);

            // expire time
            var minutes = TimeSpan.Parse(_jwtConfig.Expire).Minutes;

            user.verify_token = jwtToken;
            user.expires_at = DateTime.Now.AddMinutes(minutes);
            user.refresh_token = refToken;
            user.refresh_expires = DateTime.Now.AddMonths(1);

            try
            {
                await Context.SaveChangesAsync();

                return new AuthResult
                {
                    message = "Ok",
                    success = true,
                    token = user.verify_token,
                    refresh_token = user.refresh_token
                };
            }
            catch (Exception ex)
            {
                return new AuthResult
                {
                    message = ex.Message,
                    success = false
                };
            }
        }

        #region team lead

        // get all task categories
        public List<CategoryDto> AllCategories()
        {
            var categories = Context.category
            .Select(x => new CategoryDto
            {
                CatId = x.cat_id,
                CatName = x.cat_name
            })
            .OrderBy(x => x.CatId)
            .ToList();

            return categories;
        }

        // get team members
        public List<UsersDto> TeamMembers()
        {
            var members = Context.users
            .Select(x => new UsersDto
            {
                UserId = x.user_id,
                UserName = x.user_name,
                FirstName = x.first_name,
                UserMail = x.user_mail,
                IsAdmin = x.is_admin
            })
            .Where(x => x.IsAdmin == false)
            .OrderBy(x => x.UserId)
            .ToList();

            return members;
        }

        // get priorities
        public List<PriorityDto> Priorities()
        {
            var priorities = Context.priority
            .Select(x => new PriorityDto
            {
                PriId = x.pri_id,
                PriName = x.pri_name
            })
            .OrderBy(x => x.PriId)
            .ToList();

            return priorities;
        }

        // add a new task
        public async Task<Result> SaveTaskAsync(TaskDto data)
        {
            var task = new Task()
            {
                tsk_title = data.TskTitle,
                deadline = data.Deadline,
                tsk_note = data.TskNote,
                pri_high = data.PriHigh,
                pri_medium = data.PriMedium,
                pri_low = data.PriLow,
                cat_id = data.CatId,
                user_id = data.UserId,
                pending = data.Pending
            };

            Context.task.Add(task);

            try
            {
                await Context.SaveChangesAsync();
                return new Result
                {
                    message = "Ok",
                    success = true
                };
            }
            catch (Exception ex)
            {
                return new Result
                {
                    message = ex.Message,
                    success = false
                };
            }
        }

        // edit task
        public async Task<Result> EditTaskAsync(TaskDto data)
        {
            var task = Context.task.FirstOrDefault(x => x.tsk_id == data.TskId);

            if (task is not null)
            {
                task.tsk_title = data.TskTitle;
                task.cat_id = data.CatId;
                task.deadline = data.Deadline;
                task.pri_high = data.PriHigh;
                task.pri_medium = data.PriMedium;
                task.pri_low = data.PriLow;
                task.user_id = data.UserId;
                task.tsk_note = data.TskNote;
            }

            try
            {
                await Context.SaveChangesAsync();
                return new Result
                {
                    message = "Ok",
                    success = true
                };
            }
            catch (Exception ex)
            {
                return new Result
                {
                    message = ex.Message,
                    success = false
                };
            }
        }

        // delete a task
        public async Task<Result> DeleteTaskAsync(DeleteTask data)
        {
            var task = Context.task.FirstOrDefault(x => x.tsk_id == data.TskId);
            Context.task.Remove(task);

            try
            {
                await Context.SaveChangesAsync();
                return new Result
                {
                    message = "Ok",
                    success = true
                };
            }
            catch (Exception ex)
            {
                return new Result
                {
                    message = ex.Message,
                    success = false
                };
            }
        }

        #endregion

        // task info
        public async Task<List<TaskDto>> TaskInfo(int taskId)
        {
            var taks = await (from us in Context.users
                              join tk in Context.task on us.user_id equals tk.users.user_id
                              join ct in Context.category on tk.category.cat_id equals ct.cat_id
                              select new TaskDto
                              {
                                  TskId = tk.tsk_id,
                                  TskTitle = tk.tsk_title,
                                  Deadline = tk.deadline,
                                  TskNote = tk.tsk_note,
                                  Pending = tk.pending,
                                  Complete = tk.complete,
                                  PriHigh = tk.pri_high,
                                  PriMedium = tk.pri_medium,
                                  PriLow = tk.pri_low,
                                  UserNote = tk.user_note,
                                  CatId = tk.cat_id,
                                  UserId = tk.user_id,
                                  FirstName = us.first_name,
                                  UserMail = us.user_mail,
                                  CatName = ct.cat_name

                              }).ToListAsync();

            return taks
            .Where(x => x.TskId == taskId)
            .OrderBy(x => x.TskId).ToList();
        }

        // all tasks
        public async Task<List<TaskDto>> AllTasks()
        {
            var taks = await this.TaskData();
            return taks.OrderBy(x => x.TskId).ToList();
        }

        // pending tasks
        public async Task<List<TaskDto>> Pendings()
        {
            var taks = await this.TaskData();
            return taks.Where(x => x.Pending == true)
            .OrderBy(x => x.TskId)
            .ToList();
        }

        // completed tasks
        public async Task<List<TaskDto>> Completed()
        {
            var taks = await this.TaskData();
            return taks.Where(x => x.Complete == true)
            .OrderBy(x => x.TskId)
            .ToList();
        }

        // high priority tasks
        public async Task<List<TaskDto>> HighPriority()
        {
            var taks = await this.TaskData();
            return taks.Where(x => x.PriHigh == true)
            .OrderBy(x => x.TskId)
            .ToList();
        }

        // medium priority tasks
        public async Task<List<TaskDto>> MediumPriority()
        {
            var taks = await this.TaskData();
            return taks.Where(x => x.PriMedium == true)
            .OrderBy(x => x.TskId)
            .ToList();
        }

        // low priority tasks
        public async Task<List<TaskDto>> LowPriority()
        {
            var taks = await this.TaskData();
            return taks.Where(x => x.PriLow == true)
            .OrderBy(x => x.TskId)
            .ToList();
        }

        #region team member

        // add task note
        public async Task<Result> AddTaskNoteAsync(TaskDto data)
        {
            var task = Context.task.FirstOrDefault(x => x.tsk_id == data.TskId);
            if (task is not null)
            {
                task.user_note = data.UserNote;
            }

            try
            {
                await Context.SaveChangesAsync();
                return new Result
                {
                    message = "Ok",
                    success = true
                };
            }
            catch (Exception ex)
            {
                return new Result
                {
                    message = ex.Message,
                    success = false
                };
            }
        }

        // mark as task done
        public async Task<Result> MarkasDoneAsync(MarkDone data)
        {
            var task = Context.task.FirstOrDefault(x => x.tsk_id == data.TskId);
            if (task is not null)
            {
                task.pending = false;
                task.complete = true;
            }

            try
            {
                await Context.SaveChangesAsync();
                return new Result
                {
                    message = "Ok",
                    success = true
                };
            }
            catch (Exception ex)
            {
                return new Result
                {
                    message = ex.Message,
                    success = false
                };
            }
        }

        #endregion

        // single method use to return task info
        private async Task<List<TaskDto>> TaskData()
        {
            var taks = await (from us in Context.users
                              join tk in Context.task on us.user_id equals tk.users.user_id
                              select new TaskDto
                              {
                                  TskId = tk.tsk_id,
                                  TskTitle = tk.tsk_title,
                                  Deadline = tk.deadline,
                                  TskNote = tk.tsk_note,
                                  Pending = tk.pending,
                                  Complete = tk.complete,
                                  PriHigh = tk.pri_high,
                                  PriMedium = tk.pri_medium,
                                  PriLow = tk.pri_low,
                                  UserNote = tk.user_note,
                                  CatId = tk.cat_id,
                                  UserId = tk.user_id,
                                  UserName = us.user_name,
                                  FirstName = us.first_name

                              }).ToListAsync();

            // get user name and user role
            var claims = _jwtHelpers.ReadJwtToken();

            if (claims.role == Roles.member)
            {
                // filter by member name
                var filterdTasks = taks.Where
                (
                    x => x.UserName == claims.user_name
                );

                return filterdTasks.ToList();
            }

            return taks;
        }

        // get google user infomation
        public async Task<GoogleResponse> GoogleUserInfomation(string accessToken)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    // access token
                    var header = new AuthenticationHeaderValue("Bearer", accessToken);
                    client.DefaultRequestHeaders.Authorization = header;

                    // get the response
                    var response = new HttpResponseMessage();
                    response = await client.GetAsync("https://www.googleapis.com/oauth2/v3/userinfo");
                    if (response.IsSuccessStatusCode)
                    {
                        // read the response
                        var data = await response.Content.ReadAsStringAsync();

                        // deserializing
                        var info = JsonConvert.DeserializeObject<GoogleResponse>(data);

                        return new GoogleResponse
                        {
                            sub = info.sub,
                            name = info.name,
                            given_name = info.given_name,
                            family_name = info.family_name,
                            picture = info.picture,
                            email = info.email,
                            email_verified = info.email_verified,
                            locale = info.locale
                        };
                    }

                    return new GoogleResponse() { error = response.ReasonPhrase };
                }
            }
            catch (Exception ex)
            {
                return new GoogleResponse() { error = ex.Message };
            }
        }

        // external signin
        public AuthResult ExternalSignIn(string email)
        {
            try
            {
                // get user from email
                var user = AllUsers().FirstOrDefault
                (
                    x => x.UserMail == email
                );

                if (user is not null)
                {
                    return new AuthResult
                    {
                        message = "Ok",
                        success = true,
                        token = user.VerifyToken,
                        refresh_token = user.RefreshToken
                    };
                }

                return new AuthResult
                {
                    message = "User is not found.",
                    success = false
                };
            }
            catch (Exception ex)
            {
                return new AuthResult
                {
                    message = ex.Message,
                    success = false
                };
            }
        }

        // external signup
        public async Task<AuthResult> ExternalSignUp(ExternalSignUp data)
        {
            // make token
            var (token, refreshToken, expireAt) = _jwtHelpers.MakeTokens(new MakeToken
            {
                Name = data.GivenName,
                Mail = data.Email,
                Role = data.Role,
                Length = 100
            });

            // user info
            var user = new Users()
            {
                user_name = data.GivenName,
                first_name = data.GivenName,
                user_mail = data.Email,
                is_admin = (data.Role == Roles.lead),
                given_name = data.GivenName,
                family_name = data.FamilyName,
                email_verified = data.EmailVerified,
                locale = data.Locale,
                picture = data.Picture,
                verify_token = token,
                expires_at = DateTime.Now.AddMinutes(expireAt),
                refresh_token = refreshToken,
                refresh_expires = DateTime.Now.AddMonths(1)
            };

            Context.users.Add(user);

            try
            {
                await Context.SaveChangesAsync();

                return new AuthResult
                {
                    message = "Ok",
                    success = true
                };
            }
            catch (Exception ex)
            {
                return new AuthResult
                {
                    message = ex.Message,
                    success = false
                };
            }
        }

    }
}