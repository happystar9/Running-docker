using TetrisWeb.Components;
using TetrisWeb.Components.Models;

namespace TetrisWeb.ApiServices.Interfaces;

public interface IGameSessionService
{
    void CreateGameSession(int playerId, int gameId);
    void DeleteAllInGame(int gameId);
    void DeleteGameSession(int playerId, int gameId);
    Task<GameLoop?> GetGameSession(int playerId, int gameId);

}