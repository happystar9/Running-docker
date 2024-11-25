//All basic tetrominos have 4 cells that are based off the starting cell
//which spawns centered at the top of the board
//They have 4 orientations which are different arangements of cells off the starting cell
//Not all tetrominos change based off orientation (the square stays a square)

namespace TetrisWeb.Components.Models.Tetrominos;
public class Block : Tetromino
{
    public Block(Grid grid) : base(grid) { }

    public override TetrominoStyle Style => TetrominoStyle.Block;

    public override string CssClass => "tetris-yellow-cell";

    public override CellList CoveredCells
    {
        get
        {
            CellList cells = new CellList();
            cells.Add(CenterPieceRow, CenterPieceCol);
            cells.Add(CenterPieceRow - 1, CenterPieceCol);
            cells.Add(CenterPieceRow, CenterPieceCol + 1);
            cells.Add(CenterPieceRow - 1, CenterPieceCol + 1);
            return cells;
        }
    }
}