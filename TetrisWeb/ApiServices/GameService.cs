using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components;
using System;
using TetrisWeb.Components.Models;
using System.Net.NetworkInformation;
using TetrisWeb.Components.Pages.Partials;
using System.Collections.Concurrent;
using TetrisWeb.GameData;
using Microsoft.EntityFrameworkCore;
using TetrisWeb.DTOs;
using System.Linq;
using TetrisWeb.ApiServices.Interfaces;
using TetrisWeb.ApiServices;
using System.Reflection.Emit;

namespace TetrisWeb.ApiServices;

public class GameService(Dbf25TeamArzContext context, IApiKeyManagementService ApiKeyService) : IGameService
{
    private readonly ConcurrentDictionary<string, GameSessionDto> _gameSessions = new();
    private readonly int maxPlayersPerGame = 99; // Example max limit for players

    public async Task<Game> CreateGameAsync(string createdByAuthId)
    {
        var game = new Game()
        {
            CreatedByAuthId = createdByAuthId,
            StartTime = DateTime.Now,
            PlayerCount = 0
        };

        context.Games.Add(game);

        await context.SaveChangesAsync();

        GameDto gameDto = new GameDto
        {
            Id = game.Id,
            CreatedByAuthId = game.CreatedByAuthId,
            StartTime = game.StartTime,
            PlayerCount = game.PlayerCount
        };

        return game;
    }

	

	public async Task<GameSessionDto> JoinGameAsync(int gameId, int playerId)
    {
        var game = await context.Games.Include(g => g.GameSessions).FirstOrDefaultAsync(g => g.Id == gameId);
        if (game == null)
        {
            throw new KeyNotFoundException("Game not found.");
        }
        if (game.PlayerCount > maxPlayersPerGame)
        {
            throw new InvalidOperationException("Game is at max capacity.");

        }

        var session = new GameSessionDto()
        {
            GameId = game.Id,
            PlayerId = playerId,
            Score = 0
        };

        var key = await ApiKeyService.AssignKeyAsync(playerId);
        _gameSessions[key] = session;

        return session;

    }

    public async Task EndGameAsync(int gameId)
    {
        var game = await context.Games
            .Include(g => g.GameSessions)
            .FirstOrDefaultAsync(g => g.Id == gameId);
        if (game == null)
        {
            throw new KeyNotFoundException("Game not found.");
        }

        //post the game itself and its details to the database
        game.StopTime = DateTime.Now;
        game.PlayerCount = _gameSessions.Count;

        //post all of the sessions to the database
        foreach (var session in _gameSessions.Values)
        {
            var gameSession = new GameSession()
            {
                GameId = session.GameId,
                PlayerId = session.PlayerId,
                Score = session.Score
            };

            context.GameSessions.Add(gameSession);
        }

        await context.SaveChangesAsync();

        //do we need to reset this here?
        //_gameSessions = new ConcurrentDictionary<string, GameSessionDto>();
    }


    public async Task<List<Game>> GetAllGamesAsync(){
        return await context.Games.ToListAsync();
    }

    public async Task<List<Game>> GetAllLiveGamesAsync(){
        return await context.Games.Where(g => g.StopTime == null).ToListAsync();
    }

    public async Task<Game> GetGameByIdAsync(int gameId)
    {
        return await context.Games.FirstOrDefaultAsync(g => g.Id == gameId);
    }
}


