using UnityEngine;

[CreateAssetMenu(fileName = "PlayerConfig", menuName = "SO/PlayerConfig")]
public class PlayerConfig : ScriptableObject
{
    [field: SerializeField] public GameObject Prefab { get; private set; }
    [field: SerializeField, Min(0)] public Vector2Int SpawnMazeCoord { get; private set; }
    [field: SerializeField, Min(0)] public float Speed { get; private set; }
    [field: SerializeField, Min(0)] public float Ypos { get; private set; }

}