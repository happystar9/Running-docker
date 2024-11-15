using TetrisWeb.ApiServices;
using TetrisWeb.ApiServices.Interfaces;
using TetrisWeb.DTOs;
using TetrisWeb.GameData;
using TetrisWeb.Services;

namespace TetrisWeb.Services
{
    public class GameSessionManager(IApiKeyManagementService keyManager)
    {
        GameSessionDto game;
        public readonly Dictionary<string, GameSessionService> _games = new();

        public async Task<GameSessionService> CreateGame(int playerId)
        {
            var apiKey = await keyManager.AssignKeyAsync(playerId);
            if (!_games.TryGetValue(apiKey, out var gameService))
            {
                gameService = new GameSessionService(game);
                _games[apiKey] = gameService;
            }
            return gameService;
        }

        public async Task<GameSessionService> CreateGame()
        {
            var gameService = new GameSessionService(game);

            return gameService;
        }
        public void EndGame(string playerId)
        {
            _games.Remove(playerId);
        }
    }
}
