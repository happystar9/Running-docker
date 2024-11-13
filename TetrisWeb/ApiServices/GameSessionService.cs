using System.Data;
using TetrisWeb.Components.Models;
using TetrisWeb.DTOs;

namespace TetrisWeb.ApiServices;

public class GameSessionService
{
    private readonly GameSessionDto _gameSessionDto;

    private TetrominoGenerator generator = new TetrominoGenerator();
    public Tetromino? currentTetromino;

    public TetrominoStyle nextStyle;
    public TetrominoStyle secondNextStyle;
    public TetrominoStyle thirdNextStyle;

    private int standardDelay = 1000;
    private bool skipDelay = false;
    private int level = 1;
    private int score = 0;
    private int previousHighScore = 0;
    string previousScoreValue = "Nothing";

    public event Action? OnStateChange;

    public void NotifyStateChanged() => OnStateChange?.Invoke();


    public Grid GameStateGrid { get; private set; }

    public GameSessionService(GameSessionDto sessionDto)
    {
        _gameSessionDto = sessionDto;
        GameStateGrid = new Grid();
    }

    public void NewGameSession()
    {
        GameStateGrid = new Grid();
        generator = new TetrominoGenerator();
        level = 1;
        score = 0;
    }

    public void ResetGame()
    {
        GameStateGrid = new Grid();
    }

    public async Task RunGameSession()
    {
        nextStyle = generator.Next();
        secondNextStyle = generator.Next(nextStyle);
        thirdNextStyle = generator.Next(nextStyle, secondNextStyle);

        GameStateGrid.State = GameState.Playing;

        currentTetromino = generator.CreateFromStyle(nextStyle, GameStateGrid);


        while (!GameStateGrid.Cells.HasRow(21))
        {
            currentTetromino = generator.CreateFromStyle(nextStyle, GameStateGrid);
            nextStyle = secondNextStyle;
            secondNextStyle = thirdNextStyle;
            thirdNextStyle = generator.Next(currentTetromino.Style, nextStyle, secondNextStyle);

            NotifyStateChanged();

            await RunCurrentTetromino();
            await ClearCompleteRows();
            LevelChange();
        }

        GameStateGrid.State = GameState.GameOver;
    }

    public async Task RunCurrentTetromino()
    {
        while (currentTetromino.CanMoveDown())
        {
            await Delay(standardDelay);
            currentTetromino.MoveDown();

            NotifyStateChanged();

            if (!currentTetromino.CanMoveDown())
                await Delay(500);
        }
        UpdateBoard(currentTetromino);
    }

    public void LevelChange()
    {
        int counter = 1;
        int scoreCopy = score;
        while (scoreCopy > 4000)
        {
            counter++;
            scoreCopy -= 4000;
        }

        int newLevel = counter;
        if (newLevel != level)
        {
            standardDelay = 1000 - (newLevel - 1) * 100;

            level = newLevel;
        }
    }

    public async Task Delay(int millis)
    {
        int totalDelay = 0;
        while (totalDelay < millis && !skipDelay)
        {
            totalDelay += 50;
            await Task.Delay(50);
        }
        skipDelay = false;
    }

    public async Task ClearCompleteRows()
    {
        List<int> rowsComplete = new List<int>();
        for (int i = 1; i <= GameStateGrid.Height; i++)
        {
            if (GameStateGrid.Cells.GetAllInRow(i).Count == GameStateGrid.Width)
            {
                GameStateGrid.Cells.SetCssClass(i, "tetris-clear-row");

                rowsComplete.Add(i);
            }
        }

        if (rowsComplete.Any())
        {
            NotifyStateChanged();
            GameStateGrid.Cells.CollapseRows(rowsComplete);

            switch (rowsComplete.Count)
            {
                case 1:
                    score += 40 * level;
                    break;

                case 2:
                    score += 100 * level;
                    break;

                case 3:
                    score += 300 * level;
                    break;

                case 4:
                    score += 1200 * level;
                    break;
            }

            await Task.Delay(1000);
        }
        GameStateGrid.State = GameState.Playing;
    }

    public void UpdateBoard(Tetromino t)
    {
        GameStateGrid.Cells.AddMany(t.CoveredCells.GetAll(), t.CssClass);
    }

    public async Task MoveDown(int x)
    {
        for (int i = 0; i < x; i++)
        {
            currentTetromino.MoveDown();
        }
    }
    public async Task MoveLeft(int x)
    {
        for (int i = 0; i < x; i++)
        {
            currentTetromino.MoveLeft();
        }
    }
    public async Task MoveRight(int x)
    {
        for (int i = 0; i < x; i++)
        {
            currentTetromino.MoveRight();
        }
    }
    public async Task<int> Drop()
    {
        return currentTetromino.Drop();
    }
    public async Task Rotate()
    {
        currentTetromino.Rotate();
    }
}
