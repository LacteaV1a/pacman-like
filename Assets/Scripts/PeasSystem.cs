using Nox7atra.Mazes;
using System;
using System.Collections;
using UnityEngine;

public class PeasSystem : MonoBehaviour
{
    [SerializeField] private PeaConfig _peaConfig;
    [SerializeField] private PoolConfig _poolConfig;
    private MazePlayer _player;
    private W4Maze _maze;
    private PeaPool _peaPool;
    private Grid<Pea> _peasedGrid;
    private bool _isActive;
    private bool _isInitialized;

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

        ArrangePeas();
    }

    private void OnPlayerMoved(int x, int y)
    {
        var pea = _peasedGrid.GetValue(x, y);
        if (pea != null)
        {
            _peasedGrid.SetValue(x, y, null);
            _peaPool.Release(pea);
            StartCoroutine(TimerRoutine(_peaConfig.TimeSpawnSec, SetPea));
        }
    }

    private IEnumerator TimerRoutine(float time, Action callback)
    {
        yield return new WaitForSeconds(time);
        callback?.Invoke();
    }

    private void ArrangePeas()
    {
        for (int i = 0; i < _poolConfig.DefaultCapacity; i++)
        {
            SetPea();
        }
    }

    private void SetPea()
    {
        var coord = GetRandomCoord();
        if (_peasedGrid.GetValue(coord.x, coord.y) == null)
        {
            VisualizePeas(coord.x, coord.y);
        }
        else
        {
            SetPea();
        }

    }

    private Vector2Int GetRandomCoord()
    {
        var rand = new System.Random();
        var x = rand.Next(0, _maze.ColumnCount - 1);
        var y = rand.Next(0, _maze.RowCount - 1);

        return new Vector2Int(x, y);
    }

    private void VisualizePeas(int x, int y)
    {
        var pea = _peaPool.Get();
        _peasedGrid.SetValue(x, y, pea);

        var pos = _maze.GetCellWorldPosition(x, y);

        pea.SetPostion(pos.x,pos.y);
    }


    public void StopSystem()
    {
        if (_isActive == false) return;
        _player.Moved -= OnPlayerMoved;

        _isActive = false;
        _peasedGrid.Clear();
    }
}
