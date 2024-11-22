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



public interface IScoreService{
    public int GetPlayerHighScore(int playerId);

    //takes in a new score and checks to see if it is higher than the player's current high score
    //will call GetPlayerHighScore
    public bool IsHighScore(int playerId, int score);

    //calls IsHighScore and if true, will update the player's high score
    public int UpdateHighScore(int playerId, int score);

}
