using TetrisWeb.GameData;

namespace TetrisWeb.ApiServices.Interfaces;

public interface IGameService
{
    Task<Game> CreateGameAsync(string createdByAuthId);
    Task<GameSession> JoinGameAsync(int gameId, int playerId);
    Task EndGameAsync(int gameId);
    Task<List<Game>> GetAllGamesAsync();
    Task<List<Game>> GetAllLiveGamesAsync();
}


