using Leopotam.EcsLite;

public sealed class EndGameSystem : IEcsInitSystem, IEcsRunSystem
{
    private EcsFilter _deathFilter;
    private EcsPool<DeathMarker> _deathMarker;
    private GameContext _context;
    public EndGameSystem(GameContext context)
    {
        _context = context;
    }

    public void Init(IEcsSystems systems)
    {
        var world = systems.GetWorld();
        _deathFilter = world.Filter<PlayerComponent>().Inc<DeathMarker>().End();
        _deathMarker = world.GetPool<DeathMarker>();

    }

    public void Run(IEcsSystems systems)
    {
        foreach (var player in _deathFilter)
        {
            _deathMarker.Del(player);
            _context.FinishGame();
        }
    }
}
