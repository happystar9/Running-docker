//using Microsoft.AspNetCore.Mvc;
//using TetrisWeb.ApiServices;
//using TetrisShared.DTOs;
//using TetrisWeb.Components.Models;
//using TetrisWeb.Services;

////possibly have users add in their game key to each call?

//namespace TetrisWeb.Controllers;

//[Route("api/[controller]")]
//[ApiController]
//public class GameSessionController(GameManager game) : ControllerBase
//{
//    [HttpPost("startGame/{playerId}")]
//    public void JoinGame(int playerId)
//    {
//        game.CreateGame(playerId);
//        return;
//    }

//    [HttpGet("boardState/{apiKey}")]
//    public async Task<CellList> BoardState(string apiKey)
//    {
//        return game._games[apiKey].gameStateGrid.Cells;
//    }

//    [HttpPost("moveRight/{apiKey}/{x}")]
//    public async Task MoveRight(int x, string apiKey)
//    {
//        game._games[apiKey].MoveRight(x);
//    }

//    [HttpPost("moveLeft/{apiKey}/{x}")]
//    public async Task MoveLeft(int x, string apiKey)
//    {
//        game._games[apiKey].MoveLeft(x);
//    }

//    [HttpPost("rotate/{apiKey}")]
//    public async Task Rotate(string apiKey)
//    {
//        game._games[apiKey].Rotate();
//    }

//    [HttpPost("drop/{apiKey}")]
//    public async Task Drop(string apiKey)
//    {
//        game._games[apiKey].Drop();
//    }
//}
