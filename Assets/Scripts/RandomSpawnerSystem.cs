using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

public partial struct RandomSpawnerSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var dt = SystemAPI.Time.DeltaTime;

        foreach (var (spawner, xform) in
                 SystemAPI.Query<RefRW<RandomSpawner>,
                                 RefRO<LocalTransform>>())
        {
            // Spawner data as readonly
            var data = spawner.ValueRO;

            // Timer update
            var nt = data.Timer + data.PerSecond * dt;
            var count = (int)nt;
            spawner.ValueRW.Timer = nt - count;

            // Prefab instantiation
            var spawned = state.EntityManager.Instantiate(data.Prefab, count, Allocator.Temp);

            // Position initialization
            for (var i = 0; i < count; i++)
            {
                var p = xform.ValueRO.Position;
                p += spawner.ValueRW.Random.NextFloat3(-data.Extent, data.Extent);
                var cx = SystemAPI.GetComponentRW<LocalTransform>(spawned[i]);
                cx.ValueRW.Position = p;
            }
        }
    }
}
