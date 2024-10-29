using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using connect.Models;

namespace connect.Namespace
{
    [Route("api/[controller]")]
    [ApiController]
    public class ErrorController : ControllerBase
    {
        [HttpGet]
        [Route("GetErrorType")]
        public IActionResult GetErrorType(){
            ErrorViewModel error = new();
            return StatusCode(StatusCodes.Status200OK, error);
        }

    }
}
