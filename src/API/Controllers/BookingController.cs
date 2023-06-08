using ELibrary_BorrowingService.Application.Command;
using ELibrary_BorrowingService.Application.Command.Model;
using ELibrary_BorrowingService.Application.Query;
using ELibrary_BorrowingService.Application.Query.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace ELibrary_BorrowingService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BookingController : ControllerBase
{
    private readonly IBookingProvider _bookingProvider;
    private readonly IBookingReadProvider _bookingReadProvider;

    public BookingController(IBookingProvider bookingProvider, IBookingReadProvider bookingReadProvider)
    {
        _bookingProvider = bookingProvider;
        _bookingReadProvider = bookingReadProvider;
    }

    [HttpGet]
    [ProducesResponseType(200, Type = typeof(List<BorrowReadModel>))]
    [ProducesResponseType(404, Type = typeof(string))]
    public async Task<ActionResult<List<BookingReadModel>>> GetAll()
    {
        var result = await _bookingReadProvider.GetAll();
        if (result is null || result.Count == 0)
            return NotFound("No booking has been found");
        return result;
    }

    [HttpGet("ForCustomer/{customerId}")]
    [ProducesResponseType(200, Type = typeof(List<BorrowReadModel>))]
    [ProducesResponseType(404, Type = typeof(string))]
    [SwaggerOperation(Summary = "Returns with CustomerBasicInfo = null")]
    public async Task<ActionResult<List<BookingReadModel>>> GetByCustomer(string customerId)
    {
        var result = await _bookingReadProvider.GetByCustomer(customerId);
        if (result is null || result.Count == 0)
            return NotFound("No booking has been found");
        return result;
    }

    [HttpGet("ForBook/{bookId}")]
    [ProducesResponseType(200, Type = typeof(List<BorrowReadModel>))]
    [ProducesResponseType(404, Type = typeof(string))]
    [SwaggerOperation(Summary = "Returns with BookBasicInfo = null")]
    public async Task<ActionResult<List<BookingReadModel>>> GetByBook(int bookId)
    {
        var result = await _bookingReadProvider.GetByBook(bookId);
        if (result is null || result.Count == 0)
            return NotFound("No booking has been found");
        return result;
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