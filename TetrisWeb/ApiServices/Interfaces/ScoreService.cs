using Microsoft.EntityFrameworkCore;
using TetrisWeb.GameData;
using TetrisShared.DTOs;

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

    //needs testing
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
            dbContext.AddAsync(entry);
            dbContext.SaveChanges();
        }
        return GetPlayerHighScore(playerId);
    }
}
