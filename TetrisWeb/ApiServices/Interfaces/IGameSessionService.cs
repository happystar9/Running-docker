using TetrisWeb.Components;
using TetrisWeb.Components.Models;

namespace TetrisWeb.ApiServices.Interfaces;

public interface IGameSessionService
{
    Task CreateGameSession(int playerId, int gameId);
    Task DeleteAllInGame(int gameId);
    Task DeleteGameSession(int playerId, int gameId);
    Task<GameLoop?> GetGameSession(int playerId, int gameId);

}