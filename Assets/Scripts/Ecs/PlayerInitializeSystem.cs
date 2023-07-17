using Leopotam.EcsLite;
using UnityEngine;

public sealed class PlayerInitializeSystem : IEcsInitSystem
{
    private PlayerConfig _config;

    public PlayerInitializeSystem(PlayerConfig config)
    {
        _config = config;
    }

    public void Init(IEcsSystems systems)
    {
        var world = systems.GetWorld();
        var entity = world.NewEntity();

        var playerPool = world.GetPool<PlayerComponent>();
        var mazePool = world.GetPool<MazeComponent>();

        var filter = world.Filter<MazeComponent>().End();

        playerPool.Add(entity);

        ref var player = ref playerPool.Get(entity);

        player.Transform = Object.Instantiate(_config.Prefab, Vector3.zero, Quaternion.identity).transform;


        foreach (var i in filter)
        {
            ref var maze = ref mazePool.Get(i);

            Vector2 pos;
            if (_config.SpawnMazeCoord.x >= maze.Maze.ColumnCount || _config.SpawnMazeCoord.y >= maze.Maze.RowCount)
            {
                pos = maze.Maze.GetCellWorldPosition(maze.Maze.ColumnCount - 1, maze.Maze.RowCount - 1);
            }
            else
            {
                pos = maze.Maze.GetCellWorldPosition(_config.SpawnMazeCoord.x, _config.SpawnMazeCoord.y);
            }

            player.Transform.position = new Vector3(pos.x, _config.Ypos, pos.y);
        }
    }
}

public sealed class PlayerMovementSystem : IEcsInitSystem, IEcsRunSystem
{
    private EcsFilter _filter;
    private EcsPool<PlayerComponent> _playerPool;

    public void Init(IEcsSystems systems)
    {
        var world = systems.GetWorld();

        _filter = world.Filter<PlayerComponent>().End();
        _playerPool = world.GetPool<PlayerComponent>();
    }

    public void Run(IEcsSystems systems)
    {
        foreach (var i in _filter)
        {
            var player = _playerPool.Get(i);

            //float step = _timeToMove * Time.deltaTime;

            //var pos = _maze.GetXY(new Vector2(transform.position.x, transform.position.z));
            //var x = pos.x;
            //var z = pos.y;
            //W4Cell cell = _maze.GetCell(x, z);

            //transform.position = Vector3.MoveTowards(transform.position, _movePoint, step);

            //if (transform.position == _movePoint)
            //{
            //    if (hor > 0 && cell.RightWall == false)
            //    {
            //        //StartCoroutine(MoveRoutine(Vector3.right));
            //        Move(Vector3.right);
            //    }

            //    if (hor < 0 && cell.LeftWall == false)
            //    {
            //        //StartCoroutine(MoveRoutine(Vector3.left));
            //        Move(Vector3.left);
            //    }

            //    if (vert > 0 && cell.TopWall == false)
            //    {
            //        //StartCoroutine(MoveRoutine(Vector3.forward));
            //        Move(Vector3.forward);

            //    }

            //    if (vert < 0 && cell.BotWall == false)
            //    {
            //        //StartCoroutine(MoveRoutine(Vector3.back));
            //        Move(Vector3.back);
            //    }

            //    if (_isMoved == false)
            //    {
            //        _isMoved = true;
            //        Moved.Invoke(pos.x, pos.y);
            //    }

            //}

        }
    }
}
