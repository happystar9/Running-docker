using Org.BouncyCastle.Asn1.Mozilla;
using TetrisWeb.DTOs;

namespace TetrisWeb.ApiServices.Interfaces;

public interface IScoreService
{
    public int GetPlayerHighScore(int playerId);

    //takes in a new score and checks to see if it is higher than the player's current high score
    //will call GetPlayerHighScore
    public bool IsHighScore(int playerId, int score);

    //calls IsHighScore and if true, will update the player's high score
    public int UpdateHighScore(int playerId, int score);

    public Task<List<LeaderboardDto>> GetTopLeaderboardItemsAsync();

    public Task<List<LeaderboardDto>> GetTopLeaderboardItemsWithUsernamesAsync();
    public Task<List<LeaderboardDto>> GetCompleteLeaderboardAsync();
    public Task<List<LeaderboardDto>> GetScoresForGameAsync(int gameId);


}
