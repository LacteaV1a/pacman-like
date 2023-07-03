using UnityEngine;
public abstract class GameSystem : MonoBehaviour,  IStartGameListener, IFinishGameListener
{
    public abstract void OnFinishGame();

    public abstract void OnStartGame();
}
