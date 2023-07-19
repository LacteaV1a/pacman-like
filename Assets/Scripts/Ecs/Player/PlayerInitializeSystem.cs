using Leopotam.EcsLite;
using UnityEngine;

public sealed class PlayerInitializeSystem : IEcsInitSystem
{
    private PlayerConfig _config;

    public PlayerInitializeSystem(PlayerConfig config)
    {
        _config = config;
    }

    public void Init(IEcsSystems systems)
    {
        var world = systems.GetWorld();
        var entity = world.NewEntity();

        var playerPool = world.GetPool<PlayerComponent>();
        var worldObjPool = world.GetPool<WorldObjectComponent>();
        var mazeCoord = world.GetPool<MazeCoordComponent>();

        var mazePool = world.GetPool<MazeComponent>();

        var filter = world.Filter<MazeComponent>().End();

        ref var mazeCoordComponent = ref mazeCoord.Add(entity);
        mazeCoordComponent.Value = _config.SpawnMazeCoord;

        ref var player = ref playerPool.Add(entity);
        ref var worldObj = ref worldObjPool.Add(entity);

        worldObj.Transform = Object.Instantiate(_config.Prefab, Vector3.zero, Quaternion.identity).transform;
        player.Speed = _config.Speed;

        foreach (var i in filter)
        {
            ref var maze = ref mazePool.Get(i);

            Vector2 pos;
            if (_config.SpawnMazeCoord.x >= maze.Maze.ColumnCount || _config.SpawnMazeCoord.y >= maze.Maze.RowCount)
            {
                pos = maze.Maze.GetCellWorldPosition(maze.Maze.ColumnCount - 1, maze.Maze.RowCount - 1);
            }
            else
            {
                pos = maze.Maze.GetCellWorldPosition(_config.SpawnMazeCoord.x, _config.SpawnMazeCoord.y);
            }

            worldObj.Transform.position = new Vector3(pos.x, _config.Ypos, pos.y);
        }
    }
}
