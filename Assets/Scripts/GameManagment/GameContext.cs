using UnityEngine;

public sealed class GameContext : MonoBehaviour
{
    public GameState GameState
    {
        get { return this.gameMachine.GameState; }
    }

    private readonly GameMachine gameMachine = new();


    [ContextMenu("Start Game")]
    public void StartGame()
    {
        this.gameMachine.StartGame();
    }

    [ContextMenu("Pause Game")]
    public void PauseGame()
    {
        this.gameMachine.PauseGame();
    }

    [ContextMenu("Resume Game")]
    public void ResumeGame()
    {
        this.gameMachine.ResumeGame();
    }

    [ContextMenu("Finish Game")]
    public void FinishGame()
    {
        this.gameMachine.FinishGame();
    }

    public void AddListener(IGameListener listener)
    {
        this.gameMachine.AddListener(listener);
    }

    public void RemoveListener(IGameListener listener)
    {
        this.gameMachine.RemoveListener(listener);
    }
}