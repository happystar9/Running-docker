using System;
using System.Data;
using TetrisWeb.ApiServices.Interfaces;
using TetrisWeb.Components;
using TetrisWeb.AuthData;
using TetrisWeb.Components.Models;
using TetrisWeb.DTOs;
using TetrisWeb.GameData;

namespace TetrisWeb.ApiServices;

public class GameSessionService : IGameSessionService
{
    public int PlayerId { get; set; }
    public GameState GameState { get; set; }
    private Dictionary<(int playerId, int gameId),GameLoop> gameLoops = new();
    private Random random = new Random();
    public void CreateGameSession(int playerId, int gameId)
    {
        GameLoop gameLoop = new GameLoop();
        gameLoops.TryAdd((playerId, gameId), gameLoop);
        gameLoop.SendGarbage += HandleSendGarbage;
    }

    public void DeleteGameSession(int playerId, int gameId)
    {
        gameLoops.Remove((playerId,gameId));
    }

    public void DeleteAllInGame(int gameId)
    {
        if (gameLoops.Count > 0)
        {
            var keys = gameLoops.Keys.Where(key => key.gameId == gameId);
            foreach (var key in keys)
            {
                gameLoops.Remove(key);
            }
        }
        return;
    }

    public async Task<GameLoop?> GetGameSession(int playerId, int gameId)
    {
        if (gameLoops.TryGetValue((playerId, gameId), out var gameLoop))
        {
            return gameLoops[(playerId, gameId)];
        }
        return null;
    }

    public void HandleSendGarbage(int lines, int gameId)
    {
        var keys = gameLoops.Keys.Where(key => key.gameId == gameId).ToList();
        Task.Run(async () => await gameLoops[keys[random.Next(keys.Count())]].AddGarbage(lines));
    }

    public async Task<List<GameLoop>> GetAllGameSessionsByGameIdAsync(int gameId)
    {
        // Get all sessions for a given gameId
        var sessions = gameLoops
            .Where(kv => kv.Key.gameId == gameId)
            .Select(kv => kv.Value)
            .ToList();

        return await Task.FromResult(sessions); // Simulate async task
    }

    public async Task<List<GameSessionDto>> GetAllPlayersStateInGameAsync(int gameId)
    {
        var playersState = gameLoops
            .Where(kv => kv.Key.gameId == gameId)
            .Select(kv => new GameSessionDto
            {
                PlayerId = kv.Key.playerId,
                Score = kv.Value.Score,
                GameState = kv.Value.GameStateGrid.State,
                NextTetromino = kv.Value.nextStyle,
                SecondNextTetromino = kv.Value.secondNextStyle
            })
            .ToList();

        return await Task.FromResult(playersState);
    }
}
