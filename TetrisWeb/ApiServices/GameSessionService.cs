using System.Data;
using TetrisWeb.ApiServices.Interfaces;
using TetrisWeb.AuthData;
using TetrisWeb.Components.Models;
using TetrisWeb.DTOs;
using TetrisWeb.GameData;

namespace TetrisWeb.ApiServices;

public class GameSessionService : IGameSessionService
{
    private TetrominoGenerator generator = new TetrominoGenerator();
    public Tetromino? currentTetromino;

    public TetrominoStyle nextStyle;
    public TetrominoStyle secondNextStyle;
    public TetrominoStyle thirdNextStyle;
    public int HighScore { get; set; } = 0;

    public int garbageLines = 0;

    private int standardDelay = 1000;
    private bool skipDelay = false;
    private int level = 1;

    private int _score = 0;
    public int Score
    {
        get => _score;
        private set
        {
            _score = value;
            NotifyStateChanged();
        }
    }


    public event Action? OnStateChange;

    public void NotifyStateChanged() => OnStateChange?.Invoke();

    public event Action<int>? SendGarbage;
    public void NotifySendGarbage(int lines) => SendGarbage?.Invoke(lines);

    public Grid GameStateGrid { get; private set; }

    public GameSessionService()
    {
        GameStateGrid = new Grid();
    }

    public void NewGameSession()
    {
        GameStateGrid = new Grid();
        generator = new TetrominoGenerator();
        level = 1;
        if (Score > HighScore)
        {
            HighScore = Score;
        }
        Score = 0;
    }

    public void ResetGame()
    {
        GameStateGrid = new Grid();
        generator = new TetrominoGenerator();
        level = 1;
        Score = 0;
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

            if (garbageLines > 0)
            {
                await DropGarbageAny();
            }

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

    public async Task AddGarbage(int lines)
    {
        garbageLines += lines;
    }

    public async Task DropGarbageAny()
    {
        while (garbageLines > 0)
        {
            currentTetromino = generator.MakeGarbage(GameStateGrid);
            currentTetromino.Drop();
            garbageLines--;
        }
    }

    public void LevelChange()
    {
        int counter = 1;
        int scoreCopy = Score;
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
                    Score += 40 * level;
                    break;

                case 2:
                    Score += 100 * level;
                    NotifySendGarbage(1);
                    break;

                case 3:
                    Score += 300 * level;
                    NotifySendGarbage(1);
                    break;

                case 4:
                    Score += 1200 * level;
                    NotifySendGarbage(1);
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
        return Score += currentTetromino.Drop();
    }

    public async Task Rotate()
    {
        currentTetromino.Rotate();
    }
}
