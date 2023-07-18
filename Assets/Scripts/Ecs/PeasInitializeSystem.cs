using Leopotam.EcsLite;
using UnityEngine;

public sealed class PeasInitializeSystem : IEcsInitSystem
{
    private PoolConfig _peaPoolConfig;

    public PeasInitializeSystem(PoolConfig peaPoolConfig)
    {
        _peaPoolConfig = peaPoolConfig;
    }
    public void Init(IEcsSystems systems)
    {
        var world = systems.GetWorld();

        var peas = world.GetPool<PeaComponent>();
        var worldObjs = world.GetPool<WorldObjectComponent>();
        var placeHolders = world.GetPool<MazeRandomPlaceholderComponent>();

        var peaViewPoolEntity = world.NewEntity();

        ref var peaViewPoolComponent = ref world.AddComponentToEntity<PeaViewPoolComponent>(peaViewPoolEntity);
        peaViewPoolComponent.PeaViewPool = new PeaPool(_peaPoolConfig);

        var filter = world.Filter<MazeComponent>().End();
        Vector2Int mazeSize = Vector2Int.zero;

        foreach (var i in filter)
        {
            var maze = world.GetComponent<MazeComponent>(i);
            mazeSize.x = maze.Maze.ColumnCount;
            mazeSize.y = maze.Maze.RowCount;
        }

        for (int i = 0; i < _peaPoolConfig.DefaultCapacity; i++)
        {
            var entity = world.NewEntity();
            peas.Add(entity);
            worldObjs.Add(entity);
            placeHolders.Add(entity);

            ref var peaComponent = ref peas.Get(entity);
            peaComponent.View = peaViewPoolComponent.PeaViewPool.Get();

            ref var worldObjComponent = ref worldObjs.Get(entity);
            worldObjComponent.Transform = peaComponent.View.transform;

            ref var placeholderComponent = ref placeHolders.Get(entity);
            placeholderComponent.CanSetPlace = true;
            placeholderComponent.MaxCoord = mazeSize;
        }
    }
}

