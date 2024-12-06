using System;
using System.Data;
using TetrisWeb.ApiServices.Interfaces;
using TetrisWeb.Components;
using TetrisWeb.Components.Models;
using TetrisWeb.DTOs;

namespace TetrisWeb.ApiServices;

public class GameSessionService : IGameSessionService
{
    private Dictionary<(int playerId, int gameId),GameLoop> gameLoops = new();
    private Random random = new Random();
    public void CreateGameSession(int playerId, int gameId)
    {
        GameLoop gameLoop = new GameLoop();
        gameLoops.TryAdd((playerId, gameId), gameLoop);
        gameLoop.SendGarbage += HandleSendGarbage;
        return;
    }

    public void DeleteGameSession(int playerId, int gameId)
    {
        gameLoops.Remove((playerId,gameId));
        return;
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
}
