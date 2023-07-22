using Leopotam.EcsLite;
using System;
using System.Collections;
using UnityEngine;

public sealed class EcsStartup : MonoBehaviour, IStartGameListener, IFinishGameListener
{
    EcsWorld _world;
    IEcsSystems _systems;
    [SerializeField] private MazeConfig _mazeConfig;
    [SerializeField] private MazeViewConfig _mazeViewConfig;
    [SerializeField] private PlayerConfig _playerConfig;
    [SerializeField] private PoolConfig _peaPoolConfig;
    [SerializeField] private PoolConfig _enemyConfig;
    [SerializeField] private PeaConfig _peaConfig;

    [SerializeField] private GameContext _gameContext;

    private bool _start;

    [ContextMenu("Initialize")]
    private void InitializeEcs()
    {
        _world = new EcsWorld();
        _systems = new EcsSystems(_world);
        _systems
            .Add(new PlayerInputSystem())
            .Add(new MazeInitializeSystem(_mazeConfig))
            .Add(new MazeVisualizeSystem(_mazeViewConfig))
            .Add(new PlayerInitializeSystem(_playerConfig))
            .Add(new PeasInitializeSystem(_peaPoolConfig))
            .Add(new EnemyInitializeSystem(_enemyConfig))
            .Add(new PeaCollectSystem(_peaConfig))
            .Add(new MazePlaceholderSystem())
            .Add(new FollowTargetInMazeSystem())
            .Add(new MovementInMazeSystem())
            .Add(new GrabItemSystem())
            .Add(new MazeClearSystem())
            .Add(new PlayerDamageSystem())
            .Add(new TimerSystem())
            .Add(new DestroySystem())
            .Add(new EndGameSystem(_gameContext))


#if UNITY_EDITOR
            .Add(new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem())
#endif
                .Init();
    }

    private void Update() {
        if(_start)
            _systems?.Run();
    }

    [ContextMenu("Destroy")]
    private void DestroyEcs()
    {
        if (_systems != null)
        {
            _systems.Destroy();
            _systems = null;
        }

        if (_world != null)
        {
            _world.Destroy();
            _world = null;
        }
    }

    private void OnDestroy() {
        DestroyEcs();
    }

    public void OnStartGame()
    {
        InitializeEcs();
        _start = true;
    }

    public void OnFinishGame()
    {
        _start = false;
        StartCoroutine(ActionNextFrameRoutine(DestroyEcs));
        //DestroyEcs();
    }
    private IEnumerator ActionNextFrameRoutine(Action action)
    {
        yield return new WaitForEndOfFrame();
        action?.Invoke();
    }

}
