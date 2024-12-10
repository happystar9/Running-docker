using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Crypto.Macs;
using TetrisWeb.ApiServices;
using TetrisWeb.ApiServices.Interfaces;
using TetrisWeb.Components;
using TetrisWeb.DTOs;

namespace TetrisWeb.Api.Controllers;


[Route("api/[controller]")]
[ApiController]
public class GameController(IGameService gameService, GameSessionService gameSession) : ControllerBase
{
    [HttpGet("allgames")]
    public async Task<IResult> GetAllGames()
    {
        var games = await gameService.GetAllGamesAsync();

        if (!games.Any())
        {
            return Results.NoContent();
        }

        return Results.Ok(games);
    }

    [HttpGet("livegames")]
    public async Task<IResult> GetLiveGames()
    {
        var games = await gameService.GetAllLiveGamesAsync();
        if (!games.Any())
        {
            return Results.NoContent();
        }

        return Results.Ok(await gameService.GetAllLiveGamesAsync());
    }

    [HttpGet("{gameId}")]
    public async Task<IResult> GetGame(int gameId)
    {
        var game = await gameService.GetGameByIdAsync(gameId);
        if (game is null)
        {
            return Results.NoContent();
        }
        return Results.Ok(await gameService.GetGameByIdAsync(gameId));
    }

    [HttpPut ("join/{gameId}/{playerId}")]
    public async Task<IResult> JoinGame(int gameId, int playerId)
    {
        try
        {
            gameSession.CreateGameSession(gameId, playerId);
            GameLoop session = await gameSession.GetGameSession(playerId, gameId);
            gameService.JoinGameAsync(gameId, playerId, session);
            return Results.Created();
        }
        catch (Exception ex)
        {
            return Results.BadRequest(ex);  
        }
    }

}