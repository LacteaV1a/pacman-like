using Leopotam.EcsLite;
using Nox7atra.Mazes;

public sealed class MazeInitializeSystem : IEcsInitSystem
{

    private MazeConfig _config;

    public MazeInitializeSystem(MazeConfig config)
    {
        _config = config;
    }

    public void Init(IEcsSystems systems)
    {
        var world = systems.GetWorld();

        var mazePool = world.GetPool<MazeComponent>();

        var entity = world.NewEntity();

        mazePool.Add(entity);

        ref var component = ref mazePool.Get(entity);

        component.Maze = SetupMaze();
    }

    private W4Maze SetupMaze()
    {
        var mazeCreator = new MazeCreator(_config, new CustomEllerGenerator(_config.WallOccupancy));
        return mazeCreator.Create();
    }
}
