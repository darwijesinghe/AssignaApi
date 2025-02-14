using Application.DTOs;
using Application.Response;
using System.Threading.Tasks;

namespace Application.Interfaces.Services
{
    /// <summary>
    /// Interface for team lead tasks operations.
    /// </summary>
    public interface ITeamLeadService
    {
        /// <summary>
        /// Saves a task.
        /// </summary>
        /// <param name="data">The data containing the task information to be saved.</param>
        /// <returns>
        /// A <see cref="Result"/> indicating the outcome of the save operation.
        /// </returns>
        Task<Result> SaveTaskAsync(TaskDto data);

        /// <summary>
        /// Edits an existing task.
        /// </summary>
        /// <param name="data">The data containing the updated task information.</param>
        /// <returns>
        /// A <see cref="Result"/> indicating the outcome of the edit operation.
        /// </returns>
        Task<Result> EditTaskAsync(TaskDto data);

        /// <summary>
        /// Deletes a task.
        /// </summary>
        /// <param name="data">The data containing the task identifier to be deleted.</param>
        /// <returns>
        /// A <see cref="Result"/> indicating the outcome of the delete operation.
        /// </returns>
        Task<Result> DeleteTaskAsync(DeleteTaskDto data);
    }
}
