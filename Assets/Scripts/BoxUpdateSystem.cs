using Unity.Burst;
using Unity.Entities;
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
            DeltaTime = SystemAPI.Time.DeltaTime };

        job.ScheduleParallel();
    }
}

[BurstCompile(CompileSynchronously = true)]
partial struct BoxUpdateJob : IJobEntity
{
    public EntityCommandBuffer.ParallelWriter Commands;
    public Voxelizer Voxelizer;
    public float DeltaTime;

    void Execute([ChunkIndexInQuery] int index,
                 Entity entity, ref LocalTransform xform, ref Box box)
    {
        box.Time += DeltaTime;

        xform.Position.y -= Voxelizer.Gravity * box.Time;

        var p01 = box.Time / Voxelizer.VoxelLife;
        xform.Scale = Voxelizer.VoxelSize * (1 - p01 * p01 * p01 * p01);

        if (box.Time > Voxelizer.VoxelLife)
            Commands.DestroyEntity(index, entity);
    }
}
