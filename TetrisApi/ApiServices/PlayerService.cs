using TetrisWeb.AuthData;
using TetrisShared;
using TetrisShared.DTOs;
namespace TetrisWeb.ApiServices;

public class PlayerService(Dbf25TeamArzContext dbContext) : IPlayerService
{
    public async Task<PlayerDto> CreatePlayerAsync(PlayerDto player)
    {
        var playerObject = new Player
        {
            Authid = player.Authid,
            PlayerQuote = player.PlayerQuote,
            AvatarUrl = player.AvatarUrl,
            ApiKey = player.ApiKey,
            Isblocked = player.Isblocked
        };

        dbContext.Players.Add(playerObject);
        await dbContext.SaveChangesAsync();

        return new PlayerDto
        {
            Id = playerObject.Id,
            Authid = playerObject.Authid,
            PlayerQuote = playerObject.PlayerQuote,
            AvatarUrl = playerObject.AvatarUrl,
            ApiKey = playerObject.ApiKey,
            Isblocked = playerObject.Isblocked
        };
    }

    public Task<PlayerDto> GetPlayerAsync(Guid playerId)
    {
        throw new NotImplementedException();
    }

    public Task<PlayerDto> UpdatePlayerAsync(PlayerDto player)
    {
        throw new NotImplementedException();
    }
}

public interface IPlayerService
{
    Task<PlayerDto> GetPlayerAsync(Guid playerId);
    Task<PlayerDto> CreatePlayerAsync(PlayerDto player);
    Task<PlayerDto> UpdatePlayerAsync(PlayerDto player);
}