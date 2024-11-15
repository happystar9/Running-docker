using FluentAssertions;
using TetrisWeb.Components.Models;
namespace TetrisTest;

public class TetrominoTests
{
	[Fact]
	public void Tetromino_Should_Move_Right_When_Not_At_Edge()
	{
		var grid = new Grid();
		CellList coveredCells = new();
		var tetromino = new TestTetromino(grid, coveredCells);
		tetromino.CenterPieceCol = 2;

		tetromino.MoveRight();

		tetromino.CenterPieceCol.Should().Be(3);
    }
}

public class TestTetromino : Tetromino
{
    public TestTetromino(Grid grid, CellList coveredCells) : base(grid)
    {
        CoveredCells = coveredCells;
    }
}
