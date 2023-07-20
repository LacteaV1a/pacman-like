using UnityEngine.Pool;

public abstract class Pool<T> where T : class
{
    private ObjectPool<T> _objectPool;

    public int CountAll => _objectPool.CountAll;
    public int CountActive => _objectPool.CountActive;
    public int CountInactive => _objectPool.CountInactive;

    public Pool(PoolConfig config)
    {
        _objectPool = new ObjectPool<T>(CreatePooledItem, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject, false, config.DefaultCapacity, config.MaxInPool);
    }

    protected abstract T CreatePooledItem();

    protected abstract void OnReturnedToPool(T obj);

    protected abstract void OnTakeFromPool(T obj);

    protected abstract void OnDestroyPoolObject(T obj);

    public  T Get()
    {
        return _objectPool.Get();
    }
    public void Release(T item)
    {
        _objectPool.Release(item);
    }

    public void Dispose()
    {
        _objectPool.Dispose();
    }
}
