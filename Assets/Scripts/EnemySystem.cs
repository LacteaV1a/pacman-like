using Nox7atra.Mazes;
using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemySystem : GameSystem
{
    [SerializeField] private PoolConfig _poolConfig;
    private MazePlayer _player;
    private W4Maze _maze;
    private EnemyPool _enemyPool;
    private MazePathFinder _pathFinder;
    private bool _isActive;
    private bool _isInitialized;
    private List<MazeEnemy> _enemys = new();
    public List<MazeEnemy> Enemys => _enemys;
    
    public void Initialize(MazePlayer player, W4Maze maze)
    {
        _player = player;
        _maze = maze;
        _pathFinder = new MazePathFinder();
        _enemyPool = new EnemyPool(_pathFinder, new MazeGraph(maze,false), _poolConfig);

        _isInitialized = true;
    }

    public override void OnFinishGame()
    {
        foreach (var enemy in _enemys)
        {
            _enemyPool.Release(enemy);
        }
    }

    public override void OnStartGame()
    {
        StartSystem();
    }

    public void StartSystem()
    {
        _enemys.Clear();
        for (int i = 0; i < _poolConfig.DefaultCapacity; i++)
        {
            var enemy = _enemyPool.Get();
            var coord = _maze.GetRandomCoord();
            var pos = _maze.GetCellWorldPosition(coord.x, coord.y);
            enemy.SetPostion(pos.x, pos.y);
            enemy.StartChasingTarget(_player.transform);
            _enemys.Add(enemy);
        }
        _isActive = true;
    }



    public void Pause()
    {
        foreach (var enemy in _enemys)
        {
            enemy.StopChasingTarget();
        }
        _isActive = false;
    }

    public void Resume()
    {
        foreach (var enemy in _enemys)
        {
            enemy.StartChasingTarget(_player.transform);
        }
        _isActive = true;
    }


}
