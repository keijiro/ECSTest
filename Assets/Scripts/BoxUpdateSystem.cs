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
             DeltaTime = SystemAPI.Time.DeltaTime
         }
         .ScheduleParallel();
}

[BurstCompile]
partial struct BoxUpdateJob : IJobEntity
{
    public EntityCommandBuffer.ParallelWriter Commands;
    public float DeltaTime;

    void Execute([ChunkIndexInQuery] int index,
                 Entity entity, ref LocalTransform xform, ref Box box)
    {
        box.Lifetime -= DeltaTime;
        if (box.Lifetime <= 0)
            Commands.DestroyEntity(index, entity);
        else
            xform.Scale = math.min(1, box.Lifetime * 10);
    }
}
