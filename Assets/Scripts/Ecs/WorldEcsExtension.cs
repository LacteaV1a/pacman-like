using Leopotam.EcsLite;

public static class WorldEcsExtension
{
    public static ref T GetComponent<T>(this EcsWorld world, int entity) where T : struct
    {
       var pool = world.GetPool<T>();
       return ref pool.Get(entity);
    }

    public static ref T AddComponentToEntity<T>(this EcsWorld world, int entity) where T : struct
    {
        var pool = world.GetPool<T>();
        pool.Add(entity);
        return ref pool.Get(entity);
    }
}


