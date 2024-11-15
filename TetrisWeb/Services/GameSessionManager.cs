using TetrisWeb.ApiServices;
using TetrisWeb.ApiServices.Interfaces;
using TetrisWeb.DTOs;

namespace TetrisWeb.Services
{
    public class GameSessionManager(IApiKeyManagementService keyManager)
    {
        GameSessionDto game;
        public readonly Dictionary<string, GameSessionService> _gameSessions = new();

        public async Task<GameSessionService> CreateGameSession(int playerId)
        {
            var apiKey = await keyManager.AssignKeyAsync(playerId);
            if (!_gameSessions.TryGetValue(apiKey, out var gameSessionService))
            {
                gameSessionService = new GameSessionService(game);
                _gameSessions[apiKey] = gameSessionService;
            }
            return gameSessionService;
        }
        public void EndGame(string playerId)
        {
            _gameSessions.Remove(playerId);
        }
    }
}
