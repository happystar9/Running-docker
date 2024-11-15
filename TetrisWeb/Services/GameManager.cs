//using TetrisWeb.ApiServices;
//using TetrisWeb.Services;

//namespace TetrisWeb.Services;

//public class GameManager(ApiKeyManagementService keyManager)
//{
//    public readonly Dictionary<string, GameStateService> _gameSessions = new();

//    public async Task<GameStateService> CreateGame(int playerId)
//    {
//        var apiKey = await keyManager.AssignKeyAsync(playerId);
//        if (!_gameSessions.TryGetValue(apiKey, out var gameService))
//        {
//            gameService = new GameStateService();
//            _gameSessions[apiKey] = gameService;
//        }
//        return gameService;
//    }

//    public void EndGame(string playerId)
//    {
//        _gameSessions.Remove(playerId);
//    }
//}
