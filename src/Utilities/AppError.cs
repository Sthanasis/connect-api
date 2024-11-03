using connect.Models;
using Microsoft.AspNetCore.Mvc;

namespace connect.Utilities;

public class AppErrorUtility : ControllerBase
{
    public ActionResult SendServerError(string message)
    {
        return StatusCode(500, new AppResult()
        {
            Error = new ErrorViewModel { Message = message },
        });
    }

    public ActionResult SendClientError(string message)
    {
        return NotFound(new AppResult()
        {
            Error = new ErrorViewModel { Message = message },
        });
    }
}