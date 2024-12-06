using System.Data;
using TetrisWeb.ApiServices.Interfaces;
using TetrisWeb.Components;
using TetrisWeb.Components.Models;
using TetrisWeb.DTOs;

namespace TetrisWeb.ApiServices;

public class GameSessionService : IGameSessionService
{
    private Dictionary<(int playerId, int gameId),GameLoop> gameLoops = new();
    public async Task CreateGameSession(int playerId, int gameId)
    {
        GameLoop gameLoop = new GameLoop();
        gameLoops.TryAdd((playerId, gameId), gameLoop);
        return;
    }

    public async Task DeleteGameSession(int playerId, int gameId)
    {
        gameLoops.Remove((playerId,gameId));
        return;
    }

    public async Task DeleteAllInGame(int gameId)
    {
        var keys = gameLoops.Keys.Where(key => key.gameId == gameId);
        foreach (var key in keys) 
        { 
            gameLoops.Remove(key);
        }
    }

    public async Task<GameLoop> GetGameSession(int playerId, int gameId)
    {
        if (gameLoops.TryGetValue((playerId, gameId), out var gameLoop))
        {
            return gameLoops[(playerId, gameId)];
        }
        return null;
    }
}
