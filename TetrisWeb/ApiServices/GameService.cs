using System.Collections.Concurrent;
using TetrisWeb.GameData;
using Microsoft.EntityFrameworkCore;
using TetrisWeb.DTOs;
using TetrisWeb.ApiServices.Interfaces;
using TetrisWeb.Components;

namespace TetrisWeb.ApiServices;

public class GameService(Dbf25TeamArzContext context, IPlayerService playerService, IApiKeyManagementService ApiKeyService) : IGameService
{
    public ConcurrentDictionary<string, GameSessionDto> _gameSessions = new();
    private readonly int maxPlayersPerGame = 99; // Example max limit for players
    private GameSessionService _gameSessionService = new GameSessionService();

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
        _gameSessionService.DeleteAllInGame(gameId);

        var game = await context.Games.Include(g => g.GameSessions).FirstOrDefaultAsync(g => g.Id == gameId);
        if (game == null)
        {
            throw new KeyNotFoundException("Game not found.");
        }

        game.StopTime = DateTime.Now;
        game.PlayerCount = _gameSessions.Values.Where(game => game.GameId == gameId).Count();

        foreach (var session in _gameSessions.Values)
        {
            // Save session data to the database
            var sessionDto = _gameSessions.Values.FirstOrDefault(s => s.GameId == gameId && s.PlayerId == session.PlayerId);
            if (sessionDto != null)
            {
                var gameSession = new GameSession()
                {
                    GameId = sessionDto.GameId,
                    PlayerId = sessionDto.PlayerId,
                    Score = sessionDto.Score
                };

                context.GameSessions.Add(gameSession);
            }
        }

        await context.SaveChangesAsync();
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


