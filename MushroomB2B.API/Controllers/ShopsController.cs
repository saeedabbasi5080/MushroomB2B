using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MushroomB2B.Application.Features.Shops.Commands.ChargeWallet;
using MushroomB2B.Application.Features.Shops.Commands.CreateShop;
using MushroomB2B.Application.Features.Shops.Queries.GetShopById;

namespace MushroomB2B.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public sealed class ShopsController(ISender sender) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(CreateShopResult), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> CreateShop(
        [FromBody] CreateShopCommand command,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetShopById), new { id = result.ShopId }, result);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ShopDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetShopById(Guid id, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetShopByIdQuery(id), cancellationToken);
        return Ok(result);
    }

    [HttpPut("{id:guid}/wallet")]
    [ProducesResponseType(typeof(ChargeWalletResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ChargeWallet(
        Guid id,
        [FromBody] ChargeWalletCommand command,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(command with { ShopId = id }, cancellationToken);
        return Ok(result);
    }
}
