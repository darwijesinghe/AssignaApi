using AssignaApi.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AssignaApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CategoryController : ControllerBase
    {
        // services
        private readonly IDataService _dataService;

        public CategoryController(IDataService dataService)
        {
            _dataService = dataService;
        }

        // all categories
        [HttpGet("categories")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public JsonResult Categories()
        {
            var result = _dataService.AllCategories();
            return new JsonResult(new
            {
                message = "Ok",
                success = true,
                data = result
            });
        }
    }
}