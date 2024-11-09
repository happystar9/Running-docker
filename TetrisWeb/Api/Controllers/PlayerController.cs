using Microsoft.AspNetCore.Mvc;
using TetrisWeb.ApiServices.Interfaces;
using TetrisWeb.DTOs;



namespace TetrisWeb.Controllers;

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
