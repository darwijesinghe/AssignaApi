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
    /// Service implementation of ITeamMemberService.
    /// </summary>
    public class TeamMemberService : ITeamMemberService
    {
        // Services
        private readonly ITeamMemberRepository _teamMemberRepository;

        public TeamMemberService(ITeamMemberRepository teamMemberRepository)
        {
            _teamMemberRepository = teamMemberRepository;
        }

        /// <summary>
        /// Adds a note to a task.
        /// </summary>
        /// <param name="data">The data containing the task information and the note to be added.</param>
        /// <returns>
        /// A <see cref="Result"/> indicating the outcome of the add note operation.
        /// </returns>
        public async Task<Result> AddTaskNoteAsync(TaskDto data)
        {
            try
            {
                if (data is null)
                    return new Result { Message = "Required data not found.", Success = false };

                // user note data of task
                var task = new Task()
                {
                    UserNote = data.UserNote
                };

                // returns result
                return await _teamMemberRepository.AddTaskNoteAsync(task);
            }
            catch (Exception ex)
            {
                return new Result { Message = ex.Message, Success = false };
            }
        }

        /// <summary>
        /// Marks a task as done.
        /// </summary>
        /// <param name="data">The data containing the task identifier to be marked as done.</param>
        /// <returns>
        /// A a <see cref="Result"/> indicating the outcome of the mark as done operation.
        /// </returns>
        public async Task<Result> MarkasDoneAsync(MarkDoneDto data)
        {
            try
            {
                if (data is null)
                    return new Result { Message = "Required data not found.", Success = false };

                // user note data of task
                var task = new Task()
                {
                    TaskId = data.TaskId
                };

                // returns result
                return await _teamMemberRepository.MarkasDoneAsync(task);
            }
            catch (Exception ex)
            {
                return new Result { Message = ex.Message, Success = false };
            }
        }
    }
}
