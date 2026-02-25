using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MushroomB2B.Application.Features.Drivers.Commands.CreateDriver;
using MushroomB2B.Application.Features.Drivers.Queries.GetAllDrivers;

namespace MushroomB2B.API.Controllers;


[Authorize(Roles = "Admin")]
[ApiController]
[Route("api/[controller]")]
public sealed class DriversController(ISender sender) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(CreateDriverResult), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> CreateDriver(
        [FromBody] CreateDriverCommand command,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(command, cancellationToken);
        return StatusCode(StatusCodes.Status201Created, result);
    }

    [HttpGet]
    [ProducesResponseType(typeof(List<DriverDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllDrivers(CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetAllDriversQuery(), cancellationToken);
        return Ok(result);
    }
}
