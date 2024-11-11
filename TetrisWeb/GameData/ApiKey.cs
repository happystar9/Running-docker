using System;
using System.Collections.Generic;

namespace TetrisWeb.GameData;

public partial class ApiKey
{
    public int Id { get; set; }

    public int? PlayerId { get; set; }

    public string Key { get; set; } = null!;

    public DateTime CreatedOn { get; set; }

    public DateTime? ExpiredOn { get; set; }

    public virtual Player? Player { get; set; }
}
