using Application.Response;
using System.Threading.Tasks;
using Task = Domain.Classes.Task;

namespace Application.Interfaces.Repositories
{
    /// <summary>
    /// Interface for team member tasks db operations.
    /// </summary>
    public interface ITeamMemberRepository
    {
        /// <summary>
        /// Adds a note to a task.
        /// </summary>
        /// <param name="data">The data containing the task information and the note to be added.</param>
        /// <returns>
        /// A <see cref="Result"/> indicating the outcome of the add note operation.
        /// </returns>
        Task<Result> AddTaskNoteAsync(Task data);

        /// <summary>
        /// Marks a task as done.
        /// </summary>
        /// <param name="data">The data containing the task identifier to be marked as done.</param>
        /// <returns>
        /// A a <see cref="Result"/> indicating the outcome of the mark as done operation.
        /// </returns>
        Task<Result> MarkasDoneAsync(Task data);
    }
}
