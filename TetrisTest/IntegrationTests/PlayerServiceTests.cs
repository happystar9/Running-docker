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
}

