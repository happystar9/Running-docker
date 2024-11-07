using System;
using System.Collections.Generic;

namespace TetrisWeb.AuthData;

public partial class GameSession
{
    public int Id { get; set; }

    public int? GameId { get; set; }

    public int? PlayerId { get; set; }

    public int Score { get; set; }

    public virtual Game? Game { get; set; }

    public virtual Player? Player { get; set; }
}
