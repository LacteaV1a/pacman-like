using Nox7atra.Mazes;
using System;
using System.Collections;
using UnityEngine;

public class MazePlayer : GameSystem
{
    [SerializeField] private float _timeToMove = 0.3f;
    [SerializeField] private float _yHeight = 0.5f;
    private W4Maze _maze;
    private Vector3 _movePoint;
    private bool _isMoved;
    private bool _isActive;

    Coroutine routine;
    public event Action<int, int> Moved;

    public void Construct(W4Maze maze)
    {
        _maze = maze;
    }

    private void Update()
    {
        if (_maze == null || _isActive == false) return;

        float vert = Input.GetAxis("Vertical");
        float hor = Input.GetAxis("Horizontal");
        float step = _timeToMove * Time.deltaTime;

        var pos = _maze.GetXY(new Vector2(transform.position.x, transform.position.z));
        var x = pos.x;
        var z = pos.y;
        W4Cell cell = _maze.GetCell(x, z);
         
        transform.position = Vector3.MoveTowards(transform.position, _movePoint, step);

        if (transform.position == _movePoint)
        {
            if (hor > 0 && cell.RightWall == false)
            {
                //StartCoroutine(MoveRoutine(Vector3.right));
                Move(Vector3.right);
            }

            if (hor < 0 && cell.LeftWall == false)
            {
                //StartCoroutine(MoveRoutine(Vector3.left));
                Move(Vector3.left);
            }

            if (vert > 0 && cell.TopWall == false)
            {
                //StartCoroutine(MoveRoutine(Vector3.forward));
                Move(Vector3.forward);

            }

            if (vert < 0 && cell.BotWall == false)
            {
                //StartCoroutine(MoveRoutine(Vector3.back));
                Move(Vector3.back);
            }

            if(_isMoved == false)
            {
                _isMoved = true;
                Moved.Invoke(pos.x, pos.y);
            }

        }
    }

    private IEnumerator MoveRoutine(Vector3 dir)
    {
        _isMoved = true;

        float elapsedTime = 0;

        var origPos = transform.position;
        var targetPos = origPos + dir;

        while(elapsedTime < _timeToMove)
        {
            transform.position = Vector3.Lerp(origPos, targetPos, (elapsedTime / _timeToMove));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPos;

        var pos = GetCoord();
        Moved?.Invoke(pos.x, pos.y);


        _isMoved = false;
    }

    public Vector2Int GetCoord()
    {
        return _maze.GetXY(new Vector2(transform.position.x, transform.position.z));
    }

    public void Move(int x, int z)
    {
        var pos = _maze.GetCellWorldPosition(x, z);
        _movePoint = new Vector3(pos.x, _yHeight, pos.y);
        Moved.Invoke(x, z);
    }

    public void Move(Vector3 dir)
    {
        _movePoint = transform.position + dir * _maze.CellsSize;
        _isMoved = false;
    }

    public override void OnFinishGame()
    {
        _isActive = false;
    }

    public override void OnStartGame()
    {
        var pos = _maze.GetCellWorldPosition(0, 0);
        transform.position = new Vector3(pos.x, _yHeight, pos.y);
        _movePoint = transform.position;

        _isActive = true;
    }
}
