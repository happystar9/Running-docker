using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Crypto.Macs;
using TetrisWeb.ApiServices.Interfaces;
using TetrisWeb.DTOs;

namespace TetrisWeb.Api.Controllers;


[Route("api/[controller]")]
[ApiController]
public class ScoreController(IScoreService scoreService) : ControllerBase
{
    [HttpGet("leaderboard")]
    public async Task<IResult> GetCompleteLeaderboard()
    {
        return Results.Ok(await scoreService.GetCompleteLeaderboardAsync());
    }

}
