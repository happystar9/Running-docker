namespace TetrisWeb.Components.Models.Tetrominos
{
    public class LeftZigZag : Tetromino
    {
        public LeftZigZag(Grid grid) : base(grid) { }

        public override TetrominoStyle Style => TetrominoStyle.Block;

        public override string CssClass => "tetris-red-cell";

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
                        cells.Add(CenterPieceRow + 1, CenterPieceCol + 1);
                        break;
                    case Orientation.DownUp:
                        cells.Add(CenterPieceRow - 1, CenterPieceCol);
                        cells.Add(CenterPieceRow, CenterPieceCol - 1);
                        cells.Add(CenterPieceRow - 1, CenterPieceCol + 1);
                        break;

                    case Orientation.RightLeft:
                        cells.Add(CenterPieceRow - 1, CenterPieceCol);
                        cells.Add(CenterPieceRow, CenterPieceCol + 1);
                        cells.Add(CenterPieceRow + 1, CenterPieceCol + 1);
                        break;

                    case Orientation.UpDown:
                        cells.Add(CenterPieceRow - 1, CenterPieceCol);
                        cells.Add(CenterPieceRow, CenterPieceCol - 1);
                        cells.Add(CenterPieceRow - 1, CenterPieceCol + 1);
                        break;
                }
            return cells;
            }
        }
    }
}
