using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components;
using System;
using TetrisWeb.Components.Models;
using System.Net.NetworkInformation;
using TetrisWeb.Components.Pages.Partials;
using System.Collections.Concurrent;
using TetrisWeb.GameData;
using Microsoft.EntityFrameworkCore;

namespace TetrisWeb.ApiServices;

public class GameService(Dbf25TeamArzContext context)
{
    private readonly ConcurrentDictionary<string, Game> _gameSessions = new();
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

    public async Task EndGameAsync(int gameId)
    {
        var game = await context.Games.Include(g => g.GameSessions).FirstOrDefaultAsync(g => g.Id == gameId);
        if (game == null)
        {
            throw new KeyNotFoundException("Game not found.");
        }

        game.StopTime = DateTime.Now;
        await context.SaveChangesAsync();
    }
}


