using Nox7atra.Mazes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class W4CellView : MonoBehaviour
{
    [SerializeField] private GameObject _leftWall;
    [SerializeField] private GameObject _rightWall;
    [SerializeField] private GameObject _topWall;
    [SerializeField] private GameObject _botWall;

    public void Init(W4Cell cell)
    {
        _leftWall.SetActive(cell.LeftWall);
        _rightWall.SetActive(cell.RightWall);
        _topWall.SetActive(cell.TopWall);
        _botWall.SetActive(cell.BotWall);
    }
}
