using Leopotam.EcsLite;
using Nox7atra.Mazes;
using UnityEngine;
public sealed class MovementInMazeSystem : IEcsInitSystem, IEcsRunSystem
{
    private EcsFilter _filter;
    private EcsPool<MovementComponent> _movementPool;
    private EcsPool<WorldObjectComponent> _worldObjPool;
    private EcsPool<MovedMarker> _movedPool;
    private EcsPool<MazeCoordComponent> _mazeCoord;
    private W4Maze _maze;

    public void Init(IEcsSystems systems)
    {
        var world = systems.GetWorld();

        _filter = world.Filter<MovementComponent>().Inc<WorldObjectComponent>().Inc<MazeCoordComponent>().End();

        _movementPool = world.GetPool<MovementComponent>();
        _worldObjPool = world.GetPool<WorldObjectComponent>();
        _movedPool = world.GetPool<MovedMarker>();
        _mazeCoord = world.GetPool<MazeCoordComponent>();

        var mazeFilter = world.Filter<MazeComponent>().End();
        var mazePool = world.GetPool<MazeComponent>();

        foreach (var item in _filter)
        {
            ref var moveComp = ref _movementPool.Get(item);
            moveComp.DesiredPosition = _worldObjPool.Get(item).Transform.position;
        }

        foreach (var item in mazeFilter)
        {
            ref var comp = ref mazePool.Get(item);
            _maze = comp.Maze;
        }
    }

    public void Run(IEcsSystems systems)
    {
        foreach (var entity in _filter)
        {
            var movement = _movementPool.Get(entity);
            var worldObj = _worldObjPool.Get(entity);

            float step = movement.Speed * Time.deltaTime;

            var pos = _maze.GetXY(new Vector2(worldObj.Transform.position.x, worldObj.Transform.position.z));
            var x = pos.x;
            var z = pos.y;
            W4Cell cell = _maze.GetCell(x, z);

            worldObj.Transform.position = Vector3.MoveTowards(worldObj.Transform.position, movement.DesiredPosition, step);

            if (worldObj.Transform.position == movement.DesiredPosition)
            {

                if (_movedPool.Has(entity)== false)
                {
                    _movedPool.Add(entity);
                    ref var coord = ref _mazeCoord.Get(entity);
                    coord.Value = _maze.GetXY(new Vector2(movement.DesiredPosition.x, movement.DesiredPosition.z));
                }

                if (movement.Direction.x > 0 && cell.RightWall == false)
                {
                    Move(entity, worldObj.Transform, Vector3.right);
                }

                if (movement.Direction.x < 0 && cell.LeftWall == false)
                {
                    Move(entity, worldObj.Transform, Vector3.left);
                }

                if (movement.Direction.y > 0 && cell.TopWall == false)
                {
                    Move(entity, worldObj.Transform, Vector3.forward);
                }

                if (movement.Direction.y < 0 && cell.BotWall == false)
                {
                    Move(entity, worldObj.Transform, Vector3.back);
                }
            }
            else
            {
                if(_movedPool.Has(entity))
                    _movedPool.Del(entity);
            }
        }
    }

    private void Move(int entity, Transform transform, Vector3 dir)
    {
        ref var moveComp = ref _movementPool.Get(entity);
        moveComp.DesiredPosition = transform.position + dir * _maze.CellsSize;
    }
}


