using Leopotam.EcsLite;
using UnityEngine;

public sealed class GrabItemSystem : IEcsInitSystem, IEcsRunSystem
{
    private EcsFilter _grabFilter;
    private EcsFilter _playerFilter;
    private EcsPool<MazeCoordComponent> _mazeCoordPool;
    private EcsPool<GrabedMarker> _grabedPool;
    private EcsPool<CanGrabComponent> _canGrabPool;

    public void Init(IEcsSystems systems)
    {
        var world = systems.GetWorld();
        _grabFilter = world.Filter<CanGrabComponent>().Inc<MazeCoordComponent>().Exc<GrabedMarker>().End();

        _playerFilter = world.Filter<PlayerComponent>().Inc<MazeCoordComponent>().End();

        _mazeCoordPool = world.GetPool<MazeCoordComponent>();
        _grabedPool = world.GetPool<GrabedMarker>();
        _canGrabPool = world.GetPool<CanGrabComponent>();
    }

    public void Run(IEcsSystems systems)
    {
        foreach (var player in _playerFilter)
        {
            ref var playerCoord = ref _mazeCoordPool.Get(player);
            foreach (var item in _grabFilter)
            {
                ref var canGrabComponent = ref _canGrabPool.Get(item);
                if (canGrabComponent.Value == false) continue;

                ref var itemCoord = ref _mazeCoordPool.Get(item);

                if (playerCoord.Value == itemCoord.Value)
                {
                    _grabedPool.Add(item);
                    ref var canGrab = ref _canGrabPool.Get(item);
                    canGrab.Value = false;
                }
            }
        }
    }
}

