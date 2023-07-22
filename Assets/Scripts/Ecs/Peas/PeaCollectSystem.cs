using Leopotam.EcsLite;
using UnityEngine;

public sealed class PeaCollectSystem : IEcsInitSystem, IEcsRunSystem
{
    private GameObjectPool _peaViewPool;
    private EcsFilter _peaFilter;
    private EcsFilter _timerPeaPool;
    private EcsPool<PeaComponent> _peaPool;
    private EcsPool<GrabedMarker> _grabedPool;
    private EcsPool<MazePlaceholderComponent> _placeholderPool;
    private EcsPool<TimerComponent> _timerPool;
    private EcsPool<CanGrabComponent> _canGrabPool;
    private EcsPool<WorldObjectComponent> _worldObjPool;
    private PeaConfig _peaConfig;

    public PeaCollectSystem(PeaConfig peaConfig)
    {
        _peaConfig = peaConfig;
    }

    public void Init(IEcsSystems systems)
    {
        var world = systems.GetWorld();
        _peaFilter = world.Filter<PeaComponent>().Inc<GrabedMarker>().End();
        _timerPeaPool = world.Filter<PeaComponent>().Inc<TimerComponent>().End();
        var peaViewPoolFilter = world.Filter<PeaViewPoolComponent>().End();
        _peaPool = world.GetPool<PeaComponent>();
        _grabedPool = world.GetPool<GrabedMarker>();
        _placeholderPool = world.GetPool<MazePlaceholderComponent>();
        _timerPool = world.GetPool<TimerComponent>();
        _canGrabPool = world.GetPool<CanGrabComponent>();
        _worldObjPool = world.GetPool<WorldObjectComponent>();

        foreach (var item in peaViewPoolFilter)
        {
            ref var peaViewPool = ref world.GetComponent<PeaViewPoolComponent>(item);
            _peaViewPool = peaViewPool.PeaViewPool;
        }
    }

    public void Run(IEcsSystems systems)
    {
        foreach (var item in _timerPeaPool)
        {
            ref var timer = ref _timerPool.Get(item);
            if (timer.LeftTimeSec == 0)
            {
                _timerPool.Del(item);

                ref var placeholder = ref _placeholderPool.Get(item);
                placeholder.CanSetPlace = true;

                ref var wo = ref _worldObjPool.Get(item);
                wo.Transform = _peaViewPool.Get().transform;

                ref var canGrab = ref _canGrabPool.Get(item);
                canGrab.Value = true;

            }
        }

        foreach (var item in _peaFilter)
        {
            ref var pea = ref _worldObjPool.Get(item);

            if (_timerPool.Has(item)) continue;
            _peaViewPool.Release(pea.Transform.gameObject);
            _grabedPool.Del(item);

            ref var timer = ref _timerPool.Add(item);
            timer.LeftTimeSec = _peaConfig.TimeSpawnSec;

        }
    }
}
