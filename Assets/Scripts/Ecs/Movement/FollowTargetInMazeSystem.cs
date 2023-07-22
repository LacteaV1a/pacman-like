using Leopotam.EcsLite;
using Nox7atra.Mazes;
using UnityEngine;

public sealed class FollowTargetInMazeSystem : IEcsInitSystem, IEcsRunSystem
{
    private EcsFilter _followFilter;
    private EcsFilter _targetFilter;
    private EcsPool<FollowerComponent> _followPool;
    private EcsPool<MazeCoordComponent> _coordPool;
    private EcsPool<MovementComponent> _movementPool;
    private EcsPool<MovedMarker> _movedPool;
    private EcsFilter _mazeFilter;

    private W4Maze _maze;
    private MazeGraph _mazeGraph;
    private MazePathFinder _mazePathFinder;

    public void Init(IEcsSystems systems)
    {
        var world = systems.GetWorld();

        _followFilter = world.Filter<FollowerComponent>().Inc<MovementComponent>().Inc<MazeCoordComponent>().End();
        _targetFilter = world.Filter<PlayerComponent>().Inc<MazeCoordComponent>().End();
        _followPool = world.GetPool<FollowerComponent>();
        _coordPool = world.GetPool<MazeCoordComponent>();
        _movementPool = world.GetPool<MovementComponent>();
        _movedPool = world.GetPool<MovedMarker>();

        _mazeFilter = world.Filter<MazeComponent>().End();

        foreach (var entity in _mazeFilter)
        {
            ref var component = ref world.GetComponent<MazeComponent>(entity);
            _maze = component.Maze;
        }
        _mazeGraph = new MazeGraph(_maze, false);
        _mazePathFinder = new MazePathFinder();

        foreach(var entity in _followFilter)
        {
            UpdatePath(entity);
        }
    }


    public void Run(IEcsSystems systems)
    {


        foreach (var entity in _followFilter)
        {
            ref var followComp = ref _followPool.Get(entity);
            foreach (var target in _targetFilter)
            {
                ref var coord = ref _coordPool.Get(target);

                followComp.Target = coord.Value;
            }

            ref var movement = ref _movementPool.Get(entity);

            if (_movedPool.Has(entity))
            {
                movement.Direction =  UpdatePath(entity);
            }


        }

    }

    private Vector2Int UpdatePath(int entity)
    {
        ref var foolowComp = ref _followPool.Get(entity);
        ref var coord = ref _coordPool.Get(entity);

        var targetPos = foolowComp.Target;
        var endCell = _mazeGraph.GetGraphCells(targetPos.x, targetPos.y);

        var pos = coord.Value;
        var startCell = _mazeGraph.GetGraphCells(pos.x, pos.y);

        var path = _mazePathFinder.FindShortestPathDirection(startCell, endCell);

        return path.Count > 0 ? path[0] : Vector2Int.zero;

    }
}
