using TetrisWeb.DTOs;
namespace TetrisWeb.ApiServices.Interfaces;

public interface IPlayerService
{
    Task<PlayerDto> GetPlayerAsync(Guid playerId);
    Task<PlayerDto> CreatePlayerAsync(PlayerDto player);
    Task<PlayerDto> UpdatePlayerAsync(PlayerDto player);
}