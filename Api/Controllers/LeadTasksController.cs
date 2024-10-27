using System;
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
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = Role.Lead)]
    public class LeadTasksController : ControllerBase
    {
        // Services
        private readonly ITaskService     _taskService;
        private readonly ITeamLeadService _teamLeadService;
        private IMailService              _mailService;

        public LeadTasksController(ITeamLeadService teamLeadService, ITaskService taskService, IMailService mailService)
        {
            _teamLeadService = teamLeadService;
            _taskService     = taskService;
            _mailService     = mailService;
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
            // gets all tasks
            var result = await _taskService.AllTasks();

            return new JsonResult(new
            {
                data = result
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
                data = result
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
                data = result
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
                data = result
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
                data = result
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
                data = result
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
                data = result
            });
        }

        /// <summary>
        /// Adds a new task to the database.
        /// </summary>
        /// <param name="data">The <see cref="NewTaskDto"/> object that contains the task data.</param>
        /// <returns>
        /// A <see cref="JsonResult"/> containing the execution result.
        /// </returns>
        [HttpPost("add-task")]
        public async Task<JsonResult> AddTask([FromBody] NewTaskDto data)
        {
            // validations
            if (!ModelState.IsValid)
                return new JsonResult(new
                {
                    message = "Required data is not found.",
                    success = false
                });

            // checks the category
            var category = await _taskService.AllCategories();
            if (!category.Any(x => x.CatId == data.TaskCategory))
                return new JsonResult(new
                {
                    message = "Category id is not valid.",
                    success = false
                });

            // checks the assignee
            var member = await _taskService.TeamMembers();
            if (!member.Any(x => x.UserId == data.Member))
                return new JsonResult(new
                {
                    message = "Member id is not valid.",
                    success = false
                });


            // new task data
            var task = new TaskDto()
            {
                TaskTitle      = data.TaskTitle,
                Deadline       = Convert.ToDateTime(data.Deadline),
                TaskNote       = data.TaskNote,
                HighPriority   = (data.Priority == PriorityLevel.High),
                MediumPriority = (data.Priority == PriorityLevel.Medium),
                LowPriority    = (data.Priority == PriorityLevel.Low),
                CatId          = data.TaskCategory,
                UserId         = data.Member,
                Pending        = true
            };

            // gets result
            var result = await _teamLeadService.SaveTaskAsync(task);
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
        /// Edits a existing task in the database.
        /// </summary>
        /// <param name="data">The <see cref="EditTaskDto"/> object that contains the task data.</param>
        /// <returns>
        /// A <see cref="JsonResult"/> containing the execution result.
        /// </returns>
        [HttpPost("edit-task")]
        public async Task<JsonResult> EditTask([FromBody] EditTaskDto data)
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
                    return new JsonResult(new
                    {
                        message = "Task is already completed.",
                        success = false
                    });
            }
            else
            {
                return new JsonResult(new
                {
                    message = "Task is not found to edit.",
                    success = false
                });
            }

            // checks the category
            var category = await _taskService.AllCategories();
            if (!category.Any(x => x.CatId == data.TaskCategory))
                return new JsonResult(new
                {
                    message = "Category id is not valid.",
                    success = false
                });

            // checks the assignee
            var member = await _taskService.TeamMembers();
            if (!member.Any(x => x.UserId == data.Member))
                return new JsonResult(new
                {
                    message = "Member id is not valid.",
                    success = false
                });

            // task data
            var task = new TaskDto()
            {
                TaskId         = data.TaskId,
                TaskTitle      = data.TaskTitle,
                Deadline       = Convert.ToDateTime(data.Deadline),
                TaskNote       = data.TaskNote,
                HighPriority   = (data.Priority == PriorityLevel.High),
                MediumPriority = (data.Priority == PriorityLevel.Medium),
                LowPriority    = (data.Priority == PriorityLevel.Low),
                CatId          = data.TaskCategory,
                UserId         = data.Member,
                Pending        = true
            };

            // gets the result
            var result = await _teamLeadService.EditTaskAsync(task);
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
        /// Deletes a existing task in the database.
        /// </summary>
        /// <param name="data">The <see cref="DeleteTaskDto"/> object that contains the task data.</param>
        /// <returns>
        /// A <see cref="JsonResult"/> containing the execution result.
        /// </returns>
        [HttpPost("delete-task")]
        public async Task<JsonResult> DeleteTask([FromBody] DeleteTaskDto data)
        {
            // validations
            if (!ModelState.IsValid)
                return new JsonResult(new
                {
                    message = "Required data is not found.",
                    success = false
                });

            // gets the task info
            var task = await _taskService.TaskInfo(data.TaskId);
            if (!task.Any())
                return new JsonResult(new
                {
                    message = "Task is not found to delete.",
                    success = false
                });

            // gets the result
            var result = await _teamLeadService.DeleteTaskAsync(data);
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
        /// Sends remind to the task assignee.
        /// </summary>
        /// <param name="data">The <see cref="ReminderDto"/> object that contains the email data.</param>
        /// <returns>
        /// A <see cref="JsonResult"/> containing the execution result.
        /// </returns>
        [HttpPost("send-remind")]
        public async Task<JsonResult> SendReminder([FromBody] ReminderDto data)
        {
            // validations
            if (!ModelState.IsValid)
                return new JsonResult(new
                {
                    message = "Required data is not found.",
                    success = false
                });

            // checks task is already completed or not
            var complete = await _taskService.TaskInfo(data.TaskId);
            if (complete.Any())
                if (complete.First().Complete)
                {
                    return new JsonResult(new
                    {
                        message = "Task is already completed.",
                        success = false
                    });
                }

            // gets the user email
            var task = await _taskService.TaskInfo(data.TaskId);
            if (task.Any())
            {
                // mail subject
                string subject = "Assigna API task reminder.";

                // mail body
                string body = data.Message;

                // assignee mail
                string mail = task.FirstOrDefault().UserMail!;

                // gets the result
                var result = await _mailService.SendMailAsync(mail, subject, body);
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

            return new JsonResult(new
            {
                message = "Task is not found to send a remind.",
                success = false
            });
        }
    }
}