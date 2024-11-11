using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components;
using System;
using TetrisWeb.Components.Models;
using System.Net.NetworkInformation;
using TetrisWeb.Components.Pages.Partials;
using TetrisWeb.Services;

namespace TetrisWeb.Services;

public class GameStateService
{
    TetrominoGenerator generator = new TetrominoGenerator();

    Tetromino? currentTetromino;

    TetrominoStyle nextStyle;
    TetrominoStyle secondNextStyle;
    TetrominoStyle thirdNextStyle;

    int standardDelay = 1000;

    bool skipDelay = false;

    int level = 1;
    int score = 0;
    int previousHighScore = 0;
    string previousScoreValue = "Nothing";

    public Grid gameStateGrid { get; private set; }

    public GameStateService()
    {
        gameStateGrid = new Grid();
    }

    public void ResetGame()
    {
        gameStateGrid = new Grid();
    }
    public void UpdateBoard(Tetromino t)
    {
        gameStateGrid.Cells.AddMany(t.CoveredCells.GetAll(), t.CssClass);
    }

    public void NewGame()
    {
        ResetGame();
        generator = new TetrominoGenerator();
        currentTetromino = null;
        level = 1;
        if (score > previousHighScore)
        {
            previousHighScore = score;
        }
        score = 0;
    }

    public async Task RunGame()
    {
        nextStyle = generator.Next();
        secondNextStyle = generator.Next(nextStyle);
        thirdNextStyle = generator.Next(nextStyle, secondNextStyle);

        gameStateGrid.State = GameState.Playing;

        while (!gameStateGrid.Cells.HasRow(21))
        {
            currentTetromino = generator.CreateFromStyle(nextStyle, gameStateGrid);
            nextStyle = secondNextStyle;
            secondNextStyle = thirdNextStyle;
            thirdNextStyle = generator.Next(currentTetromino.Style, nextStyle, secondNextStyle);

            await RunCurrentTetromino();

            await ClearCompleteRows();

            LevelChange();
        }

        gameStateGrid.State = GameState.GameOver;
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

    public async Task RunCurrentTetromino()
    {
        while (currentTetromino.CanMoveDown())
        {
            await Delay(standardDelay);

            currentTetromino.MoveDown();

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
            standardDelay = 1000 - ((newLevel - 1) * 100);

            level = newLevel;
        }
    }

    public async Task ClearCompleteRows()
    {
        List<int> rowsComplete = new List<int>();
        for (int i = 1; i <= gameStateGrid.Height; i++)
        {
            if (gameStateGrid.Cells.GetAllInRow(i).Count == gameStateGrid.Width)
            {
                gameStateGrid.Cells.SetCssClass(i, "tetris-clear-row");

                rowsComplete.Add(i);
            }
        }

        if (rowsComplete.Any())
        {

            gameStateGrid.Cells.CollapseRows(rowsComplete);

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
        gameStateGrid.State = GameState.Playing;
    }

    public async Task MoveDown(int x)
    {
        for (int i = 1; i < x; i++)
        {
            currentTetromino.MoveDown();
        }
    }
    public async Task MoveLeft(int x)
    {
        for (int i = 1; i < x; i++)
        {
            currentTetromino.MoveLeft();
        }
    }
    public async Task MoveRight(int x)
    {
        for (int i = 1; i < x; i++)
        {
            currentTetromino.MoveRight();
        }
    }
    public async Task Drop()
    {
        currentTetromino.Drop();
    }
    public async Task Rotate()
    {
        currentTetromino.Rotate();
    }
}


