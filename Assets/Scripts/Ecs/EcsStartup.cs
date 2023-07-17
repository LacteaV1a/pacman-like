using Leopotam.EcsLite;
using UnityEngine;

sealed class EcsStartup : MonoBehaviour {
    EcsWorld _world;
    IEcsSystems _systems;
    [SerializeField] private MazeConfig _mazeConfig;
    [SerializeField] private MazeViewConfig _mazeViewConfig;
    [SerializeField] private PlayerConfig _playerConfig;

    void Start() {
        _world = new EcsWorld();
        _systems = new EcsSystems(_world);
        _systems
            .Add(new PlayerInputSystem())
            .Add (new MazeInitializeSystem(_mazeConfig))
            .Add(new MazeVisualizeSystem(_mazeViewConfig))
            .Add(new PlayerInitializeSystem(_playerConfig))

#if UNITY_EDITOR
            .Add(new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem())
#endif
                .Init();
    }

    void Update() {
        _systems?.Run();
    }

    void OnDestroy() {
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


//public sealed class PlayerInputSystem : IEcsRunSystem
//{
//    private readonly EcsFilter<PlayerComponent, MovementComponent> players = null;

//    public void Run(IEcsSystems systems)
//    {
//        foreach (var i in players)
//        {
//            var playerNum = players.Get1(i).num;
//            var yAxis = Input.GetAxis($"Player{playerNum.ToString()}Y");
//            var xAxis = Input.GetAxis($"Player{playerNum.ToString()}X");

//            ref var movement = ref players.Get2(i);
//            if (yAxis > 0)
//            {
//                movement.heading = Directions.Up;
//            }
//            else if (yAxis < 0)
//            {
//                movement.heading = Directions.Down;
//            }
//            else if (xAxis > 0)
//            {
//                movement.heading = Directions.Right;
//            }
//            else if (xAxis < 0)
//            {
//                movement.heading = Directions.Left;
//            }
//        }
//    }
//}