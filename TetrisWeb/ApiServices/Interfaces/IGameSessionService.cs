using TetrisWeb.Components.Models;

namespace TetrisWeb.ApiServices.Interfaces;

public interface IGameSessionService
{
    Grid GameStateGrid { get; } 
    //int previousHighScore { get; }
    int Score { get; }

    event Action? OnStateChange;

    //void AddGarbage();
    Task ClearCompleteRows();
    Task Delay(int millis);
    Task<int> Drop();
    Task AddGarbage(int lines);

    Task DropGarbageAny();
    void LevelChange();
    Task MoveDown(int x);
    Task MoveLeft(int x);
    Task MoveRight(int x);
    void NewGameSession();
    void NotifyStateChanged();
    void ResetGame();
    Task Rotate();
    Task RunCurrentTetromino();
    Task RunGameSession();
    void UpdateBoard(Tetromino t);
}