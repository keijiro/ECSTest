using UnityEngine;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;

public partial struct BoxUpdateSystem : ISystem
{
    public void OnUpdate(ref SystemState state)
    {
        if (!SystemAPI.HasSingleton<Voxelizer>()) return;

        var writer = 
          SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>()
          .CreateCommandBuffer(state.WorldUnmanaged).AsParallelWriter();

        var job = new BoxUpdateJob()
          { Commands = writer,
            Voxelizer = SystemAPI.GetSingleton<Voxelizer>(),
            Time = (float)SystemAPI.Time.ElapsedTime,
            DeltaTime = SystemAPI.Time.DeltaTime };

        job.ScheduleParallel();
    }
}

[BurstCompile(CompileSynchronously = true)]
partial struct BoxUpdateJob : IJobEntity
{
    public EntityCommandBuffer.ParallelWriter Commands;
    public Voxelizer Voxelizer;
    public float Time;
    public float DeltaTime;

    void Execute([ChunkIndexInQuery] int index,
                 Entity entity,
                 ref LocalTransform xform,
                 ref Box box,
                 ref URPMaterialPropertyBaseColor color)
    {
        box.Time += DeltaTime;
        var p01 = box.Time / Voxelizer.VoxelLife;

        box.Velocity -= Voxelizer.Gravity * DeltaTime;
        xform.Position.y += box.Velocity * DeltaTime;
        if (xform.Position.y < 0)
        {
            box.Velocity *= -1;
            xform.Position.y = -xform.Position.y;
        }

        var p01_ex = p01 * p01;
        xform.Scale = Voxelizer.VoxelSize * (1 - p01_ex * p01_ex * p01_ex);

        if (box.Time > Voxelizer.VoxelLife)
            Commands.DestroyEntity(index, entity);

        var hue = xform.Position.z * Voxelizer.ColorFrequency;
        hue = math.frac(hue + Time * Voxelizer.ColorSpeed);
        color.Value = (Vector4)Color.HSVToRGB(hue, 1, 1);
    }
}
