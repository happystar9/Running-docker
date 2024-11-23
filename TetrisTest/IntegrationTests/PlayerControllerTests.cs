using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using TetrisWeb.DTOs;
using Xunit.Abstractions;
using FluentAssertions;

namespace TetrisTest.IntegrationTests;


public class PlayerControllerTests : PostgresTestBase
{
    public PlayerControllerTests(WebApplicationFactory<Program> webAppFactory, ITestOutputHelper outputHelper)
        : base(webAppFactory, outputHelper) { }

    [Fact]
    public async Task RegisterPlayerCreatesPlayer()
    {
        var client = CustomWebAppFactory.CreateClient();

        var samplePlayer = new PlayerDto
        {
            Username = "Testing99",
            Authid = "Testing99",
            PlayerQuote = "TestQuote",
            AvatarUrl = "TestAvatarUrl",
            Isblocked = false
        };

        var response = await client.PostAsJsonAsync("/api/player/register", samplePlayer);
        response.EnsureSuccessStatusCode();

        var createdPlayer = await response.Content.ReadFromJsonAsync<PlayerDto>();

        createdPlayer.Should().NotBeNull();
        createdPlayer.Username.Should().Be(samplePlayer.Username);
        createdPlayer.Authid.Should().Be(samplePlayer.Authid);
        createdPlayer.PlayerQuote.Should().Be(samplePlayer.PlayerQuote);
        createdPlayer.AvatarUrl.Should().Be(samplePlayer.AvatarUrl);
        createdPlayer.Isblocked.Should().BeFalse();
        createdPlayer.Id.Should().BeGreaterThan(0);
    }
}
