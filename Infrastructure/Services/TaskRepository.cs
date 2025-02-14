using Application.Configurations;
using Application.Helpers;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Domain.Classes;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Task = Domain.Classes.Task;

namespace Infrastructure.Services
{
    /// <summary>
    /// Repository implementation for ITaskRepository.
    /// </summary>
    public class TaskRepository : ITaskRepository
    {
        // Services
        private DataContext          _context { get; }
        private readonly IJwtService _jwtService;

        public TaskRepository(DataContext context, IJwtService jwtService)
        {
            _context    = context;
            _jwtService = jwtService;
        }

        /// <summary>
        /// Retrieves a collection of team members.
        /// </summary>
        /// <returns>
        /// A collection of <see cref="User"/> representing the team members.
        /// </returns>
        public async Task<IEnumerable<User>> TeamMembers()
        {
            try
            {
                // returns all users
                return await _context.User.Where(x => !x.IsAdmin).OrderBy(o => o.UserId).ToListAsync();
            }
            catch (Exception ex)
            {
                return Enumerable.Empty<User>();
            }
        }

        /// <summary>
        /// Retrieves a collection of all categories.
        /// </summary>
        /// <returns>
        /// A collection of <see cref="Category"/> representing all categories.
        /// </returns>
        public async Task<IEnumerable<Category>> AllCategories()
        {
            try
            {
                // returns all categories
                return await _context.Category.OrderBy(o => o.CatId).ToListAsync();
            }
            catch (Exception ex)
            {
                return Enumerable.Empty<Category>();
            }
        }

        /// <summary>
        /// Retrieves a collection of all priority levels.
        /// </summary>
        /// <returns>
        /// A collection of <see cref="Priority"/> representing all priority levels.
        /// </returns>
        public async Task<IEnumerable<Priority>> Priorities()
        {
            try
            {
                // returns all priorities
                return await _context.Priority.OrderBy(o => o.PriorityId).ToListAsync();
            }
            catch (Exception ex)
            {
                return Enumerable.Empty<Priority>();
            }
        }

        /// <summary>
        /// Retrieves a collection of all tasks related data based on the user.
        /// </summary>
        /// <returns>
        /// A collection of <see cref="Task"/> containing all tasks.
        /// </returns>
        public async Task<IEnumerable<Task>> AllTasks()
        {
            try
            {
                // returns all tasks
                return await MakeTask();

            }
            catch (Exception ex)
            {
                return Enumerable.Empty<Task>();
            }
        }

        /// <summary>
        /// Retrieves a collection of all pending tasks related data based on the user.
        /// </summary>
        /// <returns>
        /// A collection of <see cref="Task"/> containing all pending tasks.
        /// </returns>
        public async Task<IEnumerable<Task>> Pendings()
        {
            try
            {
                // gets all tasks
                var tasks = await MakeTask();

                // returns all pending tasks
                return tasks.Where(x => x.Pending).OrderBy(o => o.TaskId);
            }
            catch (Exception ex)
            {
                return Enumerable.Empty<Task>();
            }
        }

        /// <summary>
        /// Retrieves a collection of all completed tasks related data based on the user.
        /// </summary>
        /// <returns>
        /// A collection of <see cref="Task"/> containing all completed tasks.
        /// </returns>
        public async Task<IEnumerable<Task>> Completed()
        {
            try
            {
                // gets all tasks
                var tasks = await MakeTask();

                // returns all completed tasks
                return tasks.Where(x => x.Complete).OrderBy(o => o.TaskId);
            }
            catch (Exception ex)
            {
                return Enumerable.Empty<Task>();
            }
        }

        /// <summary>
        /// Retrieves a collection of all high priority tasks related data based on the user.
        /// </summary>
        /// <returns>
        /// A collection of <see cref="Task"/> containing all high priority tasks.
        /// </returns>
        public async Task<IEnumerable<Task>> HighPriority()
        {
            try
            {
                // gets all tasks
                var tasks = await MakeTask();

                // returns all high priority tasks
                return tasks.Where(x => x.HighPriority).OrderBy(o => o.TaskId);
            }
            catch (Exception ex)
            {
                return Enumerable.Empty<Task>();
            }
        }

        /// <summary>
        /// Retrieves a collection of all medium priority tasks related data based on the user.
        /// </summary>
        /// <returns>
        /// A collection of <see cref="Task"/> containing all medium priority tasks.
        /// </returns>
        public async Task<IEnumerable<Task>> MediumPriority()
        {
            try
            {
                // gets all tasks
                var tasks = await MakeTask();

                // returns all medium priority tasks
                return tasks.Where(x => x.MediumPriority).OrderBy(o => o.TaskId);
            }
            catch (Exception ex)
            {
                return Enumerable.Empty<Task>();
            }
        }

        /// <summary>
        /// Retrieves a collection of all low priority tasks related data based on the user.
        /// </summary>
        /// <returns>
        /// A collection of <see cref="Task"/> containing all low  priority tasks.
        /// </returns>
        public async Task<IEnumerable<Task>> LowPriority()
        {
            try
            {
                // gets all tasks
                var tasks = await MakeTask();

                // returns all low priority tasks
                return tasks.Where(x => x.LowPriority).OrderBy(o => o.TaskId);
            }
            catch (Exception ex)
            {
                return Enumerable.Empty<Task>();
            }
        }

        /// <summary>
        /// Retrieves detailed information about a specific task based on the user.
        /// </summary>
        /// <param name="taskId">The identifier of the task for which to retrieve information.</param>
        /// <returns>
        /// A collection of <see cref="Task"/> containing the task information.
        /// </returns>
        public async Task<IEnumerable<Task>> TaskInfo(int taskId)
        {
            try
            {
                // gets all tasks
                var tasks = await MakeTask();

                // returns task for the task id
                return tasks.Where(x => x.TaskId == taskId).OrderBy(o => o.TaskId);
            }
            catch (Exception ex)
            {
                return Enumerable.Empty<Task>();
            }
        }

        // Helper methods ---------------------------------------

        /// <summary>
        /// Returns all tasks based on the user role type.
        /// </summary>
        /// <returns>
        /// A collection of <see cref="Task"/> containing all tasks.
        /// </returns>
        private async Task<IEnumerable<Task>> MakeTask()
        {
            try
            {
                // retrieves all tasks
                var tasks = await _context.Task.OrderBy(o => o.TaskId).ToListAsync();
                if (!tasks.HasValue())
                    return Enumerable.Empty<Task>();

                // gets user claims
                var claims = _jwtService.ReadJwtToken();

                // filters member tasks
                if (claims.Role == Role.Member)
                    return tasks.Where(x => x.User.UserName == claims.UserName);

                // returns all tasks
                return tasks;

            }
            catch (Exception ex)
            {
                return Enumerable.Empty<Task>();
            }
        }
    }
}
