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
