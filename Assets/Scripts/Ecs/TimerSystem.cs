using Leopotam.EcsLite;
using UnityEngine;

public sealed class TimerSystem : IEcsInitSystem, IEcsRunSystem
{
    private EcsFilter _timerFilter;
    private EcsPool<TimerComponent> _timerPool;

    public void Init(IEcsSystems systems)
    {
        var world = systems.GetWorld();
        _timerFilter = world.Filter<TimerComponent>().End();
        _timerPool = world.GetPool<TimerComponent>();
    }

    public void Run(IEcsSystems systems)
    {
        foreach (var entity in _timerFilter)
        {
            ref var timer = ref _timerPool.Get(entity);
            if (timer.LeftTimeSec <= 0) continue;
            timer.LeftTimeSec -= Time.deltaTime;
            if (timer.LeftTimeSec < 0) timer.LeftTimeSec = 0;
        }
    }
}
