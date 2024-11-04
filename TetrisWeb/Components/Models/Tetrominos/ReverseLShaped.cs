namespace TetrisWeb.Components.Models.Tetrominos
{
    public class ReverseLShaped : Tetromino
    {
        public ReverseLShaped(Grid grid) : base(grid) { }

        public override TetrominoStyle Style => TetrominoStyle.ReverseLShaped;

        public override string CssClass => "tetris-darkblue-cell";

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
                        cells.Add(CenterPieceRow - 1, CenterPieceCol);
                        break;

                    case Orientation.DownUp:
                        cells.Add(CenterPieceRow, CenterPieceCol - 1);
                        cells.Add(CenterPieceRow + 1, CenterPieceCol);
                        cells.Add(CenterPieceRow + 2, CenterPieceCol);
                        break;

                    case Orientation.RightLeft:
                        cells.Add(CenterPieceRow, CenterPieceCol + 1);
                        cells.Add(CenterPieceRow, CenterPieceCol + 2);
                        cells.Add(CenterPieceRow + 1, CenterPieceCol);
                        break;

                    case Orientation.UpDown:
                        cells.Add(CenterPieceRow, CenterPieceCol + 1);
                        cells.Add(CenterPieceRow - 1, CenterPieceCol);
                        cells.Add(CenterPieceRow - 2, CenterPieceCol);
                        break;
                }
                return cells;
            }
        }
    }
}
