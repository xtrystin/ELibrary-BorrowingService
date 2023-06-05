using ELibrary_BorrowingService.Application.Command;
using ELibrary_BorrowingService.Application.Command.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ELibrary_BorrowingService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BookingController : ControllerBase
{
    private readonly IBookingProvider _bookingProvider;

    public BookingController(IBookingProvider bookingProvider)
    {
        _bookingProvider = bookingProvider;
    }

    [HttpPost("{bookId}")]
    [ProducesResponseType(400, Type = typeof(string))]
    [ProducesResponseType(401, Type = typeof(string))]
    [ProducesResponseType(403, Type = typeof(string))]
    [ProducesResponseType(204)]
    public async Task<ActionResult> Post([FromRoute] int bookId, [FromBody] CustomerId customerId)
    {
        await _bookingProvider.Book(bookId, customerId.customerId);
        return NoContent();
    }

    [HttpPost("Return/{bookId}")]
    [ProducesResponseType(400, Type = typeof(string))]
    [ProducesResponseType(401, Type = typeof(string))]
    [ProducesResponseType(403, Type = typeof(string))]
    [ProducesResponseType(204)]
    public async Task<ActionResult> Return([FromRoute] int bookId, [FromBody] CustomerId customerId)
    {
        await _bookingProvider.UnBook(bookId, customerId.customerId);
        return NoContent();
    }
}