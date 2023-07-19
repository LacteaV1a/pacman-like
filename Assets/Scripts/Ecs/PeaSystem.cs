using Leopotam.EcsLite;
using UnityEngine;

public sealed class PeaSystem : IEcsInitSystem, IEcsRunSystem
{
    private PeaPool _peaViewPool;
    private EcsFilter _peaFilter;
    private EcsPool<PeaComponent> _peaPool;
    private EcsPool<GrabedMarker> _grabedPool;

    public void Init(IEcsSystems systems)
    {
        var world = systems.GetWorld();
        _peaFilter = world.Filter<PeaComponent>().Inc<GrabedMarker>().End();
        var peaViewPoolFilter = world.Filter<PeaViewPoolComponent>().End();
        _peaPool = world.GetPool<PeaComponent>();
        _grabedPool = world.GetPool<GrabedMarker>();

        foreach (var item in peaViewPoolFilter)
        {
            ref var peaViewPool = ref world.GetComponent<PeaViewPoolComponent>(item);
            _peaViewPool = peaViewPool.PeaViewPool;
        }
    }

    public void Run(IEcsSystems systems)
    {
        _peaFilter = systems.GetWorld().Filter<PeaComponent>().Inc<GrabedMarker>().End();
        foreach (var item in _peaFilter)
        {
            ref var pea = ref _peaPool.Get(item);
            _grabedPool.Del(item);
            //Debug.Log(item);
            //_peaViewPool.Release(pea.View);
        }
        //Debug.Log("END");

    }
}
