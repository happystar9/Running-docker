//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Xunit.Sdk;
//using TetrisWeb.DTOs;
//using TetrisWeb.GameData;
//using TetrisWeb.ApiServices;
//using System.Numerics;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.AspNetCore.Builder;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Mvc.Testing;
//using Microsoft.AspNetCore.TestHost;
//using Microsoft.Extensions.DependencyInjection.Extensions;
//using Testcontainers.PostgreSql;
//using Xunit.Abstractions;
//using Microsoft.Extensions.DependencyInjection;
//using System.Net.Http.Json;
//using FluentAssertions;
//using System.Net.Http;
//using TetrisWeb.ApiServices.Interfaces;

//namespace TetrisTest;

//public class PostgresTesting : IClassFixture<WebApplicationFactory<Program>>, IAsyncLifetime
//{

//    internal WebApplicationFactory<Program> customWebAppFactory;
//    private static PostgreSqlContainer _dbContainer;
//    internal readonly ITestOutputHelper outputHelper;
//    internal readonly HttpClient client;
//    private  IPlayerService playerService;


//    public PostgresTesting(WebApplicationFactory<Program> webAppFactory, ITestOutputHelper helper)
//    {
//        outputHelper = helper;

//        _dbContainer = new PostgreSqlBuilder()
//            .WithImage("postgres")
//            .WithPortBinding("1645")
//            .WithPassword("Strong_password_123!")
//            .Build();

//        customWebAppFactory = webAppFactory.WithWebHostBuilder(builder =>
//        {
//            builder.UseSetting("DB_CONN", _dbContainer.GetConnectionString());
//            builder.ConfigureTestServices(services =>
//            {
//                //services.Configure
//                services.RemoveAll<Dbf25TeamArzContext>();
//                services.RemoveAll<DbContextOptions>();
//                services.RemoveAll(typeof(DbContextOptions<Dbf25TeamArzContext>));
//                services.AddDbContext<Dbf25TeamArzContext>(options => options.UseNpgsql(_dbContainer.GetConnectionString()));
//            });
//        });


//    }
//    public async Task InitializeAsync()
//    {
//        await _dbContainer.StartAsync();

//        var projectRoot = Directory.GetParent(Directory.GetParent(Directory.GetParent(Directory.GetParent(Directory.GetCurrentDirectory()).FullName).FullName).FullName).FullName;

//        var schemaFilePath = Path.Combine(projectRoot, "schema.sql");

//        outputHelper.WriteLine($"Schema file path: {schemaFilePath}");

//        var schemaScript = await File.ReadAllTextAsync(schemaFilePath);

//        await _dbContainer.ExecScriptAsync(schemaScript);
//    }

//    public async Task DisposeAsync()
//    {
//        await _dbContainer.StopAsync();
//    }

//    [Fact]
//    public async Task RegisterPlayerCreatesPlayer()
//    {
//        HttpClient client = customWebAppFactory.CreateClient();


//        outputHelper.WriteLine(_dbContainer.GetConnectionString());

//        var samplePlayer = new PlayerDto
//        {
//            Username = "Testing99",
//            Authid = "Testing99",
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


//    [Fact]
//    public async Task UpdatePlayerChangesPlayerDetails()
//    {
//        var originalPlayer = new PlayerDto
//        {
//            Username = "Testing99",
//            Authid = "Testing99",
//            PlayerQuote = "TestQuote",
//            AvatarUrl = "TestAvatarUrl",
//            Isblocked = false
//        };

//        //var addResult = await playerService.CreatePlayerAsync(originalPlayer);

//        var updatedPlayer = new PlayerDto
//        {
//            Username = "Testing99",
//            Authid = "Testing99",
//            PlayerQuote = "TestQuote2",
//            AvatarUrl = "TestAvatarUrl2",
//            Isblocked = true
//        };

//        //update this to use the http client

//        //var updateResult = await playerService.UpdatePlayerAsync(updatedPlayer);

//        //var retrievedPlayer = await playerService.GetPlayerByAuthIdAsync("Testing99");

//        //updateResult.Authid.Should().Be(retrievedPlayer.Authid);
//        //updateResult.PlayerQuote.Should().Be(retrievedPlayer.PlayerQuote);
//        //updateResult.AvatarUrl.Should().Be(retrievedPlayer.AvatarUrl);
//        //updateResult.Isblocked.Should().Be(retrievedPlayer.Isblocked);

//        //Assert.Fail();

//    }
//}