using TetrisWeb.DTOs;
namespace TetrisWeb.ApiServices.Interfaces;

public interface IPlayerService
{
    Task<PlayerDto> GetPlayerAsync(string authId);
    Task<PlayerDto> CreatePlayerAsync(PlayerDto player);
    Task<PlayerDto> UpdatePlayerAsync(PlayerDto player);
    Task<int> GetPlayerTotalScore(string authId);
}