using System;
using System.Collections.Generic;

namespace TetrisApi.Data;

public partial class Chat
{
    public int Id { get; set; }

    public int PlayerId { get; set; }

    public string? Message { get; set; }

    public DateTime? TimeSent { get; set; }

    public virtual Player Player { get; set; } = null!;
}
