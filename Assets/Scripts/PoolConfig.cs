using UnityEngine;

[CreateAssetMenu(fileName = "PoolConfig", menuName = "SO/PoolConfig")]
public class PoolConfig : ScriptableObject
{
    [field: SerializeField] public Object Prefab { get; private set; }
    [field: SerializeField] public int DefaultCapacity { get; private set; } = 10;
    [field: SerializeField] public int MaxInPool { get; private set; } = 100;
}
