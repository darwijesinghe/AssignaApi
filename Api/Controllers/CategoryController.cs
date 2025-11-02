using Application.Interfaces.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AssignaApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CategoryController : ControllerBase
    {
        // Services
        private readonly ITaskService _taskService;

        public CategoryController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        /// <summary>
        /// Retrieves a list of all category related data.
        /// </summary>
        /// <returns>
        /// A <see cref="JsonResult"/> containing the task data.
        /// </returns>
        [HttpGet("categories")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<JsonResult> Categories()
        {
            // gets categories
            var result = await _taskService.AllCategories();

            return new JsonResult(new
            {
                message = "Ok.",
                success = true,
                data    = result
            });
        }
    }
}