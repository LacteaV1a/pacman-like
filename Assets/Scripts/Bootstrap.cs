using Nox7atra.Mazes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bootstrap : MonoBehaviour
{
    [SerializeField] private MazeVisualizer _mazeVisualizer;
    [SerializeField] private PeasSystem _peasSystem;
    [SerializeField] private EnemySystem _enemySystem;
    [SerializeField] private MazePlayer _player;
    [SerializeField] private MazeConfig _config;

    private void Awake()
    {
        Initialize();
    }

    public void Initialize()
    {
        var maze = SetupMaze();

        var player = SetupMazePlayer(maze);
        SetupSystem(maze, player);

        VisualizeMaze(maze);

        StartGame();
    }

    public void StartGame()
    {
        _peasSystem.StartSystem();
        _enemySystem.StartSystem();
    }

    private void SetupSystem(W4Maze maze, MazePlayer mazePlayer)
    {
        _peasSystem.Initialize(mazePlayer, maze);
        _enemySystem.Initialize(mazePlayer, maze);
    }


    private W4Maze SetupMaze()
    {
        var mazeCreator = new MazeCreator(_config, new CustomEllerGenerator(_config.WallOccupancy));
        return mazeCreator.Create();
    }

    private MazePlayer SetupMazePlayer(W4Maze maze)
    {
        var player = Instantiate(_player);
        player.Construct(maze);
        return player;
    }

    public void VisualizeMaze(W4Maze maze)
    {
        _mazeVisualizer.Construct(maze);
    }
}
