using TetrisWeb.GameData;
using TetrisWeb.DTOs;
using TetrisWeb.Components;

namespace TetrisWeb.ApiServices.Interfaces;

public interface IGameService
{
    Task<Game> CreateGameAsync(string createdByAuthId);
    Task<GameSessionDto> JoinGameAsync(int gameId, int playerId, GameLoop gameLoop);
    Task EndGameAsync(int gameId);
    Task<List<Game>> GetAllGamesAsync();
    Task<List<Game>> GetAllLiveGamesAsync();
    Task<Game> GetGameByIdAsync(int gameId);
    Task<List<Game>> GetAllPastGamesAsync();

}


