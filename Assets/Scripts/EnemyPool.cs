﻿using Nox7atra.Mazes;
using UnityEngine;

public class EnemyPool : Pool<MazeEnemy>
{
    private PoolConfig _poolConfig;
    private MazePlayer _player;
    private MazePathFinder _mazePathFinder;
    private MazeGraph _mazeGraph;
    public EnemyPool(MazePlayer player, MazePathFinder pathFinder, MazeGraph mazeGraph, PoolConfig poolConfig) : base(poolConfig)
    {
        _player = player;
        _poolConfig = poolConfig;
        _mazePathFinder = pathFinder;
        _mazeGraph = mazeGraph;
    }

    protected override MazeEnemy CreatePooledItem()
    {
        var enemy = (Object.Instantiate(_poolConfig.Prefab) as GameObject).GetComponent<MazeEnemy>();
        enemy.Construct(_player, _mazeGraph, _mazePathFinder);
        return enemy;
    }

    protected override void OnDestroyPoolObject(MazeEnemy obj)
    {
        Object.Destroy(obj);
    }

    protected override void OnReturnedToPool(MazeEnemy obj)
    {
        obj.gameObject.SetActive(false);
    }

    protected override void OnTakeFromPool(MazeEnemy obj)
    {
        obj.gameObject.SetActive(true);
    }

}
