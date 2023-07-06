using Nox7atra.Mazes;
using Nox7atra.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeVisualizer : MonoBehaviour
{
    [SerializeField] private W4CellView _w4CellPrefab;
    [SerializeField] private Material _floorMaterial;
    private W4Maze _maze;

    public void Construct(W4Maze maze)
    {
        _maze = maze;
        var floor = CreateFloor(maze.ColumnCount, maze.RowCount, _floorMaterial, maze.CellsSize);
        floor.transform.position += new Vector3(maze.ColumnCount * 0.5f * maze.CellsSize, 0, maze.RowCount * 0.5f * maze.CellsSize);

        CreateWalls();
    }

    private GameObject CreateWalls()
    {
        var wallsGO = new GameObject();
        for (int i = 0; i < _maze.ColumnCount; i++)
        {
            for (int j = 0; j < _maze.RowCount; j++)
            {
                var cell = _maze.GetCell(i, j);
                var wall = Instantiate(_w4CellPrefab, new Vector3(i * _maze.CellsSize, 0, j * _maze.CellsSize), Quaternion.identity);
                wall.Init(cell);
                wall.transform.SetParent(wallsGO.transform);
                var wallScale = wall.transform.localScale;
                wallScale *= _maze.CellsSize;
                wall.transform.localScale = wallScale;
            }

        }
        return wallsGO;
    }

    private GameObject CreateFloor(
    float width,
    float depth,
    Material mat,
    float scale)
    {
        var name = "floor" + width.ToString() + "x" + depth.ToString();
        var floor = MeshTools.CreateMeshObj(name, mat);
        width *= scale;
        depth *= scale;

        floor.sharedMesh = MeshTools.CreatePlaneMesh(
            width,
            depth,
            2,
            2,
            name,
            false,
            string.Empty,
            Mathf.Max(width, depth)
        );
        floor.transform.rotation = Quaternion.Euler(90, 0, 0);
        return floor.gameObject;
    }
}
