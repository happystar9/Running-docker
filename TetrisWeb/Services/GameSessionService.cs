using TetrisWeb.GameData;
using TetrisWeb.DTOs;

namespace TetrisWeb.Services;


public class GameSessionService
{
    public readonly Dictionary<string, GameSession> _gameSessions = new();

 
}

public interface IGameSessionManager
{

}
