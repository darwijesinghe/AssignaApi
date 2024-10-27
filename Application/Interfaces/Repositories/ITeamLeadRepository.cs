using Application.Response;
using System.Threading.Tasks;
using Task = Domain.Classes.Task;

namespace Application.Interfaces.Repositories
{
    /// <summary>
    /// Interface for team lead tasks db operations
    /// </summary>
    public interface ITeamLeadRepository
    {
        // <summary>
        /// Saves a task.
        /// </summary>
        /// <param name="data">The data containing the task information to be saved.</param>
        /// <returns>
        /// A <see cref="Result"/> indicating the outcome of the save operation.
        /// </returns>
        Task<Result> SaveTaskAsync(Task data);

        /// <summary>
        /// Edits an existing task.
        /// </summary>
        /// <param name="data">The data containing the updated task information.</param>
        /// <returns>
        /// A <see cref="Result"/> indicating the outcome of the edit operation.
        /// </returns>
        Task<Result> EditTaskAsync(Task data);

        /// <summary>
        /// Deletes a task.
        /// </summary>
        /// <param name="data">The data containing the task identifier to be deleted.</param>
        /// <returns>
        /// A <see cref="Result"/> indicating the outcome of the delete operation.
        /// </returns>
        Task<Result> DeleteTaskAsync(Task data);
    }
}
