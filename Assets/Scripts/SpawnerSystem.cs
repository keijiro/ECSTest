using UnityEngine;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

public partial struct SpawnerSystem : ISystem
{
    public void OnUpdate(ref SystemState state)
    {
        foreach (var (xform, spawner) in
                 SystemAPI.Query<RefRO<LocalTransform>, RefRO<Spawner>>())
            RunSpawner(ref state, xform.ValueRO, spawner.ValueRO);
    }

    void RunSpawner
      (ref SystemState state, in LocalTransform xform, in Spawner spawner)
    {
        // Total count of rays
        var total = spawner.Resolution.x * spawner.Resolution.y;

        // Raycast struct array
        using var rays = new NativeArray<RaycastCommand>(total, Allocator.TempJob);
        using var hits = new NativeArray<RaycastHit>(total, Allocator.TempJob);

        // Raycast setup
        {
            var slice = new NativeSlice<RaycastCommand>(rays);
            var dir = math.float3(0, 0, 1);
            var dist = spawner.Extent.z * 2;
            var idx = 0;

            for (var iy = 0; iy < spawner.Resolution.y; iy++)
            {
                for (var ix = 0; ix < spawner.Resolution.x; ix++)
                {
                    var u = (float)ix / (spawner.Resolution.x - 1);
                    var v = (float)iy / (spawner.Resolution.y - 1);

                    var x = math.lerp(-spawner.Extent.x, spawner.Extent.x, u);
                    var y = math.lerp(-spawner.Extent.y, spawner.Extent.y, v);

                    var p = xform.Position + math.float3(x, y, -spawner.Extent.z);

                    slice[idx++] = new RaycastCommand(p, dir, QueryParameters.Default, dist);
                }
            }
        }

        // Raycast batch
        RaycastCommand.ScheduleBatch(rays, hits, 16).Complete();

        // Hit count
        var count = 0;
        for (var i = 0; i < total; i++)
            if (hits[i].collider != null) count++;

        // Mass instantiation
        {
            var prefab = spawner.Prefab;
            var instances = state.EntityManager.Instantiate(prefab, count, Allocator.Temp);

            var idx = 0;
            for (var i = 0; i < total; i++)
            {
                if (hits[i].collider == null) continue;
                var xformc = SystemAPI.GetComponentRW<LocalTransform>(instances[idx++]);
                xformc.ValueRW.Position = hits[i].point;
            }
        }
    }
}
