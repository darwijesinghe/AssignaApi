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
        private IMailService MailSend { get; }
        public LeadTasksController(IDataService dataService, IMailService mailSend)
        {
            _dataService = dataService;
            MailSend = mailSend;
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
            if (result.Count == 0)
            {
                return new JsonResult(new
                {
                    message = "Requested task is not found.",
                    success = false
                });

            }

            return new JsonResult(new
            {
                data = result
            });
        }

        // add a new task
        [HttpPost("add-task")]
        public async Task<JsonResult> AddTask([FromBody] NewTask data)
        {
            // validations
            if (!ModelState.IsValid)
            {
                return new JsonResult(new
                {
                    message = "Required data is not found.",
                    success = false
                });
            }

            // check category
            var category = _dataService.AllCategories();
            if (!category.Any(x => x.CatId == data.TskCategory))
            {
                return new JsonResult(new
                {
                    message = "Category id is not valid.",
                    success = false
                });
            }

            // check assignee
            var member = _dataService.TeamMembers();
            if (!member.Any(x => x.UserId == data.Member))
            {
                return new JsonResult(new
                {
                    message = "Member id is not valid.",
                    success = false
                });
            }


            // send data to save a new task
            var task = new TaskDto()
            {
                TskTitle = data.TskTitle,
                Deadline = Convert.ToDateTime(data.Deadline),
                TskNote = data.TskNote,
                PriHigh = (data.Priority == PriLevels.high),
                PriMedium = (data.Priority == PriLevels.medium),
                PriLow = (data.Priority == PriLevels.low),
                CatId = data.TskCategory,
                UserId = data.Member,
                Pending = true
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
                    message = "Server error.",
                    success = false
                });
            }

        }

        // edit a task
        [HttpPost("edit-task")]
        public async Task<JsonResult> EditTask([FromBody] EditTask data)
        {
            // validations
            if (!ModelState.IsValid)
            {
                return new JsonResult(new
                {
                    message = "Required data is not found.",
                    success = false
                });
            }

            // check task is already completed or not
            var complete = await _dataService.TaskInfo(data.TskId);
            if (complete.Count > 0)
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
                    message = "Task is not found to edit.",
                    success = false
                });
            }

            // check category
            var category = _dataService.AllCategories();
            if (!category.Any(x => x.CatId == data.TskCategory))
            {
                return new JsonResult(new
                {
                    message = "Category id is not valid.",
                    success = false
                });
            }

            // check assignee
            var member = _dataService.TeamMembers();
            if (!member.Any(x => x.UserId == data.Member))
            {
                return new JsonResult(new
                {
                    message = "Member id is not valid.",
                    success = false
                });
            }


            // send data to save a new task
            var task = new TaskDto()
            {
                TskId = data.TskId,
                TskTitle = data.TskTitle,
                Deadline = Convert.ToDateTime(data.Deadline),
                TskNote = data.TskNote,
                PriHigh = (data.Priority == PriLevels.high),
                PriMedium = (data.Priority == PriLevels.medium),
                PriLow = (data.Priority == PriLevels.low),
                CatId = data.TskCategory,
                UserId = data.Member,
                Pending = true
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
                    message = "Server error.",
                    success = false
                });
            }
        }

        // delete a task
        [HttpPost("delete-task")]
        public async Task<JsonResult> DeleteTask([FromBody] DeleteTask data)
        {
            // validations
            if (!ModelState.IsValid)
            {
                return new JsonResult(new
                {
                    message = "Required data is not found.",
                    success = false
                });
            }

            var task = await _dataService.TaskInfo(data.TskId);
            if (task.Count == 0)
            {
                return new JsonResult(new
                {
                    message = "Task is not found to delete.",
                    success = false
                });

            }

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
                    message = "Server error.",
                    success = false
                });
            }
        }

        // send email remind
        [HttpPost("send-remind")]
        public async Task<JsonResult> SendReminder([FromBody] Reminder data)
        {
            // validations
            if (!ModelState.IsValid)
            {
                return new JsonResult(new
                {
                    message = "Required data is not found.",
                    success = false
                });
            }

            // check task is already completed or not
            var complete = await _dataService.TaskInfo(data.TskId);
            if (complete.Count > 0)
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

            // get user email
            var task = await _dataService.TaskInfo(data.TskId);
            if (task.Count > 0)
            {
                // mail subject
                string subject = "Assigna API task reminder";

                // mail body
                string body = data.Message;

                // assignee mail
                string mail = task.FirstOrDefault().UserMail!;

                var result = await MailSend.SendMailAsync(mail, subject, body);
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
                        message = "Server error.",
                        success = false
                    });
                }
            }
            else
            {
                return new JsonResult(new
                {
                    message = "Task is not found to send a remind.",
                    success = false
                });
            }

        }

    }
}