using FluentAssertions;
using TetrisWeb.ApiServices;
using TetrisWeb.Components.Models;
using TetrisWeb.DTOs;
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

    [Fact]
    public void Tetromino_Should_Not_Move_Right_When_At_Edge()
    {
        var grid = new Grid();
        CellList coveredCells = new();
        var tetromino = new TestTetromino(grid, coveredCells);
        for (int i = 0; i < 5; i++)
        {
            tetromino.MoveRight();
        }

        tetromino.CenterPieceCol.Should().Be(10);
    }

    [Fact]
    public async Task ClearCompleteRows_Should_Update_Score()
    {
        var service = new GameSessionService(new GameSessionDto());
        for(int i = 0; i <= 10; i++) { service.GameStateGrid.Cells.Add(0, i); }

        await service.ClearCompleteRows();

        service.Score.Should().Be(40);

    }
}

public class TestTetromino : Tetromino
{
    public TestTetromino(Grid grid, CellList coveredCells) : base(grid)
    {
        CoveredCells = coveredCells;
    }
}
