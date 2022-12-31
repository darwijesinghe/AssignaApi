using System.Threading.Tasks;
using AssignaApi.Interfaces;
using AssignaApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace AssignaApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,
        Roles = Roles.member)]
    public class MemberTasksController : ControllerBase
    {
        // services
        private readonly IDataService _dataService;

        public MemberTasksController(IDataService dataService)
        {
            _dataService = dataService;
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

        // high priority task
        [HttpGet("high-priority")]
        public async Task<IActionResult> HighPriority()
        {
            var result = await _dataService.HighPriority();
            return new JsonResult(new
            {
                data = result
            });
        }

        // medium priority task
        [HttpGet("medium-priority")]
        public async Task<IActionResult> MediumPriority()
        {
            var result = await _dataService.MediumPriority();
            return new JsonResult(new
            {
                data = result
            });
        }

        // low priority task
        [HttpGet("low-priority")]
        public async Task<IActionResult> LowPriority()
        {
            var result = await _dataService.LowPriority();
            return new JsonResult(new
            {
                data = result
            });
        }

        // write a task note
        [HttpPost("write-note")]
        public async Task<JsonResult> WriteNote([FromBody] AddNote data)
        {
            // validations
            if (!ModelState.IsValid)
            {
                return new JsonResult(new
                {
                    message = "Required data is not found",
                    success = false
                });
            }

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
            else
            {
                return new JsonResult(new
                {
                    message = "Task is not found to add a note",
                    success = false
                });
            }

            // send data to add task note
            var task = new TaskDto()
            {
                tsk_id = data.tsk_id,
                user_note = data.user_note
            };

            var result = await _dataService.AddTaskNoteAsync(task);
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

        // mark as done
        [HttpPost("mark-done")]
        public async Task<JsonResult> MarkasDone([FromBody] MarkDone data)
        {
            // validations
            if (!ModelState.IsValid)
            {
                return new JsonResult(new
                {
                    message = "Required data is not found",
                    success = false
                });
            }

            var task = await _dataService.TaskInfo(data.tsk_id);
            if (task.Count == 0)
            {
                return new JsonResult(new
                {
                    message = "Task is not found to mark as done",
                    success = false
                });

            }

            var result = await _dataService.MarkasDoneAsync(data);
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
    }
}