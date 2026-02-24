using MediatR;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Mvc;
using MushroomB2B.Application.Features.Orders.Commands.CreateOrder;

namespace MushroomB2B.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class OrdersController(ISender sender) : ControllerBase
{
    /// <summary>
    /// Creates a new bulk order for a shop.
    /// Stock reservation, tiered pricing, and credit check happen atomically.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(CreateOrderResult), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> CreateOrder(
        [FromBody] CreateOrderCommand command,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(command, cancellationToken);
        return CreatedAtAction(nameof(CreateOrder), new { id = result.OrderId }, result);
    }
}
