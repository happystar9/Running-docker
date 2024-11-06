namespace TetrisWeb.Components.Models
{
    public class Tetromino
    {

        public Tetromino(Grid grid)
        {
            Grid = grid;
            CenterPieceRow = grid.Height;
            CenterPieceCol = grid.Width / 2;
        }
        public Grid Grid { get; set; }
        public Orientation Orientation { get; set; } = Orientation.LeftRight;
        public int CenterPieceRow { get; set; }
        public int CenterPieceCol { get; set; }
        public virtual TetrominoStyle Style { get; }
        public virtual string CssClass { get; }
        public virtual CellList CoveredCells { get; set; }

        public bool CanMoveLeft()
        {
            foreach (var cell in CoveredCells.GetLeftmost())
            {
                if (Grid.Cells.Contains(cell.Row, cell.Column - 1))
                { return false; }
            }
            if (CoveredCells.HasColumn(1))
            { return false; }

            return true;
        }
        public bool CanMoveRight()
        {
            foreach (var cell in CoveredCells.GetRightmost())
            {
                if (Grid.Cells.Contains(cell.Row, cell.Column + 1))
                { return false; }
            }
            if (CoveredCells.HasColumn(Grid.Width))
            { return false; }

            return true;
        }
        public bool CanMoveDown()
        {
            foreach (var coord in CoveredCells.GetLowest())
            {
                if (Grid.Cells.Contains(coord.Row - 1, coord.Column))
                { return false; }
            }
            if (CoveredCells.HasRow(1))
            { return false; }

            return true;
        }

        public void MoveDown()
        {
            if (CanMoveDown())
                CenterPieceRow -= 1;
        }
        public void MoveLeft()
        {
            if (CanMoveLeft())
            { CenterPieceCol -= 1; }
        }

        public int Drop()
        {
            int i = 0;
            while (CanMoveDown())
            {
                CenterPieceRow -= 1;
                i++;
            }
            return 10 * i;
        }

        public void MoveRight()
        {
            if (CanMoveRight())
            { CenterPieceCol += 1; }
        }
        public void Rotate()
        {
            switch (Orientation)
            {
                case Orientation.UpDown:
                    Orientation = Orientation.RightLeft;
                    break;

                case Orientation.RightLeft:
                    Orientation = Orientation.DownUp;
                    break;

                case Orientation.DownUp:
                    Orientation = Orientation.LeftRight;
                    break;

                case Orientation.LeftRight:
                    Orientation = Orientation.UpDown;
                    break;
            }

            var coveredSpaces = CoveredCells;

            if (coveredSpaces.HasColumn(-1))
            {
                CenterPieceCol += 2;
            }
            else if (coveredSpaces.HasColumn(12))
            {
                CenterPieceCol -= 2;
            }
            else if (coveredSpaces.HasColumn(0))
            {
                CenterPieceCol += 1;
            }
            else if (coveredSpaces.HasColumn(11))
            {
                CenterPieceCol -= 1;
            }
        }
    }
}
