using Nox7atra.Mazes;
using UnityEngine;

public class EnemySystem : MonoBehaviour
{
    [SerializeField] private PoolConfig _poolConfig;
    private MazePlayer _player;
    private W4Maze _maze;
    private EnemyPool _enemyPool;
    private MazePathFinder _pathFinder;
    private bool _isActive;
    private bool _isInitialized;
    
    public void Initialize(MazePlayer player, W4Maze maze)
    {
        _player = player;
        _maze = maze;
        _pathFinder = new MazePathFinder();
        _enemyPool = new EnemyPool(player, _pathFinder, new MazeGraph(maze,false), _poolConfig);

        _isInitialized = true;
    }

    public void StartSystem()
    {
        for (int i = 0; i < _poolConfig.DefaultCapacity; i++)
        {
            var enemy = _enemyPool.Get();
            var coord = GetRandomCoord();
            var pos = _maze.GetCellWorldPosition(coord.x, coord.y);
            enemy.SetPostion(pos.x, pos.y);
        }
    }

    private Vector2Int GetRandomCoord()
    {
        var rand = new System.Random();
        var x = rand.Next(0, _maze.ColumnCount - 1);
        var y = rand.Next(0, _maze.RowCount - 1);

        return new Vector2Int(x, y);
    }
}