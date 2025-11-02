using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Application.Interfaces.Services;
using Application.DTOs;
using Application.Configurations;

namespace AssignaApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = Role.Member)]
    public class MemberTasksController : ControllerBase
    {
        // Services
        private readonly ITaskService       _taskService;
        private readonly ITeamMemberService _teamMemberService;

        public MemberTasksController(ITaskService taskService, ITeamMemberService teamMemberService)
        {
            _taskService       = taskService;
            _teamMemberService = teamMemberService;
        }

        /// <summary>
        /// Retrieves a list of all tasks related data based on the user.
        /// </summary>
        /// <returns>
        /// A <see cref="JsonResult"/> containing the task data.
        /// </returns>
        [HttpGet("tasks")]
        public async Task<JsonResult> Tasks()
        {
            // gets task data
            var result = await _taskService.AllTasks();

            return new JsonResult(new
            {
                message = "Ok.",
                success = true,
                data    = result
            });
        }

        /// <summary>
        /// Retrieves a list of pending tasks related data based on the user.
        /// </summary>
        /// <returns>
        /// A <see cref="JsonResult"/> containing the task data.
        /// </returns>
        [HttpGet("pendings")]
        public async Task<IActionResult> Pending()
        {
            // gets pending tasks
            var result = await _taskService.Pendings();

            return new JsonResult(new
            {
                message = "Ok.",
                success = true,
                data    = result
            });
        }

        /// <summary>
        /// Retrieves a list of completed tasks related data based on the user.
        /// </summary>
        /// <returns>
        /// A <see cref="JsonResult"/> containing the task data.
        /// </returns>
        [HttpGet("completes")]
        public async Task<IActionResult> Complete()
        {
            // gets completed tasks
            var result = await _taskService.Completed();

            return new JsonResult(new
            {
                message = "Ok.",
                success = true,
                data    = result
            });
        }

        /// <summary>
        /// Retrieves a list of high priority tasks related data based on the user.
        /// </summary>
        /// <returns>
        /// A <see cref="JsonResult"/> containing the task data.
        /// </returns>
        [HttpGet("high-priority")]
        public async Task<IActionResult> HighPriority()
        {
            // gets high priority tasks
            var result = await _taskService.HighPriority();

            return new JsonResult(new
            {
                message = "Ok.",
                success = true,
                data    = result
            });
        }

        /// <summary>
        /// Retrieves a list of medium priority tasks related data based on the user.
        /// </summary>
        /// <returns>
        /// A <see cref="JsonResult"/> containing the task data.
        /// </returns>
        [HttpGet("medium-priority")]
        public async Task<IActionResult> MediumPriority()
        {
            // gets medium priority tasks
            var result = await _taskService.MediumPriority();

            return new JsonResult(new
            {
                message = "Ok.",
                success = true,
                data    = result
            });
        }

        /// <summary>
        /// Retrieves a list of low priority tasks related data based on the user.
        /// </summary>
        /// <returns>
        /// A <see cref="JsonResult"/> containing the task data.
        /// </returns>
        [HttpGet("low-priority")]
        public async Task<IActionResult> LowPriority()
        {
            // gets low priority tasks
            var result = await _taskService.LowPriority();

            return new JsonResult(new
            {
                message = "Ok.",
                success = true,
                data    = result
            });
        }

        /// <summary>
        /// Retrieves a list of tasks related info based on the user.
        /// </summary>
        /// <returns>
        /// A <see cref="JsonResult"/> containing the task data.
        /// </returns>
        [HttpGet("task-info")]
        public async Task<IActionResult> TaskInfo(int taskId)
        {
            // gets tasks info
            var result = await _taskService.TaskInfo(taskId);
            if (!result.Any())
                return new JsonResult(new
                {
                    message = "Requested task is not found.",
                    success = false
                });

            return new JsonResult(new
            {
                message = "Ok.",
                success = true,
                data    = result
            });
        }

        /// <summary>
        /// Adds note for the specific task.
        /// </summary>
        /// <returns>
        /// A <see cref="JsonResult"/> containing the task data.
        /// </returns>
        [HttpPost("write-note")]
        public async Task<JsonResult> WriteNote([FromBody] AddNoteDto data)
        {
            // validations
            if (!ModelState.IsValid)
                return new JsonResult(new
                {
                    message = "Required data is not found.",
                    success = false
                });

            // checks the task is already completed or not
            var complete = await _taskService.TaskInfo(data.TaskId);
            if (complete.Any())
            {
                if (complete.First().Complete)
                {
                    return new JsonResult(new
                    {
                        message = "Task is already completed.",
                        success = false
                    });
                }
            }
            else
            {
                return new JsonResult(new
                {
                    message = "Task is not found to add a note.",
                    success = false
                });
            }

            // task note data
            var task = new TaskDto()
            {
                TaskId   = data.TaskId,
                UserNote = data.UserNote
            };

            // gets result
            var result = await _teamMemberService.AddTaskNoteAsync(task);
            if (result.Success)
                return new JsonResult(new
                {
                    message = "Ok.",
                    success = true
                });

            return new JsonResult(new
            {
                message = "Server error.",
                success = false
            });
        }

        /// <summary>
        /// Marks a specific task as done.
        /// </summary>
        /// <returns>
        /// A <see cref="JsonResult"/> containing the task data.
        /// </returns>
        [HttpPost("mark-done")]
        public async Task<JsonResult> MarkasDone([FromBody] MarkDoneDto data)
        {
            // validations
            if (!ModelState.IsValid)
                return new JsonResult(new
                {
                    message = "Required data is not found.",
                    success = false
                });

            // gets task info
            var task = await _taskService.TaskInfo(data.TaskId);
            if (!task.Any())
                return new JsonResult(new
                {
                    message = "Task is not found to mark as done.",
                    success = false
                });

            // gets result
            var result = await _teamMemberService.MarkasDoneAsync(data);
            if (result.Success)
                return new JsonResult(new
                {
                    message = "Ok.",
                    success = true
                });

            return new JsonResult(new
            {
                message = "Server error.",
                success = false
            });
        }
    }
}