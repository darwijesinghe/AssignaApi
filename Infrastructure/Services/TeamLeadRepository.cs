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
    /// Repository implementation for ITeamLeadRepository.
    /// </summary>
    public class TeamLeadRepository : ITeamLeadRepository
    {
        // Services
        private DataContext _context { get; }

        public TeamLeadRepository(DataContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Saves a task.
        /// </summary>
        /// <param name="data">The data containing the task information to be saved.</param>
        /// <returns>
        /// A <see cref="Result"/> indicating the outcome of the save operation.
        /// </returns>
        public async Task<Result> SaveTaskAsync(Task data)
        {
            try
            {
                // adds data to context
                await _context.Task.AddAsync(data);

                // saves changes to the context
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
        /// Edits an existing task.
        /// </summary>
        /// <param name="data">The data containing the updated task information.</param>
        /// <returns>
        /// A <see cref="Result"/> indicating the outcome of the edit operation.
        /// </returns>
        public async Task<Result> EditTaskAsync(Task data)
        {
            try
            {
                // gets task for the task id
                var task = _context.Task.FirstOrDefault(x => x.TaskId == data.TaskId);
                if (task is null)
                    return new Result { Message = "Task not found.", Success = false };

                // new values
                task.TaskTitle      = data.TaskTitle;
                task.CatId          = data.CatId;
                task.Deadline       = data.Deadline;
                task.HighPriority   = data.HighPriority;
                task.MediumPriority = data.MediumPriority;
                task.LowPriority    = data.LowPriority;
                task.UserId         = data.UserId;
                task.TaskNote       = data.TaskNote;

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
        /// Deletes a task.
        /// </summary>
        /// <param name="data">The data containing the task identifier to be deleted.</param>
        /// <returns>
        /// A <see cref="Result"/> indicating the outcome of the delete operation.
        /// </returns>
        public async Task<Result> DeleteTaskAsync(Task data)
        {
            try
            {
                // gets task for the task id
                var task = _context.Task.FirstOrDefault(x => x.TaskId == data.TaskId);
                if (task is null)
                    return new Result { Message = "Task not found.", Success = false };

                // removes the task
                _context.Task.Remove(task);

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
