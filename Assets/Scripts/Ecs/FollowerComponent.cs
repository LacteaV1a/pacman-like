using Nox7atra.Mazes;
using System.Collections.Generic;
using UnityEngine;

public struct FollowerComponent 
{
    public Vector2Int Target;
    public int CurrentPathIndex;
    public List<Vector2Int> ShortestPathDirection;

}
