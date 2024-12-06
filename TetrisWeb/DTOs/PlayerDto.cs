using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TetrisWeb.DTOs;

public class PlayerDto
{
    public string Username { get; set; }
    public int Id { get; set; }

    public string Authid { get; set; } = null!;

    public string? PlayerQuote { get; set; }

    public string? AvatarUrl { get; set; }

    public bool Isblocked { get; set; }

    public int CurrentGameId { get; set; }
}
