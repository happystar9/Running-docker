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

namespace TetrisWeb.ApiServices;

public class GameService(Dbf25TeamArzContext context) : IGameService
{
    private readonly ConcurrentDictionary<string, GameDto> _gameSessions = new();
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

	

	public async Task<GameSession> JoinGameAsync(int gameId, int playerId)
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

        var session = new GameSession()
        {
            GameId = game.Id,
            PlayerId = playerId,
            Score = 0
        };

        game.GameSessions.Add(session);
        game.PlayerCount++;

        await context.SaveChangesAsync();
        return session;

    }

    //adjusted this method because it was adding the gameSessions to game again
    //we'll see if it breaks anything

    public async Task EndGameAsync(int gameId)
    {
        var game = await context.Games
            .Include(g => g.GameSessions)
            .FirstOrDefaultAsync(g => g.Id == gameId);
        if (game == null)
        {
            throw new KeyNotFoundException("Game not found.");
        }

        game.StopTime = DateTime.Now;

        await context.SaveChangesAsync();
    }


    public async Task<List<Game>> GetAllGamesAsync(){
        return await context.Games.ToListAsync();
        
    }

    public async Task<List<Game>> GetAllLiveGamesAsync(){
        return await context.Games.Where(g => g.StopTime == null).ToListAsync();
    }
}


