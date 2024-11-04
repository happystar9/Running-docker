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
