using UnityEngine;

[CreateAssetMenu(fileName = "MazeViewConfig", menuName = "SO/MazeViewConfig")]
public class MazeViewConfig : ScriptableObject
{
    [field: SerializeField] public W4CellView PrefabCell { get; private set; }
    [field: SerializeField] public Material Floor { get; private set; }
}
