using Leopotam.EcsLite;
using Nox7atra.Mazes;
using UnityEngine;
public sealed class PlayerMovementInMazeSystem : IEcsInitSystem, IEcsRunSystem
{
    private EcsFilter _filter;
    private EcsPool<PlayerComponent> _playerPool;
    private EcsPool<WorldObjectComponent> _worldObjPool;
    private EcsPool<MovedMarker> _movedPool;
    private EcsPool<MazeCoordComponent> _mazeCoord;
    private W4Maze _maze;
    private Vector3 _movePoint;
    private bool _isMoved;

    public void Init(IEcsSystems systems)
    {
        var world = systems.GetWorld();

        _filter = world.Filter<PlayerComponent>().Inc<WorldObjectComponent>().Inc<MazeCoordComponent>().End();

        _playerPool = world.GetPool<PlayerComponent>();
        _worldObjPool = world.GetPool<WorldObjectComponent>();
        _movedPool = world.GetPool<MovedMarker>();
        _mazeCoord = world.GetPool<MazeCoordComponent>();

        var mazeFilter = world.Filter<MazeComponent>().End();
        var mazePool = world.GetPool<MazeComponent>();

        foreach (var item in _filter)
        {
           _movePoint = _worldObjPool.Get(item).Transform.position;
        }

        foreach (var item in mazeFilter)
        {
            ref var comp = ref mazePool.Get(item);
            _maze = comp.Maze;
        }
    }

    public void Run(IEcsSystems systems)
    {
        foreach (var i in _filter)
        {
            var player = _playerPool.Get(i);
            var worldObj = _worldObjPool.Get(i);

            float step = player.Speed * Time.deltaTime;

            var pos = _maze.GetXY(new Vector2(worldObj.Transform.position.x, worldObj.Transform.position.z));
            var x = pos.x;
            var z = pos.y;
            W4Cell cell = _maze.GetCell(x, z);

            worldObj.Transform.position = Vector3.MoveTowards(worldObj.Transform.position, _movePoint, step);

            if (worldObj.Transform.position == _movePoint)
            {

                if (_isMoved == false)
                {
                    _isMoved = true;
                    _movedPool.Add(i);
                    ref var coord = ref _mazeCoord.Get(i);
                    coord.Value = pos;
                }


                if (player.Direction.x > 0 && cell.RightWall == false)
                {
                    Move(worldObj.Transform, Vector3.right);
                    _movedPool.Del(i);
                }

                if (player.Direction.x < 0 && cell.LeftWall == false)
                {
                    Move(worldObj.Transform, Vector3.left);
                    _movedPool.Del(i);
                }

                if (player.Direction.y > 0 && cell.TopWall == false)
                {
                    Move(worldObj.Transform, Vector3.forward);
                    _movedPool.Del(i);
                }

                if (player.Direction.y < 0 && cell.BotWall == false)
                {
                    Move(worldObj.Transform, Vector3.back);
                    _movedPool.Del(i);
                }


            }
        }
    }

    private void Move(Transform transform, Vector3 dir)
    {
        _movePoint = transform.position + dir * _maze.CellsSize;
        _isMoved = false;

    }
}


