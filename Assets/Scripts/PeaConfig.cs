using UnityEngine;

[CreateAssetMenu(fileName = "PeaConfig", menuName = "SO/PeaConfig")]
public class PeaConfig : ScriptableObject
{
    [field: SerializeField] public int TimeSpawnSec { get; private set; } = 2;
}
