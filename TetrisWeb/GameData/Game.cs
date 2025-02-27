﻿using System;
using System.Collections.Generic;

namespace TetrisWeb.GameData;

public partial class Game
{
    public int Id { get; set; }

    public string CreatedByAuthId { get; set; } = null!;

    public DateTime StartTime { get; set; }

    public DateTime? StopTime { get; set; }

    public int? PlayerCount { get; set; }

    public virtual Player CreatedByAuth { get; set; } = null!;

    public virtual ICollection<GameSession> GameSessions { get; set; } = new List<GameSession>();
}
