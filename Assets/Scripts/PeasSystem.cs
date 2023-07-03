using Nox7atra.Mazes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeasSystem : GameSystem
{
    [SerializeField] private PeaConfig _peaConfig;
    [SerializeField] private PoolConfig _poolConfig;
    private MazePlayer _player;
    private W4Maze _maze;
    private PeaPool _peaPool;
    private Grid<Pea> _peasedGrid;
    private bool _isActive;
    private bool _isInitialized;
    private List<Pea> _peas = new();

    public void Initialize(MazePlayer player, W4Maze maze)
    {
        _player = player;
        _maze = maze;
        _peaPool = new PeaPool(_poolConfig);
        _peasedGrid = new Grid<Pea>(maze.ColumnCount, maze.RowCount);

        _isInitialized = true;
    }

    public void StartSystem()
    {
        if (_isActive || _isInitialized == false) return;
        _isActive = true;
        _player.Moved += OnPlayerMoved;

        SpawnPeas();
    }

    private void OnPlayerMoved(int x, int y)
    {
        var pea = _peasedGrid.GetValue(x, y);
        if (pea != null)
        {
            _peasedGrid.SetValue(x, y, null);
            _peaPool.Release(pea);
            StartCoroutine(TimerRoutine(_peaConfig.TimeSpawnSec, () => {
                if(_isActive)
                    SetPea();
            }));
        }
    }

    private IEnumerator TimerRoutine(float time, Action callback)
    {
        yield return new WaitForSeconds(time);
        callback?.Invoke();
    }

    private void SpawnPeas()
    {
        _peas.Clear();
        for (int i = 0; i < _poolConfig.DefaultCapacity; i++)
        {
            SetPea();
        }
    }

    private void SetPea()
    {
        var coord = _maze.GetRandomCoord();
        if (_peasedGrid.GetValue(coord.x, coord.y) == null)
        {
            VisualizePeas(coord.x, coord.y);
        }
        else
        {
            SetPea();
        }

    }
    private void VisualizePeas(int x, int y)
    {
        var pea = _peaPool.Get();
        _peasedGrid.SetValue(x, y, pea);

        var pos = _maze.GetCellWorldPosition(x, y);

        pea.SetPostion(pos.x,pos.y);
        if (_peas.Contains(pea) == false)
            _peas.Add(pea);
    }


    public void StopSystem()
    {
        if (_isActive == false) return;

        _isActive = false;
        foreach (var pea in _peas)
        {
            _peaPool.Release(pea);
        }

        _player.Moved -= OnPlayerMoved;

        _peasedGrid.Clear();
    }

    public override void OnFinishGame()
    {
        StopSystem();
    }

    public override void OnStartGame()
    {
        StartSystem();
    }
}
