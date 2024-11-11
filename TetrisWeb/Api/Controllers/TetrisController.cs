using Microsoft.AspNetCore.Mvc;
using TetrisWeb.ApiServices;
using TetrisShared.DTOs;
using TetrisWeb.Components.Models;
using TetrisWeb.Services;

//possibly have users add in their game key to each call?

namespace TetrisWeb.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TetrisController(IPlayerService playerService, GameStateService game) : ControllerBase
{
    [HttpPost("startGame/{apiKey}")]
    public async Task StartGame(string apiKey)
    {
        //need to make game based off api key not just creating a game
        await game.RunGame();
        return;
    }

    [HttpGet("boardState")]
    public async Task<CellList> BoardState()
    {
        return game.gameStateGrid.Cells;
    }

    [HttpPost("moveRight/{x}")]
    public async Task MoveRight(int x)
    {
        game.MoveRight(x);
    }

    [HttpPost("moveLeft/{x}")]
    public async Task MoveLeft(int x)
    {
        game.MoveLeft(x);
    }

    [HttpPost("rotate")]
    public async Task Rotate()
    {
        game.Rotate();
    }

    [HttpPost("drop")]
    public async Task Drop()
    {
        game.Drop();
    }
}
