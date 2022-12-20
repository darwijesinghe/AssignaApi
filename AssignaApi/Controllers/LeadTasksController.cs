using System;
using System.Threading.Tasks;
using AssignaApi.Interfaces;
using AssignaApi.Models;
using AssignaApi.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace AssignaApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,
        Roles = Roles.lead)]
    public class LeadTasksController : ControllerBase
    {
        // services
        private readonly IDataService _dataService;
        private IMailService _mailSend { get; }
        public LeadTasksController(IDataService dataService, IMailService mailSend)
        {
            _dataService = dataService;
            _mailSend = mailSend;
        }

        // return all task
        [HttpGet("tasks")]
        public async Task<JsonResult> Tasks()
        {
            var result = await _dataService.AllTasks();
            return new JsonResult(new
            {
                data = result
            });
        }

        // pending task
        [HttpGet("pendings")]
        public async Task<IActionResult> Pending()
        {
            var result = await _dataService.Pendings();
            return new JsonResult(new
            {
                data = result
            });
        }

        // completed task
        [HttpGet("completes")]
        public async Task<IActionResult> Complete()
        {
            var result = await _dataService.Completed();
            return new JsonResult(new
            {
                data = result
            });
        }

        // high priority tasks
        [HttpGet("high-priority")]
        public async Task<IActionResult> HighPriority()
        {
            var result = await _dataService.HighPriority();
            return new JsonResult(new
            {
                data = result
            });
        }

        // medium priority tasks
        [HttpGet("medium-priority")]
        public async Task<IActionResult> MediumPriority()
        {
            var result = await _dataService.MediumPriority();
            return new JsonResult(new
            {
                data = result
            });
        }

        // low priority tasks
        [HttpGet("low-priority")]
        public async Task<IActionResult> LowPriority()
        {
            var result = await _dataService.LowPriority();
            return new JsonResult(new
            {
                data = result
            });
        }

        // task info
        [HttpGet("task-info")]
        public async Task<IActionResult> TaskInfo(int taskId)
        {
            var result = await _dataService.TaskInfo(taskId);
            return new JsonResult(new
            {
                data = result
            });
        }

        // add a new task
        [HttpPost("add-task")]
        public async Task<JsonResult> AddTask([FromBody] NewTask data)
        {
            // send data to save a new task
            var task = new TaskDto()
            {
                tsk_title = data.tsk_title,
                deadline = Convert.ToDateTime(data.deadline),
                tsk_note = data.tsk_note,
                pri_high = (data.priority == PriLevels.high) ? true : false,
                pri_medium = (data.priority == PriLevels.medium) ? true : false,
                pri_low = (data.priority == PriLevels.low) ? true : false,
                cat_id = data.tsk_category,
                user_id = data.member,
                pending = true
            };

            var result = await _dataService.SaveTaskAsync(task);
            if (result.success)
            {
                return new JsonResult(new
                {
                    message = "Ok",
                    success = true
                });
            }
            else
            {
                return new JsonResult(new
                {
                    message = "Server error",
                    success = false
                });
            }

        }

        // edit a task
        [HttpPost("edit-task")]
        public async Task<JsonResult> EditTask([FromBody] EditTask data)
        {
            // check task is already completed or not
            var complete = await _dataService.TaskInfo(data.tsk_id);
            if (complete.Count > 0)
            {
                if (complete.First().complete)
                {
                    return new JsonResult(new
                    {
                        message = "Task is already completed",
                        success = false
                    });
                }
            }

            // send data to save a new task
            var task = new TaskDto()
            {
                tsk_id = data.tsk_id,
                tsk_title = data.tsk_title,
                deadline = Convert.ToDateTime(data.deadline),
                tsk_note = data.tsk_note,
                pri_high = (data.priority == PriLevels.high) ? true : false,
                pri_medium = (data.priority == PriLevels.medium) ? true : false,
                pri_low = (data.priority == PriLevels.low) ? true : false,
                cat_id = data.tsk_category,
                user_id = data.member,
                pending = true
            };

            var result = await _dataService.EditTaskAsync(task);
            if (result.success)
            {
                return new JsonResult(new
                {
                    message = "Ok",
                    success = true
                });
            }
            else
            {
                return new JsonResult(new
                {
                    message = "Server error",
                    success = false
                });
            }
        }

        // delete a task
        [HttpPost("delete-task")]
        public async Task<JsonResult> DeleteTask([FromBody] DeleteTask data)
        {
            var result = await _dataService.DeleteTaskAsync(data);
            if (result.success)
            {
                return new JsonResult(new
                {
                    message = "Ok",
                    success = true
                });
            }
            else
            {
                return new JsonResult(new
                {
                    message = "Server error",
                    success = false
                });
            }
        }

        // send email remind
        [HttpPost("send-remind")]
        public async Task<JsonResult> SendReminder([FromBody] Reminder data)
        {
            // check task is already completed or not
            var complete = await _dataService.TaskInfo(data.tsk_id);
            if (complete.Count > 0)
            {
                if (complete.First().complete)
                {
                    return new JsonResult(new
                    {
                        message = "Task is already completed",
                        success = false
                    });
                }
            }

            // get user email
            var task = await _dataService.TaskInfo(data.tsk_id);
            if (task is not null)
            {
                // mail subject
                string subject = "Assigna API task reminder";

                // mail body
                string body = data.message;

                // assignee mail
                string mail = task.FirstOrDefault().user_mail!;

                var result = await _mailSend.SendMailAsync(mail, subject, body);
                if (result.success)
                {
                    return new JsonResult(new
                    {
                        message = "Ok",
                        success = true
                    });
                }
                else
                {
                    return new JsonResult(new
                    {
                        message = "Server error",
                        success = false
                    });
                }
            }
            else
            {
                return new JsonResult(new
                {
                    message = "Task is not found",
                    success = false
                });
            }

        }

    }
}