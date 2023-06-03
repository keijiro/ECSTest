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

        data.Timer += data.PerSecond * SystemAPI.Time.DeltaTime;

        var count = (int)data.Timer;
        data.Timer -= count;

        data.Seed++;

        SystemAPI.SetSingleton<Spawner>(data);

        if (count == 0) return;

        var prefab = data.Prefab;
        var instances = state.EntityManager.Instantiate(prefab, count, Allocator.Temp);

        var rnd = new Random(data.Seed);
        rnd.NextFloat4();

        foreach (var entity in instances)
        {
            var xform = SystemAPI.GetComponentRW<LocalTransform>(entity);
            xform.ValueRW.Position = rnd.NextFloat3(-5, 5);
        }
    }
}
