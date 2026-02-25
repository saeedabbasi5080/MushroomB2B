using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MushroomB2B.Application.Features.Admin.Commands.AssignDeliveryBatch;
using MushroomB2B.Application.Features.Admin.Commands.SoftDeleteOrder;
using MushroomB2B.Application.Features.Admin.Commands.SoftDeleteProduct;
using MushroomB2B.Application.Features.Admin.Commands.SoftDeleteShop;
using MushroomB2B.Application.Features.Admin.Commands.VerifyShop;
using MushroomB2B.Application.Features.Admin.Queries.GetAllOrders;
using MushroomB2B.Application.Features.Admin.Queries.GetAllShops;
using MushroomB2B.Application.Features.Admin.Queries.GetDailyDeliveryReport;
using MushroomB2B.Domain.Enums;

namespace MushroomB2B.API.Controllers;

[ApiController]
[Route("api/admin")]
[Authorize(Roles = "Admin")]
public sealed class AdminController(ISender sender) : ControllerBase
{
    [HttpGet("orders")]
    [ProducesResponseType(typeof(List<AdminOrderDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllOrders(
        [FromQuery] OrderStatus? status,
        [FromQuery] DateTime? fromDate,
        [FromQuery] DateTime? toDate,
        [FromQuery] Guid? shopId,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetAllOrdersQuery
        {
            Status = status,
            FromDate = fromDate,
            ToDate = toDate,
            ShopId = shopId
        }, cancellationToken);

        return Ok(result);
    }

    [HttpGet("orders/daily-report")]
    [ProducesResponseType(typeof(List<DeliveryReportItemDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetDailyDeliveryReport(
        [FromQuery] DateTime date,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetDailyDeliveryReportQuery(date), cancellationToken);
        return Ok(result);
    }

    [HttpGet("shops")]
    [ProducesResponseType(typeof(List<AdminShopDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllShops(CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetAllShopsQuery(), cancellationToken);
        return Ok(result);
    }

    [HttpPut("shops/{id:guid}/verify")]
    [ProducesResponseType(typeof(VerifyShopResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> VerifyShop(Guid id, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new VerifyShopCommand(id), cancellationToken);
        return Ok(result);
    }

    [HttpPost("delivery-batches")]
    [ProducesResponseType(typeof(AssignDeliveryBatchResult), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> AssignDeliveryBatch(
        [FromBody] AssignDeliveryBatchCommand command,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(command, cancellationToken);
        return StatusCode(StatusCodes.Status201Created, result);
    }

    [HttpDelete("shops/{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SoftDeleteShop(Guid id, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new SoftDeleteShopCommand(id), cancellationToken);
        return Ok(result);
    }

    [HttpDelete("orders/{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SoftDeleteOrder(Guid id, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new SoftDeleteOrderCommand(id), cancellationToken);
        return Ok(result);
    }

    [HttpDelete("products/{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SoftDeleteProduct(Guid id, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new SoftDeleteProductCommand(id), cancellationToken);
        return Ok(result);
    }

}
