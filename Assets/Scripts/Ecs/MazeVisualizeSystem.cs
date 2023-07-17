using Leopotam.EcsLite;
using Nox7atra.Mazes;
using Nox7atra.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeVisualizeSystem : IEcsInitSystem
{
    private W4Maze _maze;
    private MazeViewConfig _config;

    public MazeVisualizeSystem(MazeViewConfig config)
    {
        _config = config;
    }

    public void Init(IEcsSystems systems)
    {
        var world = systems.GetWorld();

        var filter = world.Filter<MazeComponent>().End();

        var mazeComp = world.GetPool<MazeComponent>();


        foreach (var enttity in filter)
        {
            ref var component = ref mazeComp.Get(enttity);
            Construct(component.Maze);
        }
    }

    public void Construct(W4Maze maze)
    {
        _maze = maze;
        var floor = CreateFloor(maze.ColumnCount, maze.RowCount, _config.Floor, maze.CellsSize);
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
                var wall = Object.Instantiate(_config.PrefabCell, new Vector3(i * _maze.CellsSize, 0, j * _maze.CellsSize), Quaternion.identity);
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
