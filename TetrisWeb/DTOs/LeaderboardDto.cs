namespace TetrisWeb.DTOs;

public class LeaderboardDto
{
    public int? Id { get; set; }

    public int PlayerId { get; set; }
    public string? Username { get; set; }

    public long? TotalScore { get; set; }

    public int? GamesPlayed { get; set; }

    public int? WinCount { get; set; }
}
