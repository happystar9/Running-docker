//using FluentAssertions;
//using Microsoft.AspNetCore.Mvc.Testing;
//using TetrisWeb.DTOs;
//using Xunit.Abstractions;
//using TetrisWeb.ApiServices.Interfaces;
//using TetrisWeb.ApiServices;

//namespace TetrisTest.IntegrationTests;

//[Collection("SequentialTestExecution")]
//public class PlayerServiceTests : PostgresTestBase
//{
//    public PlayerServiceTests(WebApplicationFactory<Program> webAppFactory, ITestOutputHelper outputHelper)
//        : base(webAppFactory, outputHelper) { }

//    [Fact]
//    public async Task CanRegisterPlayerUsingService()
//    {
//        var playerService = GetService<IPlayerService>();

//        var samplePlayer = new PlayerDto
//        {
//            Username = "Testing98",
//            Authid = "Testing98",
//            PlayerQuote = "TestQuote",
//            AvatarUrl = "TestAvatarUrl",
//            Isblocked = false
//        };

//        var createdPlayer = await playerService.CreatePlayerAsync(samplePlayer);

//        createdPlayer.Should().NotBeNull();
//        createdPlayer.Username.Should().Be(samplePlayer.Username);
//        createdPlayer.Authid.Should().Be(samplePlayer.Authid);
//        createdPlayer.PlayerQuote.Should().Be(samplePlayer.PlayerQuote);
//        createdPlayer.AvatarUrl.Should().Be(samplePlayer.AvatarUrl);
//        createdPlayer.Isblocked.Should().BeFalse();
//        createdPlayer.Id.Should().BeGreaterThan(0);
//    }

//    [Fact]
//    public async Task CanGetPlayerByAuthId()
//    {
//        var playerService = GetService<IPlayerService>();

//        //arrange - create 10 players with different authIds
//        for (int i = 0; i < 10; i++)
//        {
//            var samplePlayer = new PlayerDto
//            {
//                Username = $"Player{i}",
//                Authid = $"{i}",
//                PlayerQuote = "TestQuote",
//                AvatarUrl = "TestAvatarUrl",
//                Isblocked = false
//            };

//            await playerService.CreatePlayerAsync(samplePlayer);
//        }

//        //testing a specific case
//        var player = await playerService.GetPlayerByAuthIdAsync("5");

//        player.Should().NotBeNull();
//        player.Authid.Should().Be("5");
//        player.Username.Should().Be("Player5");

//        //testing all cases based on the test setup
//        for (int i = 0; i < 10; i++)
//        {
//            var p = await playerService.GetPlayerByAuthIdAsync($"{i}");

//            p.Should().NotBeNull();
//            p.Authid.Should().Be($"{i}");
//            p.Username.Should().Be($"Player{i}");
//        }
//    }

//    [Fact]
//    public async Task CanGetPlayerByUsername()
//    {
//        var playerService = GetService<IPlayerService>();

//        var samplePlayer = new PlayerDto
//        {
//            Username = "Player11",
//            Authid = "11",
//            PlayerQuote = "TestQuote",
//            AvatarUrl = "TestAvatarUrl",
//            Isblocked = false
//        };

//        await playerService.CreatePlayerAsync(samplePlayer);

//        var player = await playerService.GetPlayerByUsernameAsync("Player11");

//        player.Should().NotBeNull();
//        player.Authid.Should().Be("11");
//        player.Username.Should().Be("Player11");
//    }


//    [Fact]
//    public async Task CanUpdateExistingPlayerDetails()
//    {
//        var playerService = GetService<IPlayerService>();

//        var samplePlayer = new PlayerDto
//        {
//            Username = "Player15",
//            Authid = "15",
//            PlayerQuote = "TestQuote",
//            AvatarUrl = "TestAvatarUrl",
//            Isblocked = false
//        };

//        await playerService.CreatePlayerAsync(samplePlayer);

//        var player = await playerService.GetPlayerByUsernameAsync("Player15");
//        player.Should().NotBeNull();

//        var updatedPlayer = new PlayerDto
//        {
//            Id = player.Id,
//            Username = "UpdatedPlayer15",
//            Authid = "15",
//            PlayerQuote = "UpdatedTestQuote",
//            AvatarUrl = "UpdatedTestAvatarUrl",
//            Isblocked = true
//        };

//        var updated = await playerService.UpdatePlayerAsync(updatedPlayer);

//        updated.Should().NotBeNull();

//        var retrievedPlayer = await playerService.GetPlayerByUsernameAsync("UpdatedPlayer15");
//        retrievedPlayer.Username.Should().Be("UpdatedPlayer15");
//        retrievedPlayer.PlayerQuote.Should().Be("UpdatedTestQuote");
//        retrievedPlayer.AvatarUrl.Should().Be("UpdatedTestAvatarUrl");
//        retrievedPlayer.Isblocked.Should().BeTrue();
//    }


//    [Fact]
//    public async Task CanGetPlayersAllTimeScore()
//    {
//        var playerService = GetService<IPlayerService>();
//        var gameSessionService = GetService<GameSessionService>();
//        var gameService = GetService<IGameService>();

//        var samplePlayer = new PlayerDto
//        {
//            Username = "Player13",
//            Authid = "13",
//            PlayerQuote = "TestQuote",
//            AvatarUrl = "TestAvatarUrl",
//            Isblocked = false
//        };

//        var adminUser = new PlayerDto
//        {
//            Username = "TestAuthUser",
//            Authid = "TestAuthId2",
//            PlayerQuote = "TestQuote",
//            AvatarUrl = "TestAvatarUrl",
//            Isblocked = false
//        };

//        await playerService.CreatePlayerAsync(samplePlayer);
//        await playerService.CreatePlayerAsync(adminUser);

//        var game = await gameService.CreateGameAsync(adminUser.Authid);


//        var postedPlayer = await playerService.GetPlayerByAuthIdAsync("13");

//        Task.Delay(500).Wait();
//        var session = await gameService.JoinGameAsync(game.Id, postedPlayer.Id, new GameSessionService());
//        await gameService.EndGameAsync(game.Id);
//        var savedGames = await gameService.GetAllGamesAsync();

//        savedGames.Should().NotBeNullOrEmpty();
        
//        var retrievedGame = await gameService.GetGameByIdAsync(game.Id);
//        retrievedGame.GameSessions.Should().NotBeNullOrEmpty();
//        retrievedGame.GameSessions.Count.Should().Be(1);

//        //var totalScore = await playerService.GetPlayerTotalScore(postedPlayer.Authid);
//        //totalScore.Should().Be(10);
//    }


//    [Fact]
//    public async Task CanBlockPlayer()
//    {
//        var playerService = GetService<IPlayerService>();

//        var samplePlayer = new PlayerDto
//        {
//            Username = "Player14",
//            Authid = "14",
//            PlayerQuote = "TestQuote",
//            AvatarUrl = "TestAvatarUrl",
//            Isblocked = false
//        };

//        await playerService.CreatePlayerAsync(samplePlayer);

//        var player = await playerService.GetPlayerByUsernameAsync("Player14");

//        player.Should().NotBeNull();
//        player.Isblocked.Should().BeFalse();

//        var updatePlayerDto = new PlayerDto
//        {
//            Id = player.Id,
//            Username = player.Username,
//            Authid = player.Authid,
//            PlayerQuote = player.PlayerQuote,
//            AvatarUrl = player.AvatarUrl,
//            Isblocked = true
//        };

//        var blockedPlayer = await playerService.UpdatePlayerAsync(updatePlayerDto);

//        blockedPlayer.Should().NotBeNull();
//        blockedPlayer.Isblocked.Should().BeTrue();
//    }
//}





////Task<PlayerDto> GetPlayerByAuthIdAsync(string authId);
////Task<PlayerDto> GetPlayerByUsernameAsync(string username);
////Task<PlayerDto> UpdatePlayerAsync(PlayerDto player);
////Task<int> GetPlayerTotalScore(string authId);

