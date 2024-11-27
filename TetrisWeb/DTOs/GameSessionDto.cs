using TetrisWeb.Components.Models;
using TetrisWeb.GameData;

namespace TetrisWeb.DTOs;

public class GameSessionDto
{
    public int Id { get; set; }

    public int? GameId { get; set; }

    public int PlayerId { get; set; }

    public int Score { get; set; }
    public GameState? GameState { get; set; }

}

