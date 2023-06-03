using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using Klak.Math;

public partial struct VoxelUpdateSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
      => new VoxelUpdateJob(){dt = SystemAPI.Time.DeltaTime}.Schedule();
}

public partial struct LifetimeSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var ecbSingleton = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>();
        var ecb = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged);

        foreach (var (xform, entity) in
                 SystemAPI.Query<RefRO<LocalTransform>>().WithAll<Voxel>().WithEntityAccess())
        {
            if (xform.ValueRO.Scale < 0.1f) ecb.DestroyEntity(entity);
        }
    }
}

[BurstCompile]
partial struct VoxelUpdateJob : IJobEntity
{
    public float dt;

    void Execute(ref LocalTransform xform, in Voxel voxel)
    {
        // Per-instance random number
        var hash = new XXHash(voxel.ID);
        var rand1 = hash.Float(1);
        var rand2 = hash.Float(2);

        // Move/shrink
        xform = xform.Translate(math.float3(0.1f, -2.0f, 0.3f) * (rand2 + 0.1f) * dt);
        xform = xform.ApplyScale(math.lerp(0.9f, 0.98f, rand1));
    }
}
