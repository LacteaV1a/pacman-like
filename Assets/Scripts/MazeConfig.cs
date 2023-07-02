using UnityEngine;

[CreateAssetMenu(fileName = "MazeConfig", menuName = "SO/MazeConfig")]
public class MazeConfig : ScriptableObject
{
    [field: SerializeField] public int CellsX { get; private set; } = 10;
    [field: SerializeField] public int CellsY { get; private set; } = 10;
    [field: SerializeField] public int CellsSize { get; private set; } = 1;
    [field: SerializeField] public int Seed { get; private set; } = 0;
    [field: SerializeField, Range(0,1)] public float WallOccupancy { get; private set; } = 0.9f;
}
