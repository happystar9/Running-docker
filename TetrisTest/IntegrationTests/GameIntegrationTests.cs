//using Microsoft.AspNetCore.Mvc.Testing;
//using TetrisWeb.ApiServices.Interfaces;
//using TetrisWeb.ApiServices;
//using TetrisWeb.DTOs;
//using Xunit.Abstractions;
//using FluentAssertions;
//using TetrisWeb.Components;
//using Microsoft.Extensions.DependencyInjection;

//namespace TetrisTest.IntegrationTests;

//[Collection("SequentialTestExecution")]
//public class GameIntegrationTests : PostgresTestBase
//{
//    public GameIntegrationTests(WebApplicationFactory<Program> webAppFactory, ITestOutputHelper outputHelper)
//       : base(webAppFactory, outputHelper) { }
//    [Fact]
//    public async Task JoinAndEndGameUpdatesGameDetails()
//    {
//        var playerService = GetService<IPlayerService>();
//        var gameSessionService = GetService<GameSessionService>();
//        var gameService = GetService<IGameService>();

//        var samplePlayer = new PlayerDto
//        {
//            Username = "Player12",
//            Authid = "12",
//            PlayerQuote = "TestQuote",
//            AvatarUrl = "TestAvatarUrl",
//            Isblocked = false
//        };

//        var adminUser = new PlayerDto
//        {
//            Username = "TestAuthUser",
//            Authid = "TestAuthId",
//            PlayerQuote = "TestQuote",
//            AvatarUrl = "TestAvatarUrl",
//            Isblocked = false
//        };

//        await playerService.CreatePlayerAsync(samplePlayer);
//        await playerService.CreatePlayerAsync(adminUser);

//        var game = await gameService.CreateGameAsync(adminUser.Authid);

//        var postedPlayer = await playerService.GetPlayerByAuthIdAsync("12");
       
//        //TO-DO: refactor this to fit the new implementation
//        var retrievedGame = await gameService.GetGameByIdAsync(game.Id);
//        var session = await gameService.JoinGameAsync(retrievedGame.Id, postedPlayer.Id, new GameLoop());
//        await gameService.EndGameAsync(game.Id);

//        retrievedGame.GameSessions.Count.Should().Be(1);
//        retrievedGame.GameSessions.Should().NotBeNullOrEmpty();
//        retrievedGame.GameSessions.Count.Should().Be(1);

//    }

//}
