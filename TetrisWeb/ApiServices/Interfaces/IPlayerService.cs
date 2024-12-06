using TetrisWeb.DTOs;
namespace TetrisWeb.ApiServices.Interfaces;

public interface IPlayerService
{
    Task<PlayerDto> GetPlayerByAuthIdAsync(string authId);
    Task<PlayerDto> GetPlayerByUsernameAsync(string username);
    Task<PlayerDto> GetPlayerByIdAsync(int id);
    Task<List<PlayerDto>> GetAllPlayersAsync();
    Task<PlayerDto> CreatePlayerAsync(PlayerDto player);
    Task<PlayerDto> UpdatePlayerAsync(PlayerDto player);
    Task<int> GetPlayerTotalScore(string authId);
}