using Nox7atra.Mazes;
using UnityEngine;

public class LoseConditionSystem : GameSystem
{
    private bool _isActive;

    private MazePlayer _player;
    private W4Maze _maze;
    private EnemySystem _enemySystem;
    private GameContext _gameContext;

    public void Initialize(MazePlayer player, W4Maze maze, EnemySystem enemySystem, GameContext gameContext)
    {
        _player = player;
        _maze = maze;
        _enemySystem = enemySystem;
        _gameContext = gameContext;
    }

    private void Update()
    {
        if (_isActive == false) return;
        foreach (var enemy in _enemySystem.Enemys)
        {
            var enemyPos = _maze.GetXY(new Vector2(enemy.transform.position.x, enemy.transform.position.z));
            if (enemyPos == _player.GetCoord())
            {
                _gameContext.FinishGame();
                _isActive = false;
                break;
            }
        }
    }

    public override void OnFinishGame()
    {
        _isActive = false;
    }

    public override void OnStartGame()
    {
        _isActive = true;
    }
}