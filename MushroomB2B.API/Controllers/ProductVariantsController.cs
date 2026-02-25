using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MushroomB2B.Application.Features.ProductVariants.Commands.AddVariant;
using MushroomB2B.Application.Features.ProductVariants.Commands.UpdateStock;

namespace MushroomB2B.API.Controllers;


[Authorize]
[ApiController]
public sealed class ProductVariantsController(ISender sender) : ControllerBase
{
    [HttpPost("api/products/{productId:guid}/variants")]
    [ProducesResponseType(typeof(AddVariantResult), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddVariant(
        Guid productId,
        [FromBody] AddVariantCommand command,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(command with { ProductId = productId }, cancellationToken);
        return StatusCode(StatusCodes.Status201Created, result);
    }

    [HttpPut("api/variants/{id:guid}/stock")]
    [ProducesResponseType(typeof(UpdateStockResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateStock(
        Guid id,
        [FromBody] UpdateStockCommand command,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(command with { VariantId = id }, cancellationToken);
        return Ok(result);
    }
}
