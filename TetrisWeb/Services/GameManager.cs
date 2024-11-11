

namespace TetrisWeb.Services
{
    public class GameManager()
    {
        private readonly Dictionary<string, GameStateService> _games = new();

        public GameStateService GetOrCreateGame(string playerId)
        {
            if ()
            if (!_games.TryGetValue(playerId, out var gameService))
            {
                gameService = new GameStateService();
                _games[playerId] = gameService;
            }
            return gameService;
        }

        public void EndGame(string playerId)
        {
            _games.Remove(playerId);
        }
    }
}
