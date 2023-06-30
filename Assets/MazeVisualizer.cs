using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeVisualizer : MonoBehaviour
{
    [SerializeField] private GameObject _verticalWallPrefab;

    [SerializeField] private GameObject _horizontalWallPrefab;

    public void CreateVerticalWalls(bool[,] verticalWalls, int rows, int cols)
    {
        CreateWalls(_verticalWallPrefab, verticalWalls, rows, cols);
    }

    public void CreateWalls(GameObject prefab, bool[,] walls, int rows, int cols )
    {
        for (int i = 0; i < cols; i++)
        {
            for (int j = 0; j < rows; j++)
            {
                if (walls[j, i])
                {
                    Instantiate(prefab, new Vector3(i, 0, j), Quaternion.identity);
                }
            }
        }
    }

    public void CreateHorizontalWalls(bool[,] verticalWalls, int rows, int cols)
    {
        CreateWalls(_horizontalWallPrefab, verticalWalls, rows, cols);
    }
}
