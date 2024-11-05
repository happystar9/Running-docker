using System;
using System.Collections.Generic;

namespace TetrisApi.Data;

public partial class Leaderboard
{
    public int Id { get; set; }

    public int PlayerId { get; set; }

    public long? TotalScore { get; set; }

    public int? GamesPlayed { get; set; }

    public int? WinCount { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Player Player { get; set; } = null!;
}
