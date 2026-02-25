using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MushroomB2B.Application.Common.Models;
using MushroomB2B.Application.Features.Orders.Commands.CreateOrder;
using MushroomB2B.Application.Features.Orders.Commands.UpdateOrderStatus;
using MushroomB2B.Application.Features.Orders.Queries.GetOrderById;
using MushroomB2B.Application.Features.Orders.Queries.GetOrdersByShopId;
using MushroomB2B.Domain.Enums;

namespace MushroomB2B.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public sealed class OrdersController(ISender sender) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(CreateOrderResult), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> CreateOrder(
        [FromBody] CreateOrderCommand command,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetOrderById), new { id = result.OrderId }, result);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(OrderDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetOrderById(Guid id, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetOrderByIdQuery(id), cancellationToken);
        return Ok(result);
    }

    [HttpGet("shop/{shopId:guid}")]
    [ProducesResponseType(typeof(PaginatedResult<ShopOrderDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetOrdersByShop(
        Guid shopId,
        [FromQuery] OrderStatus? status,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        var result = await sender.Send(new GetOrdersByShopIdQuery
        {
            ShopId = shopId,
            Status = status,
            PageNumber = pageNumber,
            PageSize = pageSize
        }, cancellationToken);

        return Ok(result);
    }

    [HttpPut("{id:guid}/status")]
    [ProducesResponseType(typeof(UpdateOrderStatusResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateOrderStatus(
        Guid id,
        [FromBody] UpdateOrderStatusCommand command,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(command with { OrderId = id }, cancellationToken);
        return Ok(result);
    }
}
