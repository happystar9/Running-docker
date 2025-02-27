﻿namespace TetrisWeb.Components.Models.Tetrominos
{
    public class Garbage : Tetromino
    {
        Random random = new Random();
        public Garbage(Grid grid) : base(grid) { }

        //public override TetrominoStyle Style => TetrominoStyle.Garbage;

        public override string CssClass => "tetris-orange-cell";

        public override CellList CoveredCells
        {
            get
            {
                CellList cells = new CellList();
                var skip = random.Next(0, LastCol + 1);
                for (int i = 1; i < LastCol + 1; i++) { // use coveredCells.getallinrow instead
                    if (i == skip)
                    {
                        i++;
                    }
                    cells.Add(CenterPieceRow, i);
                }
                return cells;
            }
        }
    }
}
