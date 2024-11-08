using Microsoft.AspNetCore.Mvc;
using TetrisWeb.ApiServices;
using TetrisShared.DTOs;
using TetrisWeb.Components.Models;



namespace TetrisWeb.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TetrisController(IPlayerService playerService) : ControllerBase
{
    [HttpGet("boardState")]
    public async Task<CellList> BoardState()
    {
        
    }

    [HttpPost("moveRight/{x}")]
    public async Task MoveRight(int x)
    {

    }

    [HttpPost("moveLeft/{x}")]
    public async Task MoveLeft(int x)
    {

    }

    [HttpPost("rotate")]
    public async Task Rotate()
    {

    }

    [HttpPost("drop")]
    public async Task Drop()
    {

    }
}
