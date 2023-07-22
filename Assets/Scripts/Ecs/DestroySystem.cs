using Leopotam.EcsLite;
using UnityEngine;

public sealed class DestroySystem : IEcsDestroySystem
{
    public void Destroy(IEcsSystems systems)
    {
        var world = systems.GetWorld();
        var worldObjFilter = world.Filter<WorldObjectComponent>().End();
        var worldobjPool = world.GetPool<WorldObjectComponent>();

        foreach (var entity in worldObjFilter)
        {
            ref var worldObj = ref worldobjPool.Get(entity);
            if(worldObj.Transform != null)
                Object.Destroy(worldObj.Transform.gameObject);
        }

    }
}
