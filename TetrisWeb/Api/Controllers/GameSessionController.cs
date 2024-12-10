using Microsoft.AspNetCore.Mvc;
using TetrisWeb.ApiServices;
using TetrisWeb.DTOs;
using TetrisWeb.Components.Models;
using TetrisWeb.Services;
using TetrisWeb.Components;
using TetrisWeb.GameData;
using TetrisWeb.Components.Pages;

//possibly have users add in their game key to each call?

namespace TetrisWeb.Controllers;

[Route("api/[controller]")]
[ApiController]
public class GameSessionController(GameService game, GameSessionService gameSession) : ControllerBase
{
    [HttpGet("getGame/{gameId}/{playerId}")]
    public async Task GetGameSession(int gameId,int playerId)
    {
        await gameSession.GetGameSession(playerId, gameId);
    }

    [HttpGet("boardState/{gameId}/{playerId}")]
    public async Task<CellList> BoardState(int gameId, int playerId)
    {
        return gameSession.GetGameSession(playerId, gameId).Result.GameStateGrid.Cells;
    }

    [HttpPost("moveRight/{gameId}/{playerId}/{x}")]
    public async Task MoveRight(int gameId, int playerId, int x)
    {
        gameSession.GetGameSession(playerId, gameId).Result.MoveRight(x);
    }

    [HttpPost("moveLeft/{gameId}/{playerId}/{x}")]
    public async Task MoveLeft(int x, int gameId, int playerId)
    {
        gameSession.GetGameSession(playerId, gameId).Result.MoveLeft(x);
    }

    [HttpPost("rotate/{gameId}/{playerId}")]
    public async Task Rotate(int gameId, int playerId)
    {
        gameSession.GetGameSession(playerId, gameId).Result.Rotate();
    }

    [HttpPost("drop/{gameId}/{playerId}")]
    public async Task Drop(int gameId, int playerId)
    {
        gameSession.GetGameSession(playerId, gameId).Result.Drop();
    }
}
