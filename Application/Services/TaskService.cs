using Application.Configurations;
using Application.DTOs;
using Application.Helpers;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Task = Domain.Classes.Task;

namespace Application.Services
{
    /// <summary>
    /// Service implementation of ITaskService.
    /// </summary>
    public class TaskService : ITaskService
    {
        // Services
        private readonly ITaskRepository _taskRepository;

        public TaskService(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        /// <summary>
        /// Retrieves a list of team members.
        /// </summary>
        /// <returns>
        /// A list of <see cref="UserDto"/> representing the team members.
        /// </returns>
        public async Task<List<UserDto>> TeamMembers()
        {
            try
            {
                // gets member data
                var members = await _taskRepository.TeamMembers();

                if (!members.HasValue())
                    return new List<UserDto>();

                // returns converted data
                return members.Select(m => new UserDto
                {
                    UserId    = m.UserId,
                    UserName  = m.UserName,
                    FirstName = m.FirstName,
                    UserMail  = m.UserMail,
                    IsAdmin   = m.IsAdmin
                })
                .OrderBy(o => o.UserId)
                .ToList();
            }
            catch (Exception)
            {
                return new List<UserDto>();
            }
        }

        /// <summary>
        /// Retrieves a list of all categories.
        /// </summary>
        /// <returns>
        /// A list of <see cref="CategoryDto"/> representing all categories.
        /// </returns>
        public async Task<List<CategoryDto>> AllCategories()
        {
            try
            {
                // gets category data
                var categories = await _taskRepository.AllCategories();

                if (!categories.HasValue())
                    return new List<CategoryDto>();

                // returns converted data
                return categories.Select(c => new CategoryDto
                {
                    CatId   = c.CatId,
                    CatName = c.CatName
                })
                .OrderBy(o => o.CatId)
                .ToList();
            }
            catch (Exception)
            {
                return new List<CategoryDto>();
            }
        }

        /// <summary>
        /// Retrieves a list of all priority levels.
        /// </summary>
        /// <returns>
        /// A list of <see cref="PriorityDto"/> representing all priority levels.
        /// </returns>
        public async Task<List<PriorityDto>> Priorities()
        {
            try
            {
                // gets priority data
                var priorities = await _taskRepository.Priorities();

                if (!priorities.HasValue())
                    return new List<PriorityDto>();

                // returns converted data
                return priorities.Select(p => new PriorityDto
                {
                    PriorityId   = p.PriorityId,
                    PriorityName = p.PriorityName
                })
                .OrderBy(x => x.PriorityId)
                .ToList();
            }
            catch (Exception)
            {
                return new List<PriorityDto>();
            }
        }

        /// <summary>
        /// Retrieves a list of all tasks related data based on the user.
        /// </summary>
        /// <returns>
        /// A list of <see cref="TaskDto"/> containing all tasks.
        /// </returns>
        public async Task<List<TaskDto>> AllTasks()
        {
            try
            {
                // gets all task data
                var tasks = await _taskRepository.AllTasks();

                if (!tasks.HasValue())
                    return new List<TaskDto>();

                // returns converted data
                return tasks.Select(t => ConvertToTasksDto(t)).ToList();
            }
            catch (Exception)
            {
                return new List<TaskDto>();
            }
        }

        /// <summary>
        /// Retrieves a list of all pending tasks related data based on the user.
        /// </summary>
        /// <returns>
        /// A list of <see cref="TaskDto"/> containing all pending tasks.
        /// </returns>
        public async Task<List<TaskDto>> Pendings()
        {
            try
            {
                // gets all pending task data
                var tasks = await _taskRepository.Pendings();

                if (!tasks.HasValue())
                    return new List<TaskDto>();

                // returns converted data
                return tasks.Select(t => ConvertToTasksDto(t)).ToList();
            }
            catch (Exception)
            {
                return new List<TaskDto>();
            }
        }

        /// <summary>
        /// Retrieves a list of all completed tasks related data based on the user.
        /// </summary>
        /// <returns>
        /// A list of <see cref="TaskDto"/> containing all completed tasks.
        /// </returns>
        public async Task<List<TaskDto>> Completed()
        {
            try
            {
                // gets all completed task data
                var tasks = await _taskRepository.Completed();

                if (!tasks.HasValue())
                    return new List<TaskDto>();

                // returns converted data
                return tasks.Select(t => ConvertToTasksDto(t)).ToList();
            }
            catch (Exception)
            {
                return new List<TaskDto>();
            }
        }

        /// <summary>
        /// Retrieves a list of all high priority tasks related data based on the user.
        /// </summary>
        /// <returns>
        /// A list of <see cref="TaskDto"/> containing all high priority tasks.
        /// </returns>
        public async Task<List<TaskDto>> HighPriority()
        {
            try
            {
                // gets all high priority task data
                var tasks = await _taskRepository.HighPriority();

                if (!tasks.HasValue())
                    return new List<TaskDto>();

                // returns converted data
                return tasks.Select(t => ConvertToTasksDto(t)).ToList();
            }
            catch (Exception)
            {
                return new List<TaskDto>();
            }
        }

        /// <summary>
        /// Retrieves a list of all medium priority tasks related data based on the user.
        /// </summary>
        /// <returns>
        /// A list of <see cref="TaskDto"/> containing all medium priority tasks.
        /// </returns>
        public async Task<List<TaskDto>> MediumPriority()
        {
            try
            {
                // gets all medium priority task data
                var tasks = await _taskRepository.MediumPriority();

                if (!tasks.HasValue())
                    return new List<TaskDto>();

                // returns converted data
                return tasks.Select(t => ConvertToTasksDto(t)).ToList();
            }
            catch (Exception)
            {
                return new List<TaskDto>();
            }
        }

        /// <summary>
        /// Retrieves a list of all low priority tasks related data based on the user.
        /// </summary>
        /// <returns>
        /// A list of <see cref="TaskDto"/> containing all low  priority tasks.
        /// </returns>
        public async Task<List<TaskDto>> LowPriority()
        {
            try
            {
                // gets all low priority task data
                var tasks = await _taskRepository.LowPriority();

                if (!tasks.HasValue())
                    return new List<TaskDto>();

                // returns converted data
                return tasks.Select(t => ConvertToTasksDto(t)).ToList();
            }
            catch (Exception)
            {
                return new List<TaskDto>();
            }
        }

        /// <summary>
        /// Retrieves detailed information about a specific task based on the user.
        /// </summary>
        /// <param name="taskId">The identifier of the task for which to retrieve information.</param>
        /// <returns>
        /// A list of <see cref="TaskDto"/> containing the task information.
        /// </returns>
        public async Task<List<TaskDto>> TaskInfo(int taskId)
        {
            try
            {
                if (taskId <= 0)
                    return new List<TaskDto>();

                // gets task info data
                var info = await _taskRepository.TaskInfo(taskId);

                if (!info.HasValue())
                    return new List<TaskDto>();

                // returns converted data
                return info.Select(t => ConvertToTasksDto(t)).ToList();
            }
            catch (Exception)
            {
                return new List<TaskDto>();
            }
        }

        // Helpers ------------------------------------------

        /// <summary>
        /// Converts a single <see cref="Task"/> entity to a <see cref="TaskDto"/> object. Maps all 
        /// relevant properties from the <see cref="Task"/> entity to the corresponding
        /// properties in <see cref="TaskDto"/>.
        /// </summary>
        /// <param name="task">The <see cref="Task"/> entity to be converted.</param>
        /// <returns>
        /// A <see cref="TaskDto"/> object containing the mapped data.
        /// </returns>
        private TaskDto ConvertToTasksDto(Task task)
        {
            if (task is null)
                return new TaskDto();

            // returns converted list
            return new TaskDto
            {
                TaskId         = task.TaskId,
                TaskTitle      = task.TaskTitle,
                Deadline       = task.Deadline,
                TaskNote       = task.TaskNote,
                Pending        = task.Pending,
                Complete       = task.Complete,
                HighPriority   = task.HighPriority,
                MediumPriority = task.MediumPriority,
                LowPriority    = task.LowPriority,
                UserNote       = task.UserNote,
                CatId          = task.Category.CatId,
                CatName        = task.Category.CatName,
                UserId         = task.UserId,
                UserName       = task.User.UserName,
                FirstName      = task.User.FirstName,
                UserMail       = task.User.UserMail
            };
        }
    }
}
