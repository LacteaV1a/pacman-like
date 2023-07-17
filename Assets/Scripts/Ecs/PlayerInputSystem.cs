using Leopotam.EcsLite;
using UnityEngine;

public sealed class PlayerInputSystem : IEcsRunSystem
{
    public void Run(IEcsSystems systems)
    {
        var filter = systems.GetWorld().Filter<PlayerComponent>().End();
        var playerPool = systems.GetWorld().GetPool<PlayerComponent>();

        foreach (var i in filter)
        {
            var yAxis = Input.GetAxis($"Vertical");
            var xAxis = Input.GetAxis($"Horizontal");

            ref var player = ref playerPool.Get(i);
            if (yAxis > 0)
            {
                player.Direction = Vector2.up;
            }
            else if (yAxis < 0)
            {
                player.Direction = Vector2.down;
            }
            else if (xAxis > 0)
            {
                player.Direction = Vector2.right;
            }
            else if (xAxis < 0)
            {
                player.Direction = Vector2.left;
            }
        }
    }
}