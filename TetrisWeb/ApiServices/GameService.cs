﻿using System.Collections.Concurrent;
using TetrisWeb.GameData;
using Microsoft.EntityFrameworkCore;
using TetrisWeb.DTOs;
using TetrisWeb.ApiServices.Interfaces;
using TetrisWeb.Components;

namespace TetrisWeb.ApiServices;

public class GameService(Dbf25TeamArzContext context, IPlayerService playerService, IApiKeyManagementService ApiKeyService, GameSessionService _gameSessionService) : IGameService
{
    public ConcurrentDictionary<string, GameSessionDto> _gameSessions = new();
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

    public async Task<GameSessionDto> JoinGameAsync(int gameId, int playerId, GameLoop gameLoop)
    {

        //verify that the player is not blocked before letting them join
        var player = await playerService.GetPlayerByIdAsync(playerId);

        if (!player.Isblocked)
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
        else
        {
            throw new InvalidOperationException("Blocked players cannot join games.");
        }

    }


    public async Task EndGameAsync(int gameId)
    {
        var sessions = await _gameSessionService.GetAllGameSessionsByGameIdAsync(gameId);

        var game = await context.Games.Include(g => g.GameSessions).FirstOrDefaultAsync(g => g.Id == gameId);
        if (game == null)
        {
            throw new KeyNotFoundException("Game not found.");
        }

        game.StopTime = DateTime.Now;
        game.PlayerCount = sessions.Count();


        
        foreach (var session in sessions)
        {
            // Save session data to the database
            if (session != null)
            {
                var gameSession = new GameSession()
                {
                    GameId = session.gameId,
                    PlayerId = session.playerId,
                    Score = Math.Max(session.HighScore, session.Score)
                };

                context.GameSessions.Add(gameSession);
            }
        }

        await context.SaveChangesAsync();

        _gameSessionService.DeleteAllInGame(gameId);
    }


    public async Task<List<Game>> GetAllGamesAsync()
    {
        return await context.Games.ToListAsync();
    }

    public async Task<List<Game>> GetAllLiveGamesAsync()
    {
        return await context.Games.Where(g => g.StopTime == null).ToListAsync();
    }

    public async Task<List<Game>> GetAllPastGamesAsync()
    {
        return await context.Games.Where(g => g.StopTime != null).ToListAsync();

    }

    public async Task<Game> GetGameByIdAsync(int gameId)
    {
        return await context.Games.FirstOrDefaultAsync(g => g.Id == gameId);
    }


}


