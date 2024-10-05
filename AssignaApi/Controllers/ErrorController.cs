using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AssignaApi.Controllers
{
    [ApiController]
    public class ErrorController : ControllerBase
    {
        // services
        private readonly ILogger<ErrorController> _logger;

        public ErrorController(ILogger<ErrorController> logger)
        {
            _logger = logger;
        }

        // error handling
        [HttpGet("/error")]
        public IActionResult Error()
        {
            // error information
            var context = HttpContext.Features.Get<IExceptionHandlerFeature>();
            var errorMessage = context.Error.Message;

            // log the error
            _logger.LogError(errorMessage);

            return Problem();
        }
    }
}