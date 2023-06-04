using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using Klak.Math;

public partial struct VoxelUpdateSystem : ISystem
{
    public void OnUpdate(ref SystemState state)
      => new VoxelUpdateJob(){Delta = SystemAPI.Time.DeltaTime}.ScheduleParallel();
}

[BurstCompile]
partial struct VoxelUpdateJob : IJobEntity
{
    public float Delta;

    void Execute(ref LocalTransform xform, in Voxel voxel)
    {
        // Per-instance random number
        var hash = new XXHash(voxel.ID);
        var rand1 = hash.Float(1);
        var rand2 = hash.Float(2);

        // Move/shrink
        var vel = math.float3(0.1f, -2.0f, 0.3f) * (rand2 + 0.1f);
        xform = xform.Translate(vel * Delta);
        xform = xform.ApplyScale(math.lerp(0.9f, 0.98f, rand1));
    }
}
