using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

public partial struct SpawnerSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
      => state.RequireForUpdate<Spawner>();

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var data = SystemAPI.GetSingleton<Spawner>();
        var prefab = data.Prefab;
        var instances = state.EntityManager.Instantiate(prefab, data.SpawnCount, Allocator.Temp);

        var rnd = new Random(123);
        foreach (var entity in instances)
        {
            var xform = SystemAPI.GetComponentRW<LocalTransform>(entity);
            xform.ValueRW.Position = rnd.NextFloat3(-5, 5);
        }

        state.Enabled = false;
    }
}
