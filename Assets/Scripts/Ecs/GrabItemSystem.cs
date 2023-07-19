using Leopotam.EcsLite;

public sealed class GrabItemSystem : IEcsInitSystem, IEcsRunSystem
{
    private EcsFilter _grabFilter;
    private EcsFilter _playerFilter;
    private EcsPool<MazeCoordComponent> _mazeCoordPool;
    private EcsPool<GrabedMarker> _grabedPool;

    public void Init(IEcsSystems systems)
    {
        var world = systems.GetWorld();
        _grabFilter = world.Filter<CanGrabComponent>().Inc<MazeCoordComponent>().Exc<GrabedMarker>().End();

        _playerFilter = world.Filter<PlayerComponent>().Inc<MovedMarker>().Inc<MazeCoordComponent>().End();

        _mazeCoordPool = world.GetPool<MazeCoordComponent>();
        _grabedPool = world.GetPool<GrabedMarker>();

    }

    public void Run(IEcsSystems systems)
    {
        foreach (var player in _playerFilter)
        {
            ref var playerCoord = ref _mazeCoordPool.Get(player);
            foreach (var item in _grabFilter)
            {
                ref var itemCoord = ref _mazeCoordPool.Get(item);

                if(playerCoord.Value == itemCoord.Value)
                {
                    _grabedPool.Add(item);
                }
            }
        }
    }
}

