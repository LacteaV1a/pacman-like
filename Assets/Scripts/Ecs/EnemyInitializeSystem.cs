using Leopotam.EcsLite;
using UnityEngine;

public sealed class EnemyInitializeSystem : IEcsInitSystem
{
    private PoolConfig _enemyPoolConfig;

    public EnemyInitializeSystem(PoolConfig peaPoolConfig)
    {
        _enemyPoolConfig = peaPoolConfig;
    }
    public void Init(IEcsSystems systems)
    {
        var world = systems.GetWorld();

        var enemys = world.GetPool<EnemyComponent>();
        var worldObjs = world.GetPool<WorldObjectComponent>();
        var placeHolders = world.GetPool<MazePlaceholderComponent>();
        var followerPool = world.GetPool<FollowerComponent>();
        var movementPool = world.GetPool<MovementComponent>();

        var peaViewPoolEntity = world.NewEntity();

        ref var peaViewPoolComponent = ref world.AddComponentToEntity<EnemyViewPoolComponent>(peaViewPoolEntity);
        peaViewPoolComponent.EnemyViewPool = new GameObjectPool(_enemyPoolConfig);

        var filter = world.Filter<MazeComponent>().End();
        Vector2Int mazeSize = Vector2Int.zero;

        foreach (var i in filter)
        {
            var maze = world.GetComponent<MazeComponent>(i);
            mazeSize.x = maze.Maze.ColumnCount;
            mazeSize.y = maze.Maze.RowCount;
        }

        for (int i = 0; i < _enemyPoolConfig.DefaultCapacity; i++)
        {
            var entity = world.NewEntity();
            enemys.Add(entity);
            followerPool.Add(entity);
            ref var movement = ref movementPool.Add(entity);
            movement.Speed = 1.5f;

            ref var worldObjComponent = ref worldObjs.Add(entity);
            worldObjComponent.Transform = peaViewPoolComponent.EnemyViewPool.Get().transform;

            ref var placeholderComponent = ref placeHolders.Add(entity);
            placeholderComponent.CanSetPlace = true;

            placeholderComponent.MaxCoord = mazeSize / 2;
            placeholderComponent.MaxCoord = mazeSize;
        }
    }
}