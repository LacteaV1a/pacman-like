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
    private Transform _target;

    private float _timer;

    private bool _isChasing;

    public void Construct(MazeGraph graph, MazePathFinder mazePathFinder)
    {
        _mazeGraph = graph;
        _mazePathFinder = mazePathFinder;
        _timePathUpdateSec += Random.Range(0, 2) / 10f;
    }

    private void UpdatePath()
    {
        var targetPos = _mazeGraph.Maze.GetXY(new Vector2(_target.position.x, _target.position.z));
        var endCell = _mazeGraph.GetGraphCells(targetPos.x, targetPos.y);

        var pos = _mazeGraph.Maze.GetXY(new Vector2(transform.position.x, transform.position.z));
        var startCell = _mazeGraph.GetGraphCells(pos.x, pos.y);

        if(_shortestPath != null && _currentPathIndex < _shortestPath.Count)
        {
            startCell = _shortestPath[_currentPathIndex];
        }

        _shortestPath = _mazePathFinder.FindShortestPath(startCell, endCell);

        _currentPathIndex = 0;
    }

    private void Update()
    {
        if (_isChasing == false || _target == null || _shortestPath == null) return;

        if (_currentPathIndex < _shortestPath.Count)
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

    public void StartChasingTarget(Transform target)
    {
        _target = target;
        _isChasing = true;
        UpdatePath();
    }

    public void StopChasingTarget()
    {
        _isChasing = false;
        _shortestPath =null;
        _currentPathIndex = 0;
        _target = null;
        _timer = 0;
    }

    public bool IsEnable()
    {
        return gameObject.activeSelf;
    }
    public void SetPostion(float x, float z)
    {
        transform.position = new Vector3(x, 0, z);
    }
}