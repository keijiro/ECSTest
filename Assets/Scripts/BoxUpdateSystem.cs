using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;

public partial struct BoxUpdateSystem : ISystem
{
    EntityCommandBuffer.ParallelWriter NewCommandWriter(in SystemState state)
      => SystemAPI.GetSingleton
         <BeginSimulationEntityCommandBufferSystem.Singleton>()
           .CreateCommandBuffer(state.WorldUnmanaged)
           .AsParallelWriter();

    public void OnUpdate(ref SystemState state)
      => new BoxUpdateJob()
         {
             Commands = NewCommandWriter(state),
             DeltaTime = SystemAPI.Time.DeltaTime,
             Scale = SystemAPI.GetSingleton<Voxelizer>().VoxelSize
         }
         .ScheduleParallel();
}

[BurstCompile]
partial struct BoxUpdateJob : IJobEntity
{
    public EntityCommandBuffer.ParallelWriter Commands;
    public float DeltaTime;
    public float Scale;

    void Execute([ChunkIndexInQuery] int index,
                 Entity entity, ref LocalTransform xform, ref Box box)
    {
        box.Time += DeltaTime;
        if (box.Time > 0.2f)
            Commands.DestroyEntity(index, entity);
        else
        {
            xform.Position.y -= box.Time * 0.1f;
            xform.Scale = Scale;
        }
    }
}
