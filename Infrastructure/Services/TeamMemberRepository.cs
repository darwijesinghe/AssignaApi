using Application.Interfaces.Repositories;
using Application.Response;
using Infrastructure.Data;
using System;
using System.Linq;
using System.Threading.Tasks;
using Task = Domain.Classes.Task;

namespace Infrastructure.Services
{
    /// <summary>
    /// Repository implementation for ITeamMemberRepository.
    /// </summary>
    public class TeamMemberRepository : ITeamMemberRepository
    {
        // Services
        private DataContext _context { get; }

        public TeamMemberRepository(DataContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Adds a user note to a task.
        /// </summary>
        /// <param name="data">The data containing the task information and the note to be added.</param>
        /// <returns>
        /// A <see cref="Result"/> indicating the outcome of the add note operation.
        /// </returns>
        public async Task<Result> AddTaskNoteAsync(Task data)
        {
            try
            {
                // gets task for the task id
                var task = _context.Task.FirstOrDefault(x => x.TaskId == data.TaskId);
                if (task is null)
                    return new Result { Message = "Task not found.", Success = false };

                // user note
                task.UserNote = data.UserNote;

                await _context.SaveChangesAsync();
                return new Result
                {
                    Message = "Ok.",
                    Success = true
                };
            }
            catch (Exception ex)
            {
                return new Result
                {
                    Message = ex.Message,
                    Success = false
                };
            }
        }

        /// <summary>
        /// Marks a task as done.
        /// </summary>
        /// <param name="data">The data containing the task identifier to be marked as done.</param>
        /// <returns>
        /// A a <see cref="Result"/> indicating the outcome of the mark as done operation.
        /// </returns>
        public async Task<Result> MarkasDoneAsync(Task data)
        {
            try
            {
                // gets task for the task id
                var task = _context.Task.FirstOrDefault(x => x.TaskId == data.TaskId);
                if (task is null)
                    return new Result { Message = "Task not found.", Success = false };

                // new values
                task.Pending  = false;
                task.Complete = true;

                await _context.SaveChangesAsync();
                return new Result
                {
                    Message = "Ok.",
                    Success = true
                };
            }
            catch (Exception ex)
            {
                return new Result
                {
                    Message = ex.Message,
                    Success = false
                };
            }
        }
    }
}
