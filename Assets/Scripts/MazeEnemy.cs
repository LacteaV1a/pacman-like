using Nox7atra.Mazes;
using System.Collections.Generic;
using UnityEngine;

public class MazeEnemy : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _timePathUpdateSec;
    private MazeGraph _mazeGraph;
    private List<MazeGraphCell> _shortestPath;
    private int _currentPathIndex = 0;
    private MazePathFinder _mazePathFinder;
    private MazePlayer _player;

    private float _timer;

    public void Construct(MazePlayer player, MazeGraph graph, MazePathFinder mazePathFinder)
    {
        _player = player;
        _mazeGraph = graph;
        _mazePathFinder = mazePathFinder;
        _timePathUpdateSec += Random.Range(0, 10) / 10f;
    }

    private void UpdatePath()
    {
        var playerCoord = _player.GetCoord();
        var endCell = _mazeGraph.GetGraphCells(playerCoord.x, playerCoord.y);

        var pos = _mazeGraph.Maze.GetXY(new Vector2(transform.position.x, transform.position.z));
        var startCell = _mazeGraph.GetGraphCells(pos.x, pos.y);

        _shortestPath = _mazePathFinder.FindShortestPath(startCell, endCell);
        _currentPathIndex = 0;
    }

    private void Update()
    {
        if (_shortestPath != null && _currentPathIndex < _shortestPath.Count)
        {
            MazeGraphCell currentCell = _shortestPath[_currentPathIndex];
            Vector3 targetPosition = new Vector3(currentCell.Position.x, 0f, currentCell.Position.y);
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * _speed);

            if (transform.position == targetPosition)
            {
                _currentPathIndex++;
            }
        }

        if(_timer < _timePathUpdateSec)
        {
            _timer += Time.deltaTime;
        }
        else
        {
            _timer = 0;
            UpdatePath();
        }
    }

    public void SetPostion(float x, float z)
    {
        transform.position = new Vector3(x, 0, z);
    }
}