using UnityEngine;

public struct PlayerComponent { }

public struct MovementComponent
{
    public Vector2 Direction;
    public float Speed;
    public Vector3 DesiredPosition;
}

public struct WorldObjectComponent
{
    public Transform Transform;
}
