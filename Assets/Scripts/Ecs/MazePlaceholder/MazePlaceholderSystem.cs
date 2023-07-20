using Leopotam.EcsLite;
using Nox7atra.Mazes;
using System.Collections.Generic;
using UnityEngine;

public enum PlaceholderLayer
{
    PEA = 0,
    ENEMY = 1,   
}

public sealed class MazePlaceholderSystem : IEcsInitSystem, IEcsRunSystem
{
    private EcsFilter _filter;
    private EcsFilter _grabFilter;
    private EcsPool<MazePlaceholderComponent> _randomPlaceholderPool;
    private EcsPool<WorldObjectComponent> _worldObjPool;
    private EcsPool<MazeCoordComponent> _coordsPool;
    private EcsPool<GridsComponent> _gridsPool;
    private W4Maze _maze;
    private Dictionary<PlaceholderLayer, Grid<int>> _grids;

    public void Init(IEcsSystems systems)
    {
        var world = systems.GetWorld();
        _filter = world.Filter<MazePlaceholderComponent>().Inc<WorldObjectComponent>().Exc<MazeCoordComponent>().End();

        _randomPlaceholderPool = world.GetPool<MazePlaceholderComponent>();
        
        _worldObjPool = world.GetPool<WorldObjectComponent>();
        _coordsPool = world.GetPool<MazeCoordComponent>();
        _gridsPool = world.GetPool<GridsComponent>();

        var gridsEntity = world.NewEntity();

        ref var gridsComponent = ref _gridsPool.Add(gridsEntity);
        _grids = gridsComponent.Grids = new();

        var mazeFilter = world.Filter<MazeComponent>().End();

        foreach (var item in mazeFilter)
        {
            ref var maze = ref world.GetComponent<MazeComponent>(item);
            _maze = maze.Maze;
        }
        Run(systems);
    }

    public void Run(IEcsSystems systems)
    {
        foreach (var i in _filter)
        {
            ref var placeholder = ref _randomPlaceholderPool.Get(i);
            if (placeholder.CanSetPlace == false) continue;

            if (_grids.ContainsKey(placeholder.Layer) == false)
            {
                _grids.Add(placeholder.Layer, new Grid<int>(_maze.ColumnCount, _maze.RowCount));
            }

            var randomCoord = GetRandomCoord(placeholder.MinCoord, placeholder.MaxCoord);

            var grid = _grids[placeholder.Layer];

            if (grid.GetValue(randomCoord.x, randomCoord.y) != 0)
            {
                if (grid.TryGetEmptyCell(out int x, out int y))
                {
                    randomCoord = new Vector2Int(x, y);
                }
                //else
                //{
                //    placeholder.CanSetPlace = false;
                //    return;
                //}
            }

            grid.SetValue(randomCoord.x, randomCoord.y, i);

            SetPosition(i, randomCoord);

            placeholder.CanSetPlace = false;

        }
    }

    private void SetPosition(int entity, Vector2Int position)
    {
        var pos = _maze.GetCellWorldPosition(position.x, position.y);

        ref var worldObj = ref _worldObjPool.Get(entity);
        worldObj.Transform.position = new Vector3(pos.x, 0, pos.y);


        ref var coord = ref _coordsPool.Add(entity);
        coord.Value = position;
    }

    private Vector2Int GetRandomCoord(Vector2Int minCoord, Vector2Int maxCoord)
    {
        var randomCoordX = Random.Range(minCoord.x, maxCoord.x);
        var randomCoordY = Random.Range(minCoord.y, maxCoord.y);
        return new Vector2Int(randomCoordX, randomCoordY);
    }
}


