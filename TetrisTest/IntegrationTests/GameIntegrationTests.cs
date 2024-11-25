//using Microsoft.AspNetCore.Mvc.Testing;
//using TetrisWeb.ApiServices.Interfaces;
//using TetrisWeb.ApiServices;
//using TetrisWeb.DTOs;
//using Xunit.Abstractions;
//using FluentAssertions;
//using Microsoft.Extensions.DependencyInjection;

//namespace TetrisTest.IntegrationTests;

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

//        var authUser = new PlayerDto
//        {
//            Username = "TestAuthUser",
//            Authid = "TestAuthId",
//            PlayerQuote = "TestQuote",
//            AvatarUrl = "TestAvatarUrl",
//            Isblocked = false
//        };

//        await playerService.CreatePlayerAsync(samplePlayer);
//        await playerService.CreatePlayerAsync(authUser);

//        var game = await gameService.CreateGameAsync("TestAuthId");

//        var postedPlayer = await playerService.GetPlayerByAuthIdAsync("12");


//        var session = await gameService.JoinGameAsync(game.Id, postedPlayer.Id);
//        await gameService.EndGameAsync(game.Id);
//        var savedGames = await gameService.GetAllGamesAsync();

//        savedGames.Should().NotBeNullOrEmpty();
//        savedGames.Count.Should().Be(1);
//        savedGames.Should().Contain(g => g.PlayerCount == 1);

//        var retrievedGame = savedGames.Where(g => g.PlayerCount == 1).First();
//        retrievedGame.GameSessions.Should().NotBeNullOrEmpty();
//        retrievedGame.GameSessions.Count.Should().Be(1);

//        //var totalScore = await playerService.GetPlayerTotalScore(postedPlayer.Authid);
//        //totalScore.Should().Be(10);
//    }

//}
