using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using TetrisWeb.DTOs;
using Xunit.Abstractions;
using TetrisWeb.Services;
using TetrisWeb.ApiServices.Interfaces;

namespace TetrisTest.IntegrationTests;

public class PlayerServiceTests : PostgresTestBase
{
    public PlayerServiceTests(WebApplicationFactory<Program> webAppFactory, ITestOutputHelper outputHelper)
        : base(webAppFactory, outputHelper) { }

    [Fact]
    public async Task CanRegisterPlayerUsingService()
    {
        var playerService = GetService<IPlayerService>();

        var samplePlayer = new PlayerDto
        {
            Username = "Testing98",
            Authid = "Testing98",
            PlayerQuote = "TestQuote",
            AvatarUrl = "TestAvatarUrl",
            Isblocked = false
        };

        var createdPlayer = await playerService.CreatePlayerAsync(samplePlayer);

        createdPlayer.Should().NotBeNull();
        createdPlayer.Username.Should().Be(samplePlayer.Username);
        createdPlayer.Authid.Should().Be(samplePlayer.Authid);
        createdPlayer.PlayerQuote.Should().Be(samplePlayer.PlayerQuote);
        createdPlayer.AvatarUrl.Should().Be(samplePlayer.AvatarUrl);
        createdPlayer.Isblocked.Should().BeFalse();
        createdPlayer.Id.Should().BeGreaterThan(0);
    }

    [Fact]
    public async Task CanGetPlayerByAuthId()
    {
        var playerService = GetService<IPlayerService>();

        //arrange - create 10 players with different authIds
        for (int i = 0; i < 10; i++)
        {
            var samplePlayer = new PlayerDto
            {
                Username = $"Player{i}",
                Authid = $"{i}",
                PlayerQuote = "TestQuote",
                AvatarUrl = "TestAvatarUrl",
                Isblocked = false
            };

            await playerService.CreatePlayerAsync(samplePlayer);
        }

        //testing a specific case
        var player = await playerService.GetPlayerByAuthIdAsync("5");

        player.Should().NotBeNull();
        player.Authid.Should().Be("5");
        player.Username.Should().Be("Player5");

        //testing all cases based on the test setup
        for (int i = 0; i < 10; i++)
        {
            var p = await playerService.GetPlayerByAuthIdAsync($"{i}");

            p.Should().NotBeNull();
            p.Authid.Should().Be($"{i}");
            p.Username.Should().Be($"Player{i}");
        }
    }

    [Fact]
    public async Task CanGetPlayerByUsername()
    {
        var playerService = GetService<IPlayerService>();

        var samplePlayer = new PlayerDto
        {
            Username = "Player11",
            Authid = "11",
            PlayerQuote = "TestQuote",
            AvatarUrl = "TestAvatarUrl",
            Isblocked = false
        };

        await playerService.CreatePlayerAsync(samplePlayer);

        var player = await playerService.GetPlayerByUsernameAsync("Player11");

        player.Should().NotBeNull();
        player.Authid.Should().Be("11");
        player.Username.Should().Be("Player11");
    }


    [Fact]
    public async Task CanUpdateExistingPlayerDetails()
    {
        Assert.Fail();
    }

    [Fact]
    public async Task CanGetPlayersAllTimeScore()
    {
        var playerService = GetService<IPlayerService>();
        var gameSessionService = GetService<GameSessionService>();

        //

    }
}





//Task<PlayerDto> GetPlayerByAuthIdAsync(string authId);
//Task<PlayerDto> GetPlayerByUsernameAsync(string username);
//Task<PlayerDto> UpdatePlayerAsync(PlayerDto player);
//Task<int> GetPlayerTotalScore(string authId);

