using Microsoft.AspNetCore.Mvc;
using TetrisWeb.ApiServices.Interfaces;
using TetrisWeb.DTOs;

namespace TetrisWeb.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PlayerController(IPlayerService playerService, IApiKeyManagementService apiKeyService) : ControllerBase
{
    [HttpPost("register")]
    public async Task<IResult> RegisterPlayer([FromBody] PlayerDto playerDto)
    {
        var newPlayer = await playerService.CreatePlayerAsync(playerDto);
        return Results.Ok(newPlayer);
    }

    //[HttpGet("{playerId}")]

    [HttpPost("{playerId}/assignkey")]
    public async Task<IResult> AssignApiKey(int playerId)
    {
        try
        {
            string newKey = await apiKeyService.AssignKeyAsync(playerId);
            return Results.Ok($"{newKey} successfully assigned to player {playerId}");
        }
        catch (Exception ex)
        {
            return Results.BadRequest(ex.Message);
        }
    }

    [HttpGet("{playerId}/apikeys")]
    public async Task<IResult> GetApiKeys(int playerId)
    {
        try
        {
            var playerKeys = await apiKeyService.GetPlayerKeysAsync(playerId);

            if(playerKeys == null || !playerKeys.Any())
            {
                return Results.NotFound($"No API keys found for player with ID {playerId}.");
            }

            return Results.Ok(playerKeys);
        }
        catch (Exception ex)
        {
            return Results.BadRequest(ex.Message);
        }
    }

    [HttpGet("validate")]
    public IActionResult ValidateApiKey([FromQuery] string key)
    {
        bool isValid = apiKeyService.IsValidApiKey(key);

        if (isValid)
            return Ok(new { message = "API key is valid." });
        else
            return Unauthorized(new { message = "Invalid API key." });
    }

    //doesn't actually delete the key, just sets the expiration date to now
    [HttpDelete("invalidate")]
    public async Task<IActionResult> InvalidateApiKey([FromQuery] string key)
    {
        await apiKeyService.InvalidateApiKeyAsync(key);

        return Ok(new { message = "API key invalidated." });
    }


}
