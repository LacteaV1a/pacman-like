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
    private EcsPool<MazePlaceholderComponent> _randomPlaceholderPool;
    private EcsPool<WorldObjectComponent> _worldObjPool;
    private EcsPool<MazeCoordComponent> _coordsPool;

    private W4Maze _maze;
    private Dictionary<PlaceholderLayer, Grid<int>> _grids = new();

    public void Init(IEcsSystems systems)
    {
        var world = systems.GetWorld();
        _filter = world.Filter<MazePlaceholderComponent>().Inc<WorldObjectComponent>().End();

        _randomPlaceholderPool = world.GetPool<MazePlaceholderComponent>();
        _worldObjPool = world.GetPool<WorldObjectComponent>();
        _coordsPool = world.GetPool<MazeCoordComponent>();

        var mazeFilter = world.Filter<MazeComponent>().End();

        foreach (var item in mazeFilter)
        {
            ref var maze = ref world.GetComponent<MazeComponent>(item);
            _maze = maze.Maze;
        }
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

            if (_grids[placeholder.Layer].GetValue(randomCoord.x, randomCoord.y) == 0)
                _grids[placeholder.Layer].SetValue(randomCoord.x, randomCoord.y, i);

            var pos = _maze.GetCellWorldPosition(randomCoord.x, randomCoord.y);

            ref var worldObj = ref _worldObjPool.Get(i);
            worldObj.Transform.position = new Vector3(pos.x, 0, pos.y);

            _coordsPool.Add(i);
            ref var coord = ref _coordsPool.Get(i);
            coord.Value = randomCoord;

            placeholder.CanSetPlace = false;

        }
    }

    private Vector2Int GetRandomCoord(Vector2Int minCoord, Vector2Int maxCoord)
    {
        var randomCoordX = Random.Range(minCoord.x, maxCoord.x);
        var randomCoordY = Random.Range(minCoord.y, maxCoord.y);
        return new Vector2Int(randomCoordX, randomCoordY);
    }
}

