using ELibrary_BorrowingService.Application.Command;
using ELibrary_BorrowingService.Application.Command.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace ELibrary_BorrowingService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BorrowController : ControllerBase
{
    private readonly IBorrowProvider _borrowProvider;

    public BorrowController(IBorrowProvider borrowProvider)
	{
        _borrowProvider = borrowProvider;
    }

    [HttpPost("bookId")]
    [Authorize(Roles = "admin, employee")]
    [ProducesResponseType(400, Type = typeof(string))]
    [ProducesResponseType(401, Type = typeof(string))]
    [ProducesResponseType(403, Type = typeof(string))]
    [ProducesResponseType(204)]
    public async Task<ActionResult> Post([FromRoute] int bookId, [FromBody] CustomerId customerId)
    {
        await _borrowProvider.Borrow(bookId, customerId.customerId);
        return NoContent();
    }

    [HttpPost("Return/bookId")]
    [Authorize(Roles = "admin, employee")]
    [ProducesResponseType(400, Type = typeof(string))]
    [ProducesResponseType(401, Type = typeof(string))]
    [ProducesResponseType(403, Type = typeof(string))]
    [ProducesResponseType(204)]
    public async Task<ActionResult> Return([FromRoute] int bookId, [FromBody] CustomerId customerId)
    {
        await _borrowProvider.Return(bookId, customerId.customerId);
        return NoContent();
    }
}
