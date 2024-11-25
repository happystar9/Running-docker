//All basic tetrominos have 4 cells that are based off the starting cell
//which spawns centered at the top of the board
//They have 4 orientations which are different arangements of cells off the starting cell
//Not all tetrominos change based off orientation (the square stays a square)

namespace TetrisWeb.Components.Models.Tetrominos
{
    public class TShaped : Tetromino
    {
        public TShaped(Grid grid) : base(grid) { }

        public override TetrominoStyle Style => TetrominoStyle.TShaped;

        public override string CssClass => "tetris-purple-cell";

        public override CellList CoveredCells
        {
            get
            {
                CellList cells = new CellList();
                cells.Add(CenterPieceRow, CenterPieceCol);

                switch (Orientation)
                {
                    case Orientation.LeftRight:
                        cells.Add(CenterPieceRow - 1, CenterPieceCol);
                        cells.Add(CenterPieceRow, CenterPieceCol + 1);
                        cells.Add(CenterPieceRow + 1, CenterPieceCol);
                        break;
                    case Orientation.DownUp:
                        cells.Add(CenterPieceRow - 1, CenterPieceCol);
                        cells.Add(CenterPieceRow, CenterPieceCol - 1);
                        cells.Add(CenterPieceRow, CenterPieceCol + 1);
                        break;

                    case Orientation.RightLeft:
                        cells.Add(CenterPieceRow - 1, CenterPieceCol);
                        cells.Add(CenterPieceRow, CenterPieceCol - 1);
                        cells.Add(CenterPieceRow + 1, CenterPieceCol);
                        break;

                    case Orientation.UpDown:
                        cells.Add(CenterPieceRow + 1, CenterPieceCol);
                        cells.Add(CenterPieceRow, CenterPieceCol - 1);
                        cells.Add(CenterPieceRow, CenterPieceCol + 1);
                        break;
                }
                return cells;
            }
        }
    }
}
