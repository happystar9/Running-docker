namespace TetrisWeb.ApiServices;

public interface IScoreService{
    public int GetPlayerHighScore(int playerId);

    //takes in a new score and checks to see if it is higher than the player's current high score
    //will call GetPlayerHighScore
    public bool IsHighScore(int playerId, int score);

    //calls IsHighScore and if true, will update the player's high score
    public int UpdateHighScore(int playerId, int score);

}
