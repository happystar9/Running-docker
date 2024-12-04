using System.Collections.Concurrent;
using TetrisWeb.GameData;
using Microsoft.EntityFrameworkCore;
using TetrisWeb.DTOs;
using TetrisWeb.ApiServices.Interfaces;

namespace TetrisWeb.ApiServices;

public class GameService(Dbf25TeamArzContext context, IPlayerService playerService, IApiKeyManagementService ApiKeyService) : IGameService
{
    private readonly ConcurrentDictionary<string, GameSessionDto> _gameSessions = new();
    private readonly int maxPlayersPerGame = 99; // Example max limit for players
    private List<GameSessionService> gameSessionList = new();
    Random random = new Random();

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

    

    public async Task<GameSessionDto> JoinGameAsync(int gameId, int playerId, GameSessionService gameSession)
    {

        //verify that the player is not blocked before letting them join
        var player = playerService.GetPlayerByIdAsync(playerId).Result;
        if (!player.Isblocked)
        {

            gameSessionList.Add(gameSession);
            gameSession.SendGarbage += HandleSendGarbage;
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

    public void HandleSendGarbage(int lines)
    {
        Task.Run(async () => await gameSessionList[random.Next(gameSessionList.Count)].AddGarbage(lines));
    }
}


