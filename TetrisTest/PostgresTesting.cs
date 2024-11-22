using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Sdk;
using TetrisWeb.DTOs;
using TetrisWeb.GameData;
using TetrisWeb.ApiServices;
using System.Numerics;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Testcontainers.PostgreSql;
using Xunit.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Json;
using FluentAssertions;
using System.Net.Http;
using TetrisWeb.ApiServices.Interfaces;

namespace TetrisTest;

public class PostgresTesting : IClassFixture<WebApplicationFactory<Program>>, IAsyncLifetime
{
    internal const string schema = """"
                -- DROP SCHEMA game;

        CREATE SCHEMA game;

        -- DROP SEQUENCE game.api_key_id_seq;

        CREATE SEQUENCE game.api_key_id_seq
        	INCREMENT BY 1
        	MINVALUE 1
        	MAXVALUE 2147483647
        	START 1
        	CACHE 1
        	NO CYCLE;
        -- DROP SEQUENCE game.chat_id_seq;

        CREATE SEQUENCE game.chat_id_seq
        	INCREMENT BY 1
        	MINVALUE 1
        	MAXVALUE 2147483647
        	START 1
        	CACHE 1
        	NO CYCLE;
        -- DROP SEQUENCE game.game_id_seq;

        CREATE SEQUENCE game.game_id_seq
        	INCREMENT BY 1
        	MINVALUE 1
        	MAXVALUE 2147483647
        	START 1
        	CACHE 1
        	NO CYCLE;
        -- DROP SEQUENCE game.game_session_id_seq;

        CREATE SEQUENCE game.game_session_id_seq
        	INCREMENT BY 1
        	MINVALUE 1
        	MAXVALUE 2147483647
        	START 1
        	CACHE 1
        	NO CYCLE;
        -- DROP SEQUENCE game.leaderboard_id_seq;

        CREATE SEQUENCE game.leaderboard_id_seq
        	INCREMENT BY 1
        	MINVALUE 1
        	MAXVALUE 2147483647
        	START 1
        	CACHE 1
        	NO CYCLE;
        -- DROP SEQUENCE game.player_id_seq;

        CREATE SEQUENCE game.player_id_seq
        	INCREMENT BY 1
        	MINVALUE 1
        	MAXVALUE 2147483647
        	START 1
        	CACHE 1
        	NO CYCLE;-- game.player definition

        -- Drop table

        -- DROP TABLE game.player;

        CREATE TABLE game.player (
        	id int4 GENERATED ALWAYS AS IDENTITY( INCREMENT BY 1 MINVALUE 1 MAXVALUE 2147483647 START 1 CACHE 1 NO CYCLE) NOT NULL,
        	authid varchar(450) NOT NULL,
        	player_quote varchar(80) NULL,
        	avatar_url varchar(512) NULL,
        	isblocked bool DEFAULT false NULL,
        	username varchar(25) NULL,
        	CONSTRAINT player_authid_key UNIQUE (authid),
        	CONSTRAINT player_pkey PRIMARY KEY (id)
        );


        -- game.api_key definition

        -- Drop table

        -- DROP TABLE game.api_key;

        CREATE TABLE game.api_key (
        	id int4 GENERATED ALWAYS AS IDENTITY( INCREMENT BY 1 MINVALUE 1 MAXVALUE 2147483647 START 1 CACHE 1 NO CYCLE) NOT NULL,
        	player_id int4 NULL,
        	"key" varchar(256) NOT NULL,
        	created_on timestamp NOT NULL,
        	expired_on timestamp NULL,
        	CONSTRAINT api_key_pkey PRIMARY KEY (id),
        	CONSTRAINT api_key_player_id_fkey FOREIGN KEY (player_id) REFERENCES game.player(id)
        );


        -- game.chat definition

        -- Drop table

        -- DROP TABLE game.chat;

        CREATE TABLE game.chat (
        	id int4 GENERATED ALWAYS AS IDENTITY( INCREMENT BY 1 MINVALUE 1 MAXVALUE 2147483647 START 1 CACHE 1 NO CYCLE) NOT NULL,
        	player_id int4 NOT NULL,
        	message varchar(150) NULL,
        	time_sent timestamp NULL,
        	CONSTRAINT chat_pkey PRIMARY KEY (id),
        	CONSTRAINT chat_player_id_fkey FOREIGN KEY (player_id) REFERENCES game.player(id)
        );


        -- game.game definition

        -- Drop table

        -- DROP TABLE game.game;

        CREATE TABLE game.game (
        	id int4 GENERATED ALWAYS AS IDENTITY( INCREMENT BY 1 MINVALUE 1 MAXVALUE 2147483647 START 1 CACHE 1 NO CYCLE) NOT NULL,
        	created_by_auth_id varchar(255) NOT NULL,
        	start_time timestamp NOT NULL,
        	stop_time timestamp NULL,
        	player_count int4 NULL,
        	CONSTRAINT game_pkey PRIMARY KEY (id),
        	CONSTRAINT game_created_by_auth_id_fkey FOREIGN KEY (created_by_auth_id) REFERENCES game.player(authid)
        );


        -- game.game_session definition

        -- Drop table

        -- DROP TABLE game.game_session;

        CREATE TABLE game.game_session (
        	id int4 GENERATED ALWAYS AS IDENTITY( INCREMENT BY 1 MINVALUE 1 MAXVALUE 2147483647 START 1 CACHE 1 NO CYCLE) NOT NULL,
        	game_id int4 NULL,
        	player_id int4 NULL,
        	score int4 NOT NULL,
        	CONSTRAINT game_session_pkey PRIMARY KEY (id),
        	CONSTRAINT game_session_game_id_fkey FOREIGN KEY (game_id) REFERENCES game.game(id),
        	CONSTRAINT game_session_player_id_fkey FOREIGN KEY (player_id) REFERENCES game.player(id)
        );


        -- game.leaderboard definition

        -- Drop table

        -- DROP TABLE game.leaderboard;

        CREATE TABLE game.leaderboard (
        	id int4 GENERATED ALWAYS AS IDENTITY( INCREMENT BY 1 MINVALUE 1 MAXVALUE 2147483647 START 1 CACHE 1 NO CYCLE) NOT NULL,
        	player_id int4 NOT NULL,
        	total_score int8 NULL,
        	games_played int4 NULL,
        	win_count int4 NULL,
        	created_at timestamp NULL,
        	CONSTRAINT leaderboard_pkey PRIMARY KEY (id),
        	CONSTRAINT leaderboard_player_id_fkey FOREIGN KEY (player_id) REFERENCES game.player(id)
        );

        """";

    internal WebApplicationFactory<Program> customWebAppFactory;
    private static PostgreSqlContainer _dbContainer;
    internal readonly ITestOutputHelper outputHelper;
    internal readonly HttpClient client;
    private  IPlayerService playerService;


    public PostgresTesting(WebApplicationFactory<Program> webAppFactory, ITestOutputHelper helper)
    {
        outputHelper = helper;

        _dbContainer = new PostgreSqlBuilder()
            .WithImage("postgres")
            .WithPortBinding("1645")
            .WithPassword("Strong_password_123!")
            .Build();

        customWebAppFactory = webAppFactory.WithWebHostBuilder(builder =>
        {
            builder.UseSetting("DB_CONN", _dbContainer.GetConnectionString());
            builder.ConfigureTestServices(services =>
            {
                //services.Configure
                services.RemoveAll<Dbf25TeamArzContext>();
                services.RemoveAll<DbContextOptions>();
                services.RemoveAll(typeof(DbContextOptions<Dbf25TeamArzContext>));
                services.AddDbContext<Dbf25TeamArzContext>(options => options.UseNpgsql(_dbContainer.GetConnectionString()));
            });
        });


    }
    public async Task InitializeAsync()
    {
        await _dbContainer.StartAsync();
        
        var yourInitScriptContents = schema;
        await _dbContainer.ExecScriptAsync(yourInitScriptContents);
    }

    public async Task DisposeAsync()
    {
        await _dbContainer.StopAsync();
    }

    [Fact]
    public async Task RegisterPlayerCreatesPlayer()
    {
        HttpClient client = customWebAppFactory.CreateClient();


        outputHelper.WriteLine(_dbContainer.GetConnectionString());

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


    [Fact]
    public async Task UpdatePlayerChangesPlayerDetails()
    {
        var originalPlayer = new PlayerDto
        {
            Username = "Testing99",
            Authid = "Testing99",
            PlayerQuote = "TestQuote",
            AvatarUrl = "TestAvatarUrl",
            Isblocked = false
        };

        //var addResult = await playerService.CreatePlayerAsync(originalPlayer);

        var updatedPlayer = new PlayerDto
        {
            Username = "Testing99",
            Authid = "Testing99",
            PlayerQuote = "TestQuote2",
            AvatarUrl = "TestAvatarUrl2",
            Isblocked = true
        };

        //update this to use the http client

        //var updateResult = await playerService.UpdatePlayerAsync(updatedPlayer);

        //var retrievedPlayer = await playerService.GetPlayerByAuthIdAsync("Testing99");

        //updateResult.Authid.Should().Be(retrievedPlayer.Authid);
        //updateResult.PlayerQuote.Should().Be(retrievedPlayer.PlayerQuote);
        //updateResult.AvatarUrl.Should().Be(retrievedPlayer.AvatarUrl);
        //updateResult.Isblocked.Should().Be(retrievedPlayer.Isblocked);

        //Assert.Fail();

    }
}