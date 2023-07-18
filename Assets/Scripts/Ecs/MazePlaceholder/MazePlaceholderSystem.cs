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
    private EcsPool<MazeRandomPlaceholderComponent> _randomPlaceholderPool;
    private EcsPool<WorldObjectComponent> _worldObjPool;
    private W4Maze _maze;
    private Dictionary<PlaceholderLayer, Grid<int>> _grids = new();

    public void Init(IEcsSystems systems)
    {
        var world = systems.GetWorld();
        _filter = world.Filter<MazeRandomPlaceholderComponent>().Inc<WorldObjectComponent>().End();

        _randomPlaceholderPool = world.GetPool<MazeRandomPlaceholderComponent>();
        _worldObjPool = world.GetPool<WorldObjectComponent>();

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


            ref var worldObj = ref _worldObjPool.Get(i);
            var randomCoordX = Random.Range(placeholder.MinCoord.x, placeholder.MaxCoord.x);
            var randomCoordY = Random.Range(placeholder.MinCoord.y, placeholder.MaxCoord.y);

            if(_grids[placeholder.Layer].GetValue(randomCoordX, randomCoordY) == 0)
                _grids[placeholder.Layer].SetValue(randomCoordX, randomCoordY, i);

            worldObj.Transform.position = _maze.GetCellWorldPosition(randomCoordX, randomCoordY);

            placeholder.CanSetPlace = false;

        }
    }
}

