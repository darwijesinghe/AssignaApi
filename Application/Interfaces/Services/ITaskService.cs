using Application.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Interfaces.Services
{
    /// <summary>
    /// Interface for common tasks operations.
    /// </summary>
    public interface ITaskService
    {
        /// <summary>
        /// Retrieves a list of team members.
        /// </summary>
        /// <returns>
        /// A list of <see cref="UserDto"/> representing the team members.
        /// </returns>
        Task<List<UserDto>> TeamMembers();

        /// <summary>
        /// Retrieves a list of all categories.
        /// </summary>
        /// <returns>
        /// A list of <see cref="CategoryDto"/> representing all categories.
        /// </returns>
        Task<List<CategoryDto>> AllCategories();

        /// <summary>
        /// Retrieves a list of all priority levels.
        /// </summary>
        /// <returns>
        /// A list of <see cref="PriorityDto"/> representing all priority levels.
        /// </returns>
        Task<List<PriorityDto>> Priorities();

        /// <summary>
        /// Retrieves a list of all tasks related data based on the user.
        /// </summary>
        /// <returns>
        /// A list of <see cref="TaskDto"/> containing all tasks.
        /// </returns>
        Task<List<TaskDto>> AllTasks();

        /// <summary>
        /// Retrieves a list of all pending tasks related data based on the user.
        /// </summary>
        /// <returns>
        /// A list of <see cref="TaskDto"/> containing all pending tasks.
        /// </returns>
        Task<List<TaskDto>> Pendings();

        /// <summary>
        /// Retrieves a list of all completed tasks related data based on the user.
        /// </summary>
        /// <returns>
        /// A list of <see cref="TaskDto"/> containing all completed tasks.
        /// </returns>
        Task<List<TaskDto>> Completed();

        /// <summary>
        /// Retrieves a list of all high priority tasks related data based on the user.
        /// </summary>
        /// <returns>
        /// A list of <see cref="TaskDto"/> containing all high priority tasks.
        /// </returns>
        Task<List<TaskDto>> HighPriority();

        /// <summary>
        /// Retrieves a list of all medium priority tasks related data based on the user.
        /// </summary>
        /// <returns>
        /// A list of <see cref="TaskDto"/> containing all medium priority tasks.
        /// </returns>
        Task<List<TaskDto>> MediumPriority();

        /// <summary>
        /// Retrieves a list of all low priority tasks related data based on the user.
        /// </summary>
        /// <returns>
        /// A list of <see cref="TaskDto"/> containing all low  priority tasks.
        /// </returns>
        Task<List<TaskDto>> LowPriority();

        /// <summary>
        /// Retrieves detailed information about a specific task based on the user.
        /// </summary>
        /// <param name="taskId">The identifier of the task for which to retrieve information.</param>
        /// <returns>
        /// A list of <see cref="TaskDto"/> containing the task information.
        /// </returns>
        Task<List<TaskDto>> TaskInfo(int taskId);
    }
}
