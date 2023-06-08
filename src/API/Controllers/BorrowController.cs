using ELibrary_BorrowingService.Application.Command;
using ELibrary_BorrowingService.Application.Command.Model;
using ELibrary_BorrowingService.Application.Query;
using ELibrary_BorrowingService.Application.Query.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Data;

namespace ELibrary_BorrowingService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BorrowController : ControllerBase
{
    private readonly IBorrowProvider _borrowProvider;
    private readonly IBorrowReadProvider _borrowReadProvider;

    public BorrowController(IBorrowProvider borrowProvider, IBorrowReadProvider borrowReadProvider)
	{
        _borrowProvider = borrowProvider;
        _borrowReadProvider = borrowReadProvider;
    }

    [HttpGet]
    [ProducesResponseType(200, Type = typeof(List<BorrowReadModel>))]
    [ProducesResponseType(404, Type = typeof(string))]
    public async Task<ActionResult<List<BorrowReadModel>>> GetAll()
    {
        var result = await _borrowReadProvider.GetAll();
        if (result is null || result.Count == 0)
            return NotFound("No borrow has been found");
        return result;
    }

    [HttpGet("ByCustomer/{customerId}")]
    [ProducesResponseType(200, Type = typeof(List<BorrowReadModel>))]
    [ProducesResponseType(404, Type = typeof(string))]
    [SwaggerOperation(Summary = "Returns with CustomerBasicInfo = null")]
    public async Task<ActionResult<List<BorrowReadModel>>> GetByCustomer(string customerId)
    {
        var result = await _borrowReadProvider.GetByCustomer(customerId);
        if (result is null || result.Count == 0)
            return NotFound("No borrow has been found");
        return result;
    }

    [HttpGet("ByBook/{bookId}")]
    [ProducesResponseType(200, Type = typeof(List<BorrowReadModel>))]
    [ProducesResponseType(404, Type = typeof(string))]
    [SwaggerOperation(Summary = "Returns with BookBasicInfo = null")]
    public async Task<ActionResult<List<BorrowReadModel>>> GetByBook(int bookId)
    {
        var result = await _borrowReadProvider.GetByBook(bookId);
        if (result is null || result.Count == 0)
            return NotFound("No borrow has been found");
        return result;
    }

    [HttpPost("{bookId}")]
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

    [HttpPost("Return/{bookId}")]
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
