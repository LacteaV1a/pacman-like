using UnityEngine;

public class PeaPool : Pool<Pea>
{
    private PoolConfig _poolConfig;
    public PeaPool(PoolConfig poolConfig) : base(poolConfig)
    {
        _poolConfig = poolConfig;
    }

    protected override Pea CreatePooledItem()
    {
        return (Object.Instantiate(_poolConfig.Prefab) as GameObject).GetComponent<Pea>();
    }

    protected override void OnDestroyPoolObject(Pea obj)
    {
        Object.Destroy(obj);
    }

    protected override void OnReturnedToPool(Pea obj)
    {
        obj.gameObject.SetActive(false);
    }

    protected override void OnTakeFromPool(Pea obj)
    {
        obj.gameObject.SetActive(true);
    }
}
