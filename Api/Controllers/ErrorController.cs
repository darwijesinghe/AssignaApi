using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AssignaApi.Controllers
{
    [ApiController]
    public class ErrorController : ControllerBase
    {
        // Services
        private readonly ILogger<ErrorController> _logger;

        public ErrorController(ILogger<ErrorController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Handles unhandled exceptions in the application by returning an error view.
        /// </summary>
        /// <returns>
        /// Returns the error page.
        /// </returns>
        [HttpGet("/error")]
        public IActionResult Error()
        {
            // error information
            var context      = HttpContext.Features.Get<IExceptionHandlerFeature>();
            var errorMessage = context.Error.Message;

            // logs the error
            _logger.LogError(errorMessage);

            return Problem();
        }
    }
}