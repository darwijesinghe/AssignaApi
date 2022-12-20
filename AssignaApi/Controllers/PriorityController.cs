using AssignaApi.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AssignaApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PriorityController : ControllerBase
    {
        // services
        private readonly IDataService _dataService;

        public PriorityController(IDataService dataService)
        {
            _dataService = dataService;
        }

        // priorities
        [HttpGet("priorities")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public JsonResult Priorities()
        {
            var result = _dataService.Priorities();
            return new JsonResult(new
            {
                message = "Ok",
                success = true,
                data = result
            });
        }
    }
}