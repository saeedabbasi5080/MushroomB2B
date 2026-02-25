using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MushroomB2B.Application.Features.Products.Commands.AddPriceTier;
using MushroomB2B.Application.Features.Products.Commands.CreateProduct;
using MushroomB2B.Application.Features.Products.Queries.GetAllProducts;

namespace MushroomB2B.API.Controllers;


[Authorize]
[ApiController]
[Route("api/[controller]")]
public sealed class ProductsController(ISender sender) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(CreateProductResult), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> CreateProduct(
        [FromBody] CreateProductCommand command,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetAllProducts), result);
    }

    [HttpGet]
    [ProducesResponseType(typeof(List<ProductDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllProducts(CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetAllProductsQuery(), cancellationToken);
        return Ok(result);
    }

    [HttpPost("{id:guid}/price-tiers")]
    [ProducesResponseType(typeof(AddPriceTierResult), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddPriceTier(
        Guid id,
        [FromBody] AddPriceTierCommand command,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(command with { ProductId = id }, cancellationToken);
        return StatusCode(StatusCodes.Status201Created, result);
    }
}
