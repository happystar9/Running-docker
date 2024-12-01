using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Crypto.Macs;
using TetrisWeb.ApiServices.Interfaces;
using TetrisWeb.DTOs;

namespace TetrisWeb.Api.Controllers;


[Route("api/[controller]")]
[ApiController]
public class GameController(IGameService gameService) : ControllerBase
{
    [HttpGet("allgames")]
    public async Task<IResult> GetAllGames()
    {
        var games = await gameService.GetAllGamesAsync();

        if(!games.Any())
        {
            return Results.NoContent();
        }

        return Results.Ok(games);
    }

    [HttpGet("livegames")]
    public async Task<IResult> GetLiveGames()
    {
        var games = await gameService.GetAllLiveGamesAsync();
        if(!games.Any())
        {
            return Results.NoContent();
        }

        return Results.Ok(await gameService.GetAllLiveGamesAsync());
    }

    [HttpGet("{gameId}")]
    public async Task<IResult> GetGame(int gameId)
    {
        var game = await gameService.GetGameByIdAsync(gameId);
        if(game is null)
        {
            return Results.NoContent();
        }
        return Results.Ok(await gameService.GetGameByIdAsync(gameId));
    }
}
