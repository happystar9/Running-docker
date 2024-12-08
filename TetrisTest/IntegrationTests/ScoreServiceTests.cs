using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using TetrisWeb.DTOs;
using Xunit.Abstractions;
using TetrisWeb.ApiServices.Interfaces;
using TetrisWeb.ApiServices;


namespace TetrisTest.IntegrationTests;


[Collection("SequentialTestExecution")]
public class ScoreServiceTests : PostgresTestBase
{
    public ScoreServiceTests(WebApplicationFactory<Program> webAppFactory, ITestOutputHelper outputHelper)
       : base(webAppFactory, outputHelper) { }

    [Fact]
    public async Task CanGetPlayerHighScore()
    {
        var playerService = GetService<IPlayerService>();
        var gameSessionService = GetService<GameSessionService>();
        var gameService = GetService<IGameService>();
        var scoreService = GetService<IScoreService>();


        var samplePlayer = new PlayerDto
        {
            Username = "Player80",
            Authid = "80",
            PlayerQuote = "TestQuote",
            AvatarUrl = "TestAvatarUrl",
            Isblocked = false
        };

        var adminUser = new PlayerDto
        {
            Username = "TestAuthUser5",
            Authid = "TestAuthId5",
            PlayerQuote = "TestQuote",
            AvatarUrl = "TestAvatarUrl",
            Isblocked = false
        };

        await playerService.CreatePlayerAsync(samplePlayer);
        await playerService.CreatePlayerAsync(adminUser);

        var game = await gameService.CreateGameAsync(adminUser.Authid);

        var postedPlayer = await playerService.GetPlayerByAuthIdAsync("80");

        var retrievedGame = await gameService.GetGameByIdAsync(game.Id);
        var session = await gameService.JoinGameAsync(retrievedGame.Id, postedPlayer.Id, new GameSessionService());

        //figure out how we're going to update the values in GameService for the game sessions
        //call a method that updates the GameService dictionary?

        int score = session.Score = 100;
        await gameService.EndGameAsync(game.Id);
        await scoreService.UpdateHighScore(postedPlayer.Id, score);

        await scoreService.GetScoresForGameAsync(game.Id);




    }

}