using ELibrary_BorrowingService.Application.Command.Exception;
using ELibrary_BorrowingService.Domain.Exception;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace ELibrary_BorrowingService.Controllers;

[AllowAnonymous]
[ApiExplorerSettings(IgnoreApi = true)]
public class ErrorHandlerController : ControllerBase
{
    [Route("error")]
    public string Erorr()
    {
        var context = HttpContext.Features.Get<IExceptionHandlerFeature>();
        var exception = context.Error;
        var code = 500;

        if (exception is EntityNotFoundException or NoItemException)
            code = 404;
        else
            code = 400;


        Response.StatusCode = code;
        return exception.Message;

    }
}
