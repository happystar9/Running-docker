using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TetrisWeb.ApiServices.Interfaces;
using TetrisWeb.ApiServices;
using Xunit.Abstractions;
using TetrisWeb.DTOs;

namespace TetrisTest.IntegrationTests;


[Collection("SequentialTestExecution")]
public class ScoreServiceTests : PostgresTestBase
{
    public ScoreServiceTests(WebApplicationFactory<Program> webAppFactory, ITestOutputHelper outputHelper)
       : base(webAppFactory, outputHelper) { }

    public async Task CanGetPlayerHighScore()
    {
        var playerService = GetService<IPlayerService>();
        var gameSessionService = GetService<GameSessionService>();
        var gameService = GetService<IGameService>();
        var scoreService = GetService<IScoreService>();


        var samplePlayer = new PlayerDto
        {
            Username = "Player12",
            Authid = "12",
            PlayerQuote = "TestQuote",
            AvatarUrl = "TestAvatarUrl",
            Isblocked = false
        };

        var adminUser = new PlayerDto
        {
            Username = "TestAuthUser",
            Authid = "TestAuthId",
            PlayerQuote = "TestQuote",
            AvatarUrl = "TestAvatarUrl",
            Isblocked = false
        };

        await playerService.CreatePlayerAsync(samplePlayer);
        await playerService.CreatePlayerAsync(adminUser);

        var game = await gameService.CreateGameAsync(adminUser.Authid);

        var postedPlayer = await playerService.GetPlayerByAuthIdAsync("12");

        var retrievedGame = await gameService.GetGameByIdAsync(game.Id);
        var session = await gameService.JoinGameAsync(retrievedGame.Id, postedPlayer.Id);
        
        //figure out how we're going to update the values in GameService for the game sessions
        //call a method that updates the GameService dictionary?

        await gameService.EndGameAsync(game.Id);

        

    }

}