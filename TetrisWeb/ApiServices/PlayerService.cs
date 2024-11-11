using TetrisWeb.GameData;
using TetrisShared;
using TetrisShared.DTOs;
using TetrisWeb.DTOs;
using TetrisWeb.ApiServices.Interfaces;
using Microsoft.EntityFrameworkCore;

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
            Isblocked = playerObject.Isblocked
        };
    }

    public async Task<PlayerDto> GetPlayerAsync(string authId)
    {
        var player = await dbContext.Players
                .SingleOrDefaultAsync(p => p.Authid == authId);


        if (player == null)
        {
            throw new KeyNotFoundException("Player details not found.");
        }

        return new PlayerDto
        {
            Id = player.Id,
            Authid = player.Authid,
            PlayerQuote = player.PlayerQuote,
            AvatarUrl = player.AvatarUrl,
            Isblocked = player.Isblocked
        };
    }

    public async Task<PlayerDto> UpdatePlayerAsync(PlayerDto playerDto)
    {
        var player = await dbContext.Players.FindAsync(playerDto.Id);

        if (player == null)
        {
            throw new KeyNotFoundException("Player details not found.");
        }

        player.Authid = playerDto.Authid;
        player.PlayerQuote = playerDto.PlayerQuote;
        player.AvatarUrl = playerDto.AvatarUrl;
        player.Isblocked = playerDto.Isblocked;

        await dbContext.SaveChangesAsync();

        return new PlayerDto
        {
            Id = player.Id,
            Authid = player.Authid,
            PlayerQuote = player.PlayerQuote,
            AvatarUrl = player.AvatarUrl,
            Isblocked = player.Isblocked
        };

    }
}
