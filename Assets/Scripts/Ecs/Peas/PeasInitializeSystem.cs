using Leopotam.EcsLite;
using UnityEngine;

public sealed class PeasInitializeSystem : IEcsInitSystem
{
    private PoolConfig _peaPoolConfig;
    private GameObjectPool _peaViewPool;

    public PeasInitializeSystem(PoolConfig peaPoolConfig)
    {
        _peaPoolConfig = peaPoolConfig;
    }

    public void Init(IEcsSystems systems)
    {
        var world = systems.GetWorld();

        var peas = world.GetPool<PeaComponent>();
        var worldObjs = world.GetPool<WorldObjectComponent>();
        var placeHolders = world.GetPool<MazePlaceholderComponent>();
        var canGrab = world.GetPool<CanGrabComponent>();

        var peaViewPoolEntity = world.NewEntity();

        ref var peaViewPoolComponent = ref world.AddComponentToEntity<PeaViewPoolComponent>(peaViewPoolEntity);
        _peaViewPool = peaViewPoolComponent.PeaViewPool = new GameObjectPool(_peaPoolConfig);
       

        var filter = world.Filter<MazeComponent>().End();
        Vector2Int mazeSize = Vector2Int.zero;

        foreach (var i in filter)
        {
            var maze = world.GetComponent<MazeComponent>(i);
            mazeSize.x = maze.Maze.ColumnCount;
            mazeSize.y = maze.Maze.RowCount;
        }
        var cellsCount = mazeSize.x * mazeSize.y;
        var capacity = _peaPoolConfig.DefaultCapacity <= cellsCount ? _peaPoolConfig.DefaultCapacity : cellsCount;

        for (int i = 0; i < capacity; i++)
        {
            var entity = world.NewEntity();
            peas.Add(entity);
            worldObjs.Add(entity);
            placeHolders.Add(entity);
            ref var canGrabComponet = ref canGrab.Add(entity);
            canGrabComponet.Value = true;


            ref var worldObjComponent = ref worldObjs.Get(entity);
            worldObjComponent.Transform = peaViewPoolComponent.PeaViewPool.Get().transform;
            worldObjComponent.Transform.name += i;

            ref var placeholderComponent = ref placeHolders.Get(entity);
            placeholderComponent.CanSetPlace = true;
            placeholderComponent.MaxCoord = mazeSize;
        }
    }
}
