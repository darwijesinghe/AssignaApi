using Application.DTOs;
using Application.Response;
using System.Threading.Tasks;

namespace Application.Interfaces.Services
{
    /// <summary>
    /// Interface for team member tasks operations
    /// </summary>
    public interface ITeamMemberService
    {
        /// <summary>
        /// Adds a note to a task.
        /// </summary>
        /// <param name="data">The data containing the task information and the note to be added.</param>
        /// <returns>
        /// A <see cref="Result"/> indicating the outcome of the add note operation.
        /// </returns>
        Task<Result> AddTaskNoteAsync(TaskDto data);

        /// <summary>
        /// Marks a task as done.
        /// </summary>
        /// <param name="data">The data containing the task identifier to be marked as done.</param>
        /// <returns>
        /// A a <see cref="Result"/> indicating the outcome of the mark as done operation.
        /// </returns>
        Task<Result> MarkasDoneAsync(MarkDoneDto data);
    }
}
