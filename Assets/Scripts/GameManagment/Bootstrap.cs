using Nox7atra.Mazes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bootstrap : MonoBehaviour
{
    [SerializeField] private GameUI _gameUI;
    [SerializeField] private GameContext _gameContext;
    [SerializeField] private MazeVisualizer _mazeVisualizer;
    [SerializeField] private LoseConditionSystem _loseConditionSystem;
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

        _gameUI.SetStartButton(() => _gameContext.StartGame());
    }

    private void SetupSystem(W4Maze maze, MazePlayer mazePlayer)
    {
        _peasSystem.Initialize(mazePlayer, maze);
        _enemySystem.Initialize(mazePlayer, maze);
        _loseConditionSystem.Initialize(mazePlayer, maze, _enemySystem, _gameContext);

        _gameContext.AddListener(_peasSystem);
        _gameContext.AddListener(_enemySystem);
        _gameContext.AddListener(mazePlayer);
        _gameContext.AddListener(_gameUI);
        _gameContext.AddListener(_loseConditionSystem);
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
