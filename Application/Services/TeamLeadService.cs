using Application.Configurations;
using Application.DTOs;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Application.Response;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;
using Task = Domain.Classes.Task;

namespace Application.Services
{
    /// <summary>
    /// Service implementation of ITeamLeadService.
    /// </summary>
    public class TeamLeadService : ITeamLeadService
    {
        // Services
        private readonly ITeamLeadRepository _teamLeadRepository;

        public TeamLeadService(ITeamLeadRepository teamLeadRepository)
        {
            _teamLeadRepository = teamLeadRepository;
        }

        /// <summary>
        /// Saves a task.
        /// </summary>
        /// <param name="data">The data containing the task information to be saved.</param>
        /// <returns>
        /// A <see cref="Result"/> indicating the outcome of the save operation.
        /// </returns>
        public async Task<Result> SaveTaskAsync(TaskDto data)
        {
            try
            {
                if (data is null)
                    return new Result { Message = "Required data not found.", Success = false };

                // task data
                var task = new Task()
                {
                    TaskTitle      = data.TaskTitle,
                    Deadline       = data.Deadline,
                    TaskNote       = data.TaskNote,
                    HighPriority   = data.HighPriority,
                    MediumPriority = data.MediumPriority,
                    LowPriority    = data.LowPriority,
                    CatId          = data.CatId,
                    UserId         = data.UserId,
                    Pending        = data.Pending
                };

                // returns result
                return await _teamLeadRepository.SaveTaskAsync(task);
            }
            catch (Exception ex)
            {
                return new Result { Message = ex.Message, Success = false };
            }
        }

        /// <summary>
        /// Edits an existing task.
        /// </summary>
        /// <param name="data">The data containing the updated task information.</param>
        /// <returns>
        /// A <see cref="Result"/> indicating the outcome of the edit operation.
        /// </returns>
        public async Task<Result> EditTaskAsync(TaskDto data)
        {
            try
            {
                if (data is null)
                    return new Result { Message = "Required data not found.", Success = false };

                // task data
                var task = new Task()
                {
                    TaskId         = data.TaskId,
                    TaskTitle      = data.TaskTitle,
                    CatId          = data.CatId,
                    Deadline       = data.Deadline,
                    HighPriority   = data.HighPriority,
                    MediumPriority = data.MediumPriority,
                    LowPriority    = data.LowPriority,
                    UserId         = data.UserId,
                    TaskNote       = data.TaskNote
                };

                // returns result
                return await _teamLeadRepository.EditTaskAsync(task);

            }
            catch (Exception ex)
            {
                return new Result { Message = ex.Message, Success = false };
            }
        }

        /// <summary>
        /// Deletes a task.
        /// </summary>
        /// <param name="data">The data containing the task identifier to be deleted.</param>
        /// <returns>
        /// A <see cref="Result"/> indicating the outcome of the delete operation.
        /// </returns>
        public async Task<Result> DeleteTaskAsync(DeleteTaskDto data)
        {
            try
            {
                if (data is null)
                    return new Result { Message = "Required data not found.", Success = false };

                // task data
                var task = new Task()
                {
                    TaskId = data.TaskId
                };

                // returns result
                return await _teamLeadRepository.DeleteTaskAsync(task);
            }
            catch (Exception ex)
            {
                return new Result { Message = ex.Message, Success = false };
            }
        }
    }
}
