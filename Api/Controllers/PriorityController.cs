using Application.Interfaces.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AssignaApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PriorityController : ControllerBase
    {
        // Services
        private readonly ITaskService _taskService;

        public PriorityController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        /// <summary>
        /// Retrieves a list of all task's priorities.
        /// </summary>
        /// <returns>
        /// A <see cref="JsonResult"/> containing the task data.
        /// </returns>
        [HttpGet("priorities")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<JsonResult> Priorities()
        {
            // gets priorities
            var result = await _taskService.Priorities();

            return new JsonResult(new
            {
                message = "Ok.",
                success = true,
                data    = result
            });
        }
    }
}