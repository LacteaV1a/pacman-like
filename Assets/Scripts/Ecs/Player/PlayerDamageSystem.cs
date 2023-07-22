using Leopotam.EcsLite;

public sealed class PlayerDamageSystem : IEcsInitSystem, IEcsRunSystem
{
    private EcsFilter _playerFilter;
    private EcsFilter _enemyFilter;
    private EcsPool<MazeCoordComponent> _mazeCoordPool;
    private EcsPool<DeathMarker> _deathPool;

    public void Init(IEcsSystems systems)
    {
        var world = systems.GetWorld();

        _playerFilter = world.Filter<PlayerComponent>().Inc<MazeCoordComponent>().End();
        _enemyFilter = world.Filter<EnemyComponent>().Inc<MazeCoordComponent>().End();

        _mazeCoordPool = world.GetPool<MazeCoordComponent>();
        _deathPool = world.GetPool<DeathMarker>();
    }

    public void Run(IEcsSystems systems)
    {
        foreach (var playerEntity in _playerFilter)
        {
            ref var player = ref _mazeCoordPool.Get(playerEntity);

            foreach (var enemyEntity in _enemyFilter)
            {
                ref var enemy = ref _mazeCoordPool.Get(enemyEntity);

                if(player.Value == enemy.Value)
                {
                    if(_deathPool.Has(playerEntity) == false)
                    {
                        _deathPool.Add(playerEntity);
                    }
                }
            }
        }

    }
}
