using Leopotam.EcsLite;
using UnityEngine;

public sealed class EcsStartup : MonoBehaviour {
    EcsWorld _world;
    IEcsSystems _systems;
    [SerializeField] private MazeConfig _mazeConfig;
    [SerializeField] private MazeViewConfig _mazeViewConfig;
    [SerializeField] private PlayerConfig _playerConfig;
    [SerializeField] private PoolConfig _peaPoolConfig;

    private void Start() {
        _world = new EcsWorld();
        _systems = new EcsSystems(_world);
        _systems
            .Add(new PlayerInputSystem())
            .Add(new MazeInitializeSystem(_mazeConfig))
            .Add(new MazeVisualizeSystem(_mazeViewConfig))
            .Add(new PlayerInitializeSystem(_playerConfig))
            .Add(new PeasInitializeSystem(_peaPoolConfig))
            .Add(new PlayerMovementInMazeSystem())
            .Add(new MazePlaceholderSystem())
            .Add(new GrabItemSystem())
            //.Add(new PeaSystem())

#if UNITY_EDITOR
            .Add(new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem())
#endif
                .Init();
    }

    private void Update() {
        _systems?.Run();
    }

    private void OnDestroy() {
        if (_systems != null) {
            _systems.Destroy();
            _systems = null;
        }

        if (_world != null) {
            _world.Destroy();
            _world = null;
        }
    }
}
