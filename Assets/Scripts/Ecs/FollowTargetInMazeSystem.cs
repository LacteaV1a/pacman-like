using Leopotam.EcsLite;
using Nox7atra.Mazes;

public sealed class FollowTargetInMazeSystem : IEcsInitSystem, IEcsRunSystem
{
    private EcsFilter _followFilter;
    private EcsPool<FollowerComponent> _followPool;
    private EcsFilter _mazeFilter;
    private W4Maze _maze;
    private MazeGraph _mazeGraph;

    public void Init(IEcsSystems systems)
    {
        var world = systems.GetWorld();

        _followFilter = world.Filter<FollowerComponent>().End();
        _followPool = world.GetPool<FollowerComponent>();

        _mazeFilter = world.Filter<MazeComponent>().End();

        foreach (var entity in _mazeFilter)
        {
            ref var component = ref world.GetComponent<MazeComponent>(entity);
            _maze = component.Maze;
        }
        _mazeGraph = new MazeGraph(_maze, false);
    }


    public void Run(IEcsSystems systems)
    {
        foreach (var entity in _followFilter)
        {
            ref var follower = ref _followPool.Get(entity);
        }
    }
}
