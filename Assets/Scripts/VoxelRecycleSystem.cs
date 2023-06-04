using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;

public partial struct LifetimeSystem : ISystem
{
    EntityCommandBuffer.ParallelWriter NewCommandBuffer(in SystemState state)
      => SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>()
           .CreateCommandBuffer(state.WorldUnmanaged).AsParallelWriter();

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
      => new VoxelRecycleJob(){CB = NewCommandBuffer(state)}.ScheduleParallel();
}

[BurstCompile]
partial struct VoxelRecycleJob : IJobEntity
{
    public EntityCommandBuffer.ParallelWriter CB;

    void Execute([ChunkIndexInQuery] int index,
                 Entity entity, in LocalTransform xform, in Voxel voxel)
    {
        if (xform.Scale < 0.1f) CB.DestroyEntity(index, entity);
    }
}
