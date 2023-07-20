using UnityEngine;

public class GameObjectPool : Pool<GameObject>
{
    private PoolConfig _poolConfig;
    public PoolConfig Config => _poolConfig;
    public GameObjectPool(PoolConfig poolConfig) : base(poolConfig)
    {
        _poolConfig = poolConfig;
    }

    protected override GameObject CreatePooledItem()
    {
        return (Object.Instantiate(_poolConfig.Prefab) as GameObject);
    }

    protected override void OnDestroyPoolObject(GameObject obj)
    {
        Object.Destroy(obj);
    }

    protected override void OnReturnedToPool(GameObject obj)
    {
        obj.gameObject.SetActive(false);
    }

    protected override void OnTakeFromPool(GameObject obj)
    {
        obj.gameObject.SetActive(true);
    }
}
