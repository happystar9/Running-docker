using Microsoft.EntityFrameworkCore;
using TetrisWeb.GameData;
using TetrisWeb.DTOs;
using TetrisWeb.ApiServices.Interfaces;
using TetrisWeb.DTOs;

namespace TetrisWeb.ApiServices;
public class ScoreService(Dbf25TeamArzContext dbContext) : IScoreService
{
    //needs testing
    public int GetPlayerHighScore(int playerId)
    {
        return dbContext.GameSessions
            .Where(gs => gs.PlayerId == playerId)
            .Max(gs => (int?)gs.Score) ?? 0;
    }

    public bool IsHighScore(int playerId, int score)
    {
        return score > GetPlayerHighScore(playerId);
    }


    // needs testing
    public int UpdateHighScore(int playerId, int score)
    {
        Leaderboard entry = new Leaderboard
        {
            PlayerId = playerId,
            TotalScore = score
        };


        if (IsHighScore(playerId, score))
        {
            dbContext.Leaderboards.AddAsync(entry);
            dbContext.SaveChanges();
        }
        return GetPlayerHighScore(playerId);
    }

    public async Task<List<LeaderboardDto>> GetTopLeaderboardItemsAsync()
    {
        return dbContext.Leaderboards
            .OrderByDescending(l => l.TotalScore)
            .Select(l => new LeaderboardDto
            {
                PlayerId = l.PlayerId,
                TotalScore = l.TotalScore,
                GamesPlayed = l.GamesPlayed,
                WinCount = l.WinCount
            })
            .Take(15)
            .ToList();
    }

    public async Task<List<LeaderboardDto>> GetTopLeaderboardItemsWithUsernamesAsync()
    {
        return await dbContext.Leaderboards
            .OrderByDescending(l => l.TotalScore)
            .Take(15)
            .Join(dbContext.Players,
                leaderboard => leaderboard.PlayerId,
                player => player.Id,
                (leaderboard, player) => new LeaderboardDto
                {
                    PlayerId = leaderboard.PlayerId,
                    TotalScore = leaderboard.TotalScore,
                    GamesPlayed = leaderboard.GamesPlayed,
                    WinCount = leaderboard.WinCount,
                    Username = player.Username
                })
            .ToListAsync();
    }


    public async Task<List<LeaderboardDto>> GetCompleteLeaderboardAsync()
    {
        return await dbContext.Leaderboards
            .OrderByDescending(l => l.TotalScore)
            .Join(dbContext.Players,
                leaderboard => leaderboard.PlayerId,
                player => player.Id,
                (leaderboard, player) => new LeaderboardDto
                {
                    PlayerId = leaderboard.PlayerId,
                    TotalScore = leaderboard.TotalScore,
                    GamesPlayed = leaderboard.GamesPlayed,
                    WinCount = leaderboard.WinCount,
                    Username = player.Username
                })
            .ToListAsync();
    }

    //can't really test this until we have multiplayer working (and posting scores)
    public async Task<List<LeaderboardDto>> GetScoresForGameAsync(int gameId)
    {
        return await dbContext.GameSessions
            .Where(gs => gs.GameId == gameId)
            .OrderByDescending(gs => gs.Score)
            .Join(dbContext.Players,
                gameSession => gameSession.PlayerId,
                player => player.Id,
                (gameSession, player) => new LeaderboardDto
                {
                    PlayerId = (int)gameSession.PlayerId,
                    TotalScore = gameSession.Score,
                    Username = player.Username
                })
            .ToListAsync();
    }
}
