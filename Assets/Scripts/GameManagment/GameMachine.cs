using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    OFF = 0,
    PLAY = 1,
    PAUSE = 2,
    FINISH = 3,
}

public sealed class GameMachine
{
    public GameState GameState
    {
        get { return this.gameState; }
    }

    private readonly List<IGameListener> listeners = new();

    private GameState gameState = GameState.OFF;

    public void StartGame()
    {
        if (this.gameState != GameState.OFF && this.gameState != GameState.FINISH)
        {
            Debug.LogWarning($"You can start game only from {GameState.OFF} state! Current state {gameState}");
            return;
        }

        this.gameState = GameState.PLAY;

        foreach (var listener in this.listeners)
        {
            if (listener is IStartGameListener startListener)
            {
                startListener.OnStartGame();
            }
        }
    }

    public void PauseGame()
    {
        if (this.gameState != GameState.PLAY)
        {
            Debug.LogWarning($"You can pause game only from {GameState.PLAY} state! Current state {gameState}");
            return;
        }

        this.gameState = GameState.PAUSE;

        foreach (var listener in this.listeners)
        {
            if (listener is IPauseGameListener pauseListener)
            {
                pauseListener.OnPauseGame();
            }
        }
    }

    public void ResumeGame()
    {
        if (this.gameState != GameState.PAUSE)
        {
            Debug.LogWarning($"You can resume game only from {GameState.PAUSE} state! Current state {gameState}");
            return;
        }

        this.gameState = GameState.PLAY;

        foreach (var listener in this.listeners)
        {
            if (listener is IResumeGameListener resumeListener)
            {
                resumeListener.OnResumeGame();
            }
        }
    }

    public void FinishGame()
    {
        if (this.gameState != GameState.PLAY)
        {
            Debug.LogWarning($"You can finish game only from {GameState.PLAY} state! Current state {gameState}");
            return;
        }

        this.gameState = GameState.FINISH;

        foreach (var listener in this.listeners)
        {
            if (listener is IFinishGameListener finishListener)
            {
                finishListener.OnFinishGame();
            }
        }
    }

    public void AddListener(IGameListener listener)
    {
        this.listeners.Add(listener);
    }

    public void RemoveListener(IGameListener listener)
    {
        this.listeners.Remove(listener);
    }
}