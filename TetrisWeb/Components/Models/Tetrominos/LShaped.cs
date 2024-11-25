//All basic tetrominos have 4 cells that are based off the starting cell
//which spawns centered at the top of the board
//They have 4 orientations which are different arangements of cells off the starting cell
//Not all tetrominos change based off orientation (the square stays a square)

namespace TetrisWeb.Components.Models.Tetrominos
{
    public class LShaped : Tetromino
    {
        public LShaped(Grid grid) : base(grid) { }

        public override TetrominoStyle Style => TetrominoStyle.LShaped;

        public override string CssClass => "tetris-orange-cell";

        public override CellList CoveredCells
        {
            get
            {
                CellList cells = new CellList();
                cells.Add(CenterPieceRow, CenterPieceCol);

                switch (Orientation)
                {
                    case Orientation.LeftRight:
                        cells.Add(CenterPieceRow, CenterPieceCol - 1);
                        cells.Add(CenterPieceRow, CenterPieceCol - 2);
                        cells.Add(CenterPieceRow + 1, CenterPieceCol);
                        break;

                    case Orientation.DownUp:
                        cells.Add(CenterPieceRow, CenterPieceCol + 1);
                        cells.Add(CenterPieceRow + 1, CenterPieceCol);
                        cells.Add(CenterPieceRow + 2, CenterPieceCol);
                        break;

                    case Orientation.RightLeft:
                        cells.Add(CenterPieceRow, CenterPieceCol + 1);
                        cells.Add(CenterPieceRow, CenterPieceCol + 2);
                        cells.Add(CenterPieceRow - 1, CenterPieceCol);
                        break;

                    case Orientation.UpDown:
                        cells.Add(CenterPieceRow, CenterPieceCol - 1);
                        cells.Add(CenterPieceRow - 1, CenterPieceCol);
                        cells.Add(CenterPieceRow - 2, CenterPieceCol);
                        break;
                }
                return cells;
            }
        }
    }
}
