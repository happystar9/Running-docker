using TetrisWeb.GameData;
using TetrisWeb.DTOs;
using TetrisWeb.ApiServices.Interfaces;
using Microsoft.EntityFrameworkCore;
using TetrisWeb.Components.Pages;
using System.Collections.Concurrent;

namespace TetrisWeb.ApiServices;

public class PlayerService(Dbf25TeamArzContext dbContext) : IPlayerService
{
    private readonly ConcurrentDictionary<string, int> currentGameMap = new();

    public void SetCurrentGameId(string authId, int gameId)
    {
        currentGameMap[authId] = gameId;
    }

    public int GetCurrentGameId(string authId)
    {
        return currentGameMap.TryGetValue(authId, out var gameId) ? gameId : 0;
    }
    public async Task<PlayerDto> CreatePlayerAsync(PlayerDto player)
    {
        var playerObject = new Player
        {
            Username = player.Username,
            Authid = player.Authid,
            PlayerQuote = player.PlayerQuote,
            AvatarUrl = player.AvatarUrl,
            Isblocked = player.Isblocked
        };

        dbContext.Players.Add(playerObject);
        await dbContext.SaveChangesAsync();

        return new PlayerDto
        {
            Username = playerObject.Username,
            Id = playerObject.Id,
            Authid = playerObject.Authid,
            PlayerQuote = playerObject.PlayerQuote,
            AvatarUrl = playerObject.AvatarUrl,
            Isblocked = playerObject.Isblocked
        };
    }

    public async Task<PlayerDto> GetPlayerByUsernameAsync(string username)
    {
        var player = await dbContext.Players
                .SingleOrDefaultAsync(p => p.Username == username);

        if (player == null)
        {
            throw new KeyNotFoundException("Player details not found.");
        }

        var result = new PlayerDto
        {
            Id = player.Id,
            Username = player.Username,
            Authid = player.Authid,
            PlayerQuote = player.PlayerQuote,
            AvatarUrl = player.AvatarUrl,
            Isblocked = player.Isblocked
        };

        return result;
    }
    public async Task<PlayerDto> GetPlayerByIdAsync(int id)
    {
        var player = await dbContext.Players
                .SingleOrDefaultAsync(p => p.Id == id);

        if (player == null)
        {
            throw new KeyNotFoundException("Player details not found.");
        }

        return new PlayerDto
        {
            Username = player.Username,
            Id = player.Id,
            Authid = player.Authid,
            PlayerQuote = player.PlayerQuote,
            AvatarUrl = player.AvatarUrl,
            Isblocked = player.Isblocked
        };
    }

    public async Task<PlayerDto> GetPlayerByAuthIdAsync(string authId)
    {
        var player = await dbContext.Players
                .SingleOrDefaultAsync(p => p.Authid == authId);


        if (player == null)
        {
            throw new KeyNotFoundException("Player details not found.");
        }

        int currentGameId = currentGameMap.TryGetValue(authId, out var gameId) ? gameId : 0;

        return new PlayerDto
        {
            Username = player.Username,
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

        //player.Authid = playerDto.Authid;
        player.PlayerQuote = playerDto.PlayerQuote;
        player.AvatarUrl = playerDto.AvatarUrl;
        player.Isblocked = playerDto.Isblocked;
        player.Username = playerDto.Username;

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

    public async Task<int> GetPlayerTotalScore(string authId)
    {
        //from the database, get the id from the player table that has 
        var player = await dbContext.Players
                .SingleOrDefaultAsync(p => p.Authid == authId);

        if(player == null)
        {
            throw new KeyNotFoundException("Can't get a score for a player that does not exist.");
        }

        int totalScore = await dbContext.GameSessions
            .Where(gs => gs.PlayerId == player.Id)
            .SumAsync(gs => gs.Score);

        return totalScore;
    }

    public async Task<List<PlayerDto>> GetAllPlayersAsync()
    {
        return await dbContext.Players
            .Select(p => new PlayerDto
            {
                Id = p.Id,
                Username = p.Username,
                PlayerQuote = p.PlayerQuote,
                AvatarUrl = p.AvatarUrl
            })
            .ToListAsync();
    }
}
