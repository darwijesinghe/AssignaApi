using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AssignaApi.Data;
using AssignaApi.Helpers;
using AssignaApi.Interfaces;
using AssignaApi.Models;
using AssignaApi.Response;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Task = AssignaApi.Models.Task;

namespace AssignaApi.Services
{
    public class DataService : IDataService
    {
        // variables
        private const string empty = "";

        // services
        private DataContext _context { get; }
        private readonly Helper _helper;
        private readonly JwtHelpers _jwtHelpers;
        private readonly JwtConfig _jwtConfig;
        public DataService(DataContext context, Helper helper,
        JwtHelpers jwtHelpers, IOptions<JwtConfig> jwtConfig)
        {
            _context = context;
            _helper = helper;
            _jwtHelpers = jwtHelpers;
            _jwtConfig = jwtConfig.Value;
        }

        // new user register
        public async Task<AuthResult> UserRegisterAsync(UserRegister data)
        {
            // password hash
            _helper.PasswordHash(
                data.password,
                out byte[] passwordHash,
                out byte[] passwordSalt
            );

            // generate the token
            string jwtToken = _jwtHelpers.GenerateJwtToken
            (
                name: data.user_name,
                mail: data.email,
                role: data.role
            );

            // refresh token
            string refToken = _jwtHelpers.GenerateRandomToken(100);

            // expire time
            var minutes = TimeSpan.Parse(_jwtConfig.Expire).Minutes;

            var user = new Users()
            {
                user_name = data.user_name ?? empty,
                first_name = data.first_name ?? empty,
                user_mail = data.email ?? empty,
                password_hash = passwordHash,
                password_salt = passwordSalt,
                is_admin = (data.role == Roles.lead) ? true : false,
                verify_token = jwtToken,
                expires_at = DateTime.Now.AddMinutes(minutes),
                refresh_token = refToken,
                refresh_expires = DateTime.Now.AddMonths(1)
            };

            _context.users.Add(user);

            try
            {
                await _context.SaveChangesAsync();

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
            var users = _context.users
            .Select(x => new UsersDto
            {
                user_id = x.user_id,
                user_name = x.user_name,
                first_name = x.first_name,
                user_mail = x.user_mail,
                password_hash = x.password_hash,
                password_salt = x.password_salt,
                verify_token = x.verify_token,
                expires_at = x.expires_at,
                refresh_token = x.refresh_token,
                refresh_expires = x.refresh_expires,
                reset_token = x.reset_token,
                reset_expires = x.reset_expires,
                is_admin = x.is_admin
            })
            .OrderBy(x => x.user_id)
            .ToList();

            return users;
        }

        // save password reset token
        public async Task<AuthResult> ForgotTokenAsync(ForgotPassword data)
        {
            // user retrieve
            var user = _context.users.FirstOrDefault
            (
                x => x.user_mail == data.email
            );

            user.reset_token = _jwtHelpers.GenerateRandomToken(100);
            user.reset_expires = DateTime.Now.AddDays(1);

            try
            {
                await _context.SaveChangesAsync();
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
            var user = _context.users.FirstOrDefault
            (
                x => x.reset_token == data.reset_token
            );

            // password hash
            _helper.PasswordHash(
                data.password,
                out byte[] passwordHash,
                out byte[] passwordSalt
            );

            user.reset_token = null;
            user.reset_expires = null;
            user.password_hash = passwordHash;
            user.password_salt = passwordSalt;

            try
            {
                await _context.SaveChangesAsync();

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
            var user = _context.users.FirstOrDefault
            (
                x => x.refresh_token == data.refresh_token
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
                await _context.SaveChangesAsync();

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
            var categories = _context.category
            .Select(x => new CategoryDto
            {
                cat_id = x.cat_id,
                cat_name = x.cat_name
            })
            .OrderBy(x => x.cat_id)
            .ToList();

            return categories;
        }

        // get team members
        public List<UsersDto> TeamMembers()
        {
            var members = _context.users
            .Select(x => new UsersDto
            {
                user_id = x.user_id,
                user_name = x.user_name,
                first_name = x.first_name,
                user_mail = x.user_mail,
                is_admin = x.is_admin
            })
            .Where(x => x.is_admin == false)
            .OrderBy(x => x.user_id)
            .ToList();

            return members;
        }

        // get priorities
        public List<PriorityDto> Priorities()
        {
            var priorities = _context.priority
            .Select(x => new PriorityDto
            {
                pri_id = x.pri_id,
                pri_name = x.pri_name
            })
            .OrderBy(x => x.pri_id)
            .ToList();

            return priorities;
        }

        // add a new task
        public async Task<Result> SaveTaskAsync(TaskDto data)
        {
            var task = new Task()
            {
                tsk_title = data.tsk_title,
                deadline = data.deadline,
                tsk_note = data.tsk_note,
                pri_high = data.pri_high,
                pri_medium = data.pri_medium,
                pri_low = data.pri_low,
                cat_id = data.cat_id,
                user_id = data.user_id,
                pending = data.pending
            };

            _context.task.Add(task);

            try
            {
                await _context.SaveChangesAsync();
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
            var task = _context.task.FirstOrDefault(x => x.tsk_id == data.tsk_id);

            if (task is not null)
            {
                task.tsk_title = data.tsk_title;
                task.cat_id = data.cat_id;
                task.deadline = data.deadline;
                task.pri_high = data.pri_high;
                task.pri_medium = data.pri_medium;
                task.pri_low = data.pri_low;
                task.user_id = data.user_id;
                task.tsk_note = data.tsk_note;
            }

            try
            {
                await _context.SaveChangesAsync();
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
            var task = _context.task.FirstOrDefault(x => x.tsk_id == data.tsk_id);
            _context.task.Remove(task);

            try
            {
                await _context.SaveChangesAsync();
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
            var taks = await (from us in _context.users
                              join tk in _context.task on us.user_id equals tk.users.user_id
                              join ct in _context.category on tk.category.cat_id equals ct.cat_id
                              select new TaskDto
                              {
                                  tsk_id = tk.tsk_id,
                                  tsk_title = tk.tsk_title,
                                  deadline = tk.deadline,
                                  tsk_note = tk.tsk_note,
                                  pending = tk.pending,
                                  complete = tk.complete,
                                  pri_high = tk.pri_high,
                                  pri_medium = tk.pri_medium,
                                  pri_low = tk.pri_low,
                                  user_note = tk.user_note,
                                  cat_id = tk.cat_id,
                                  user_id = tk.user_id,
                                  first_name = us.first_name,
                                  user_mail = us.user_mail,
                                  cat_name = ct.cat_name

                              }).ToListAsync();

            return taks
            .Where(x => x.tsk_id == taskId)
            .OrderBy(x => x.tsk_id).ToList();
        }

        // all tasks
        public async Task<List<TaskDto>> AllTasks()
        {
            var taks = await this.TaskData();
            return taks.OrderBy(x => x.tsk_id).ToList();
        }

        // pending tasks
        public async Task<List<TaskDto>> Pendings()
        {
            var taks = await this.TaskData();
            return taks.Where(x => x.pending == true)
            .OrderBy(x => x.tsk_id)
            .ToList();
        }

        // completed tasks
        public async Task<List<TaskDto>> Completed()
        {
            var taks = await this.TaskData();
            return taks.Where(x => x.complete == true)
            .OrderBy(x => x.tsk_id)
            .ToList();
        }

        // high priority tasks
        public async Task<List<TaskDto>> HighPriority()
        {
            var taks = await this.TaskData();
            return taks.Where(x => x.pri_high == true)
            .OrderBy(x => x.tsk_id)
            .ToList();
        }

        // medium priority tasks
        public async Task<List<TaskDto>> MediumPriority()
        {
            var taks = await this.TaskData();
            return taks.Where(x => x.pri_medium == true)
            .OrderBy(x => x.tsk_id)
            .ToList();
        }

        // low priority tasks
        public async Task<List<TaskDto>> LowPriority()
        {
            var taks = await this.TaskData();
            return taks.Where(x => x.pri_low == true)
            .OrderBy(x => x.tsk_id)
            .ToList();
        }

        #region team member

        // add task note
        public async Task<Result> AddTaskNoteAsync(TaskDto data)
        {
            var task = _context.task.FirstOrDefault(x => x.tsk_id == data.tsk_id);
            if (task is not null)
            {
                task.user_note = data.user_note;
            }

            try
            {
                await _context.SaveChangesAsync();
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
            var task = _context.task.FirstOrDefault(x => x.tsk_id == data.tsk_id);
            if (task is not null)
            {
                task.pending = false;
                task.complete = true;
            }

            try
            {
                await _context.SaveChangesAsync();
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
            var taks = await (from us in _context.users
                              join tk in _context.task on us.user_id equals tk.users.user_id
                              select new TaskDto
                              {
                                  tsk_id = tk.tsk_id,
                                  tsk_title = tk.tsk_title,
                                  deadline = tk.deadline,
                                  tsk_note = tk.tsk_note,
                                  pending = tk.pending,
                                  complete = tk.complete,
                                  pri_high = tk.pri_high,
                                  pri_medium = tk.pri_medium,
                                  pri_low = tk.pri_low,
                                  user_note = tk.user_note,
                                  cat_id = tk.cat_id,
                                  user_id = tk.user_id,
                                  user_name = us.user_name,
                                  first_name = us.first_name

                              }).ToListAsync();

            // get user name and user role
            var claims = _jwtHelpers.ReadJwtToken();

            if (claims.role == Roles.member)
            {
                // filter by member name
                var filterdTasks = taks.Where
                (
                    x => x.user_name == claims.uesr_name
                );

                return filterdTasks.ToList();
            }

            return taks;
        }

    }
}