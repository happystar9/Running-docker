namespace TetrisWeb.Components.Models.Tetrominos
{
    public class Garbage : Tetromino
    {
        Random random = new Random();
        public Garbage(Grid grid) : base(grid) { }

        public override string CssClass => "tetris-garbage-cell";

        public override CellList CoveredCells
        {
            get
            {
                CellList cells = new CellList();
                for (int i = 1; i < LastCol + 1; i++) {
                    cells.Add(CenterPieceRow, i);
                }
                cells.Remove(CenterPieceRow, random.Next(1, LastCol + 1));
                return cells;
            }
        }
    }
}
