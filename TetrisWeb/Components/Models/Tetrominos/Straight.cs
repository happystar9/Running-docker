//All basic tetrominos have 4 cells that are based off the starting cell
//which spawns centered at the top of the board
//They have 4 orientations which are different arangements of cells off the starting cell
//Not all tetrominos change based off orientation (the square stays a square)

namespace TetrisWeb.Components.Models.Tetrominos
{
    public class Straight : Tetromino
    {
        public Straight(Grid grid) : base(grid) { }

        public override TetrominoStyle Style => TetrominoStyle.Straight;

        public override string CssClass => "tetris-lightblue-cell";

        public override CellList CoveredCells
        {
            get
            {
                CellList cells = new CellList();
                cells.Add(CenterPieceRow, CenterPieceCol);

                if (Orientation == Orientation.LeftRight)
                {
                    cells.Add(CenterPieceRow, CenterPieceCol - 1);
                    cells.Add(CenterPieceRow, CenterPieceCol - 2);
                    cells.Add(CenterPieceRow, CenterPieceCol + 1);
                }
                else if (Orientation == Orientation.DownUp)
                {
                    cells.Add(CenterPieceRow - 1, CenterPieceCol);
                    cells.Add(CenterPieceRow + 1, CenterPieceCol);
                    cells.Add(CenterPieceRow + 2, CenterPieceCol);
                }
                else if (Orientation == Orientation.RightLeft)
                {
                    cells.Add(CenterPieceRow, CenterPieceCol - 1);
                    cells.Add(CenterPieceRow, CenterPieceCol + 1);
                    cells.Add(CenterPieceRow, CenterPieceCol + 2);
                }
                else //UpDown
                {
                    cells.Add(CenterPieceRow - 1, CenterPieceCol);
                    cells.Add(CenterPieceRow - 2, CenterPieceCol);
                    cells.Add(CenterPieceRow + 1, CenterPieceCol);
                }

                return cells;
            }
        }
    }
}
