using Domain.Classes;
using System.Collections.Generic;
using System.Threading.Tasks;
using Task = Domain.Classes.Task;

namespace Application.Interfaces.Repositories
{
    /// <summary>
    /// Interface for common tasks db operations
    /// </summary>
    public interface ITaskRepository
    {
        /// <summary>
        /// Retrieves a collection of team members.
        /// </summary>
        /// <returns>
        /// A collection of <see cref="User"/> representing the team members.
        /// </returns>
        Task<IEnumerable<User>> TeamMembers();

        /// <summary>
        /// Retrieves a collection of all categories.
        /// </summary>
        /// <returns>
        /// A collection of <see cref="Category"/> representing all categories.
        /// </returns>
        Task<IEnumerable<Category>> AllCategories();

        /// <summary>
        /// Retrieves a collection of all priority levels.
        /// </summary>
        /// <returns>
        /// A collection of <see cref="Priority"/> representing all priority levels.
        /// </returns>
        Task<IEnumerable<Priority>> Priorities();

        /// <summary>
        /// Retrieves a collection of all tasks related data based on the user.
        /// </summary>
        /// <returns>
        /// A collection of <see cref="Task"/> containing all tasks.
        /// </returns>
        Task<IEnumerable<Task>> AllTasks();

        /// <summary>
        /// Retrieves a collection of all pending tasks related data based on the user.
        /// </summary>
        /// <returns>
        /// A collection of <see cref="Task"/> containing all pending tasks.
        /// </returns>
        Task<IEnumerable<Task>> Pendings();

        /// <summary>
        /// Retrieves a collection of all completed tasks related data based on the user.
        /// </summary>
        /// <returns>
        /// A collection of <see cref="Task"/> containing all completed tasks.
        /// </returns>
        Task<IEnumerable<Task>> Completed();

        /// <summary>
        /// Retrieves a collection of all high priority tasks related data based on the user.
        /// </summary>
        /// <returns>
        /// A collection of <see cref="Task"/> containing all high priority tasks.
        /// </returns>
        Task<IEnumerable<Task>> HighPriority();

        /// <summary>
        /// Retrieves a collection of all medium priority tasks related data based on the user.
        /// </summary>
        /// <returns>
        /// A collection of <see cref="Task"/> containing all medium priority tasks.
        /// </returns>
        Task<IEnumerable<Task>> MediumPriority();

        /// <summary>
        /// Retrieves a collection of all low priority tasks related data based on the user.
        /// </summary>
        /// <returns>
        /// A collection of <see cref="Task"/> containing all low  priority tasks.
        /// </returns>
        Task<IEnumerable<Task>> LowPriority();

        /// <summary>
        /// Retrieves detailed information about a specific task based on the user.
        /// </summary>
        /// <param name="taskId">The identifier of the task for which to retrieve information.</param>
        /// <returns>
        /// A collection of <see cref="Task"/> containing the task information.
        /// </returns>
        Task<IEnumerable<Task>> TaskInfo(int taskId);
    }
}
