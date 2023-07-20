using Leopotam.EcsLite;
using System.Collections.Generic;

public sealed class MazeClearSystem : IEcsInitSystem, IEcsRunSystem
{
    private EcsFilter _grabFilter;
    private EcsFilter _gridFilter;
    private EcsPool<MazeCoordComponent> _mazeCoordPool;
    private EcsPool<MazePlaceholderComponent> _placeholderPool;
    private Dictionary<PlaceholderLayer, Grid<int>> _grids;


    public void Init(IEcsSystems systems)
    {
        var world = systems.GetWorld();
        _grabFilter = world.Filter<MazePlaceholderComponent>().Inc<GrabedMarker>().Inc<MazeCoordComponent>().End();
        _gridFilter = world.Filter<GridsComponent>().End();
        _mazeCoordPool = world.GetPool<MazeCoordComponent>();
        _placeholderPool = world.GetPool<MazePlaceholderComponent>();

        foreach (var entity in _gridFilter)
        {
            ref var component = ref world.GetComponent<GridsComponent>(entity);
            _grids = component.Grids;
        }

    }

    public void Run(IEcsSystems systems)
    {
        foreach (var entity in _grabFilter)
        {
            ref var _mazeCoord = ref _mazeCoordPool.Get(entity);
            var coord = _mazeCoord.Value;

            ref var placeholder = ref _placeholderPool.Get(entity);
            var layer = placeholder.Layer;

            var grid = _grids[layer];

            grid.SetValue(coord.x, coord.y, 0);
            _mazeCoordPool.Del(entity);
        }
    }
}


