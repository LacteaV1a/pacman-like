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
            player.Direction = new Vector2(xAxis, yAxis);
        }
    }
}