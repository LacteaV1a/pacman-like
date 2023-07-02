using Nox7atra.Mazes;
using System;
using System.Collections;
using UnityEngine;

public class MazePlayer : MonoBehaviour
{
    [SerializeField] private float _timeToMove = 0.3f;
    [SerializeField] private float _yHeight = 0.5f;
    private W4Maze _maze;
    private Vector3 _movePoint;
    private bool _isMoving;

    Coroutine routine;
    public event Action<int, int> Moved;

    public void Construct(W4Maze maze)
    {
        _maze = maze;
        var pos = _maze.GetCellWorldPosition(0, 0);
        transform.position = new Vector3(pos.x, _yHeight, pos.y);
        _movePoint = transform.position;
    }

    private void Update()
    {
        if (_maze == null) return;

        float vert = Input.GetAxis("Vertical");
        float hor = Input.GetAxis("Horizontal");
        float step = _timeToMove * Time.deltaTime;

        var pos = _maze.GetXY(new Vector2(transform.position.x, transform.position.z));
        var x = pos.x;
        var z = pos.y;
        W4Cell cell = _maze.GetCell(x, z);

        //transform.position = Vector3.MoveTowards(transform.position, _movePoint, step);

        //if (Vector3.Distance(transform.position, _movePoint) <= 0.1f)
        //{
        if ((hor > 0 && cell.RightWall == false) && _isMoving == false)
        {
            StartCoroutine(MoveRoutine(Vector3.right));
        }

        if ((hor < 0 && cell.LeftWall == false) && _isMoving == false)
        {
            StartCoroutine(MoveRoutine(Vector3.left));
        }

        if (vert > 0 && cell.TopWall == false && _isMoving == false)
        {
            StartCoroutine(MoveRoutine(Vector3.forward));
        }

        if (vert < 0 && cell.BotWall == false && _isMoving == false)
        {
            StartCoroutine(MoveRoutine(Vector3.back));
        }



    }

    private IEnumerator MoveRoutine(Vector3 dir)
    {
        _isMoving = true;

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
        Moved.Invoke(pos.x, pos.y);


        _isMoving = false;
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
        Debug.Log("Move");
    }
}
