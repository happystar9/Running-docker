using TetrisWeb.ApiServices;
using TetrisWeb.Services;

namespace TetrisWeb.Services
{
    public class GameManager(ApiKeyManagementService keyManager)
    {
        public readonly Dictionary<string, GameStateService> _games = new();

        public async Task<GameStateService> CreateGame(int playerId)
        {
            var apiKey = await keyManager.AssignKeyAsync(playerId);
            if (!_games.TryGetValue(apiKey, out var gameService))
            {
                gameService = new GameStateService();
                _games[apiKey] = gameService;
            }
            return gameService;
        }

        public void EndGame(string playerId)
        {
            _games.Remove(playerId);
        }
    }
}
