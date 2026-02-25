using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MushroomB2B.Application.Features.Delivery.Commands.MarkItemDelivered;
using MushroomB2B.Application.Features.Delivery.Commands.MarkItemFailed;
using MushroomB2B.Application.Features.Delivery.Queries.GetDriverBatches;

namespace MushroomB2B.API.Controllers;

[Authorize(Roles = "Driver")]
[ApiController]
[Route("api/delivery")]
public sealed class DeliveryController(ISender sender) : ControllerBase
{
    [HttpGet("batches")]
    [ProducesResponseType(typeof(List<DriverBatchDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetDriverBatches(
        [FromQuery] Guid driverId,
        [FromQuery] DateTime? date,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetDriverBatchesQuery
        {
            DriverId = driverId,
            BatchDate = date
        }, cancellationToken);

        return Ok(result);
    }

    [HttpPut("items/{id:guid}/delivered")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> MarkDelivered(
        Guid id,
        [FromBody] MarkItemDeliveredCommand command,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(command with { DeliveryItemId = id }, cancellationToken);
        return Ok(result);
    }

    [HttpPut("items/{id:guid}/failed")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> MarkFailed(
        Guid id,
        [FromBody] MarkItemFailedCommand command,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(command with { DeliveryItemId = id }, cancellationToken);
        return Ok(result);
    }
}
