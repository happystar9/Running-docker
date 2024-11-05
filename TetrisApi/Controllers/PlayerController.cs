using Microsoft.AspNetCore.Mvc;
using TetrisApi.Services;
using TetrisShared.DTOs;


namespace TetrisApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PlayerController(IPlayerService playerService) : ControllerBase
{
    [HttpPost ("register")]
    public async Task<IResult> RegisterPlayer([FromBody] PlayerDto playerDto)
    {
        var newPlayer = await playerService.CreatePlayerAsync(playerDto);
        return Results.Ok(newPlayer);
    }
}
