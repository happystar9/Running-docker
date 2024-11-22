// using FluentAssertions;
// using Microsoft.AspNetCore.Hosting;
// using Microsoft.AspNetCore.Mvc.Testing;
// using Microsoft.Extensions.Configuration;
// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Net.Http.Json;
// using System.Text;
// using System.Threading.Tasks;
// using TetrisWeb.DTOs;
// using Xunit.Abstractions;

// namespace TetrisTest;

// public class PlayerIntegrationTests : PostgresTesting
// {
//    private HttpClient client;

//    public PlayerIntegrationTests(WebApplicationFactory<Program> webAppFactory, ITestOutputHelper helper) : base(webAppFactory, helper)
//    {
//        //var Factory = customWebAppFactory.WithWebHostBuilder(builder =>
//        //{
//        //    builder.UseEnvironment("testing");
//        //});

//        var Factory = webAppFactory.WithWebHostBuilder(builder =>
//        {
//            builder.ConfigureAppConfiguration((context, config) =>
//            {
//                config.AddInMemoryCollection(new Dictionary<string, string>
//        {
//            { "DB_CONN", _dbContainer.GetConnectionString() }
//        });
//            });
//        });

//        client = Factory.CreateClient();
//    }

//    [Fact]
//    public async Task RegisterPlayerCreatesPlayer()
//    {
//        var samplePlayer = new PlayerDto
//        {
//            Username = "TestUser",
//            Authid = "TestAuthId",
//            PlayerQuote = "TestQuote",
//            AvatarUrl = "TestAvatarUrl",
//            Isblocked = false
//        };

//        var response = await client.PostAsJsonAsync("/api/player/register", samplePlayer);

//        response.EnsureSuccessStatusCode();

//        var createdPlayer = await response.Content.ReadFromJsonAsync<PlayerDto>();
//        createdPlayer.Should().NotBeNull();
//        createdPlayer.Username.Should().Be(samplePlayer.Username);
//        createdPlayer.Authid.Should().Be(samplePlayer.Authid);
//        createdPlayer.PlayerQuote.Should().Be(samplePlayer.PlayerQuote);
//        createdPlayer.AvatarUrl.Should().Be(samplePlayer.AvatarUrl);
//        createdPlayer.Isblocked.Should().BeFalse();
//        createdPlayer.Id.Should().BeGreaterThan(0);

//    }

// }
