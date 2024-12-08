using FluentAssertions;
using TetrisWeb.ApiServices;
using TetrisWeb.Components.Models;
using TetrisWeb.DTOs;
namespace TetrisTest.GameLogicTests;

public class TetrominoTests
{
    [Fact]
    public void TetrominoShouldMoveRightWhenNotAtEdge()
    {
        var grid = new Grid();
        CellList coveredCells = new();
        var tetromino = new TestTetromino(grid, coveredCells);
        tetromino.CenterPieceCol = 2;

        tetromino.MoveRight();

        tetromino.CenterPieceCol.Should().Be(3);
    }

    [Fact]
    public void TetrominoShouldNotMoveRightWhenAtEdge()
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
    public async Task ClearCompleteRowsShouldUpdateScore()
    {
        var service = new GameSessionService();
        for (int i = 1; i <= 10; i++) { service.GameStateGrid.Cells.Add(1, i); }

        await service.ClearCompleteRows();

        service.Score.Should().Be(40);

    }

    [Fact]
    public void TetrominoShouldMovLeftWhenNotAtEdge()
    {
        var grid = new Grid();
        CellList coveredCells = new();
        var tetromino = new TestTetromino(grid, coveredCells);
        tetromino.CenterPieceCol = 2;

        tetromino.MoveLeft();

        tetromino.CenterPieceCol.Should().Be(1);
    }

    [Fact]
    public void TetrominoShouldNotMoveLeftWhenAtEdge()
    {
        var grid = new Grid();
        CellList coveredCells = new();
        var tetromino = new TestTetromino(grid, coveredCells);
        for (int i = 0; i < 5; i++)
        {
            tetromino.MoveLeft();
        }

        tetromino.CenterPieceCol.Should().Be(0);
    }

    [Fact]
    public void TetrominoShouldRotateOnKeyUp()
    {
        var grid = new Grid();
        CellList coveredCells = new();

        var tetromino = new TestTetromino(grid, coveredCells);

        var startingOrientation = tetromino.Orientation;

        for (int i = 0; i < 5; i++)
        {
            tetromino.Rotate();
        }

        var endingOrientation = tetromino.Orientation;

        startingOrientation.Should().Be(Orientation.LeftRight);
        endingOrientation.Should().Be(Orientation.UpDown);
    }

    [Fact]
    public void TetrominoShouldDropOnSpacebar()
    {
        var grid = new Grid();
        CellList coveredCells = new();
        var tetromino = new TestTetromino(grid, coveredCells);
        tetromino.Drop();
        Te
    }
}

public class TestTetromino : Tetromino
{
    public TestTetromino(Grid grid, CellList coveredCells) : base(grid)
    {
        CoveredCells = coveredCells;
    }
}
