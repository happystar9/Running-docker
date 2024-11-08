using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TetrisWeb.Components.Models
{
    public class Grid
    {
        public int Width { get; } = 10;
        public int Height { get; } = 20;
        public CellList Cells { get; set; } = new CellList();
        public GameState State { get; set; } = GameState.NotStarted;

        public bool IsStarted
        {
            get
            {
                return State == GameState.Playing
                       || State == GameState.GameOver;
            }
        }
    }
}
