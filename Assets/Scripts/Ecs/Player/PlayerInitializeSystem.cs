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

        var mazePool = world.GetPool<MazeComponent>();

        var filter = world.Filter<MazeComponent>().End();

        playerPool.Add(entity);
        worldObjPool.Add(entity);

        ref var player = ref playerPool.Get(entity);
        ref var worldObj = ref worldObjPool.Get(entity);

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
