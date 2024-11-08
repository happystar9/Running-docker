using System;
using TetrisWeb.Components.Models;

namespace TetrisWeb.Services;

public class GameStateService
{
    public Grid Grid { get; private set; }

    public GameStateService()
    {
        Grid = new Grid();
    }

    public void ResetGame()
    {
        Grid = new Grid();
    }
    public void UpdateBoard(Tetromino t)
    {
        Grid.Cells.AddMany(t.CoveredCells.GetAll(),t.CssClass);
    }
}


