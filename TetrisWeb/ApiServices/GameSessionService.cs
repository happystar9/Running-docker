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
    private Dictionary<(int playerId, int gameId),GameLoop> gameLoops = new();
    private Random random = new Random();
    public void CreateGameSession(int playerId, int gameId)
    {
        GameLoop gameLoop = new GameLoop();
        gameLoops.TryAdd((playerId, gameId), gameLoop);
        gameLoop.SendGarbage += HandleSendGarbage;
        gameLoop.playerId = playerId;
        gameLoop.gameId = gameId;
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

    public async Task HandleSendGarbage(int lines, int gameId)
    {
        var keys = gameLoops.Keys.Where(key => key.gameId == gameId).ToList();

        if (keys.Count > 0)
        {
            var randomKey = keys[random.Next(keys.Count)];
            await gameLoops[randomKey].AddGarbage(lines);
        }
    }
}
