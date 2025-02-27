﻿using System;
using System.Collections.Generic;

namespace TetrisWeb.AuthData;

public partial class Player
{
    public int Id { get; set; }

    public string Authid { get; set; } = null!;

    public string? PlayerQuote { get; set; }

    public string? AvatarUrl { get; set; }

    public string? ApiKey { get; set; }

    public bool? Isblocked { get; set; }

    public virtual ICollection<Chat> Chats { get; set; } = new List<Chat>();

    public virtual ICollection<GameSession> GameSessions { get; set; } = new List<GameSession>();

    public virtual ICollection<Game> Games { get; set; } = new List<Game>();

    public virtual ICollection<Leaderboard> Leaderboards { get; set; } = new List<Leaderboard>();
}
