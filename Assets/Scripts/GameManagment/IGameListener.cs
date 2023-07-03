public interface IGameListener
{
}

public interface IStartGameListener : IGameListener
{
    void OnStartGame();
}

public interface IPauseGameListener : IGameListener
{
    void OnPauseGame();
}

public interface IResumeGameListener : IGameListener
{
    void OnResumeGame();
}

public interface IFinishGameListener : IGameListener
{
    void OnFinishGame();
}