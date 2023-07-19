using Leopotam.EcsLite;
using UnityEngine;

public sealed class GrabItemSystem : IEcsInitSystem, IEcsRunSystem
{
    private EcsFilter _grabFilter;
    private EcsFilter _playerFilter;
    private EcsPool<MazeCoordComponent> _mazeCoordPool;
    private EcsPool<GrabedMarker> _grabedPool;
    private int _player;

    public void Init(IEcsSystems systems)
    {
        var world = systems.GetWorld();
        _grabFilter = world.Filter<CanGrabComponent>().Inc<MazeCoordComponent>().Exc<GrabedMarker>().End();

        _playerFilter = world.Filter<PlayerComponent>().Inc<MazeCoordComponent>().End();

        _mazeCoordPool = world.GetPool<MazeCoordComponent>();
        _grabedPool = world.GetPool<GrabedMarker>();
        foreach (var item in _playerFilter)
        {
            _player = item;
        }
    }

    public void Run(IEcsSystems systems)
    {
        ref var playerCoord = ref _mazeCoordPool.Get(_player);
        foreach (var item in _grabFilter)
        {
            ref var itemCoord = ref _mazeCoordPool.Get(item);

            if (playerCoord.Value == itemCoord.Value && _grabedPool.Has(item) == false)
            {
                _grabedPool.Add(item);
            }
        }
    }
}

