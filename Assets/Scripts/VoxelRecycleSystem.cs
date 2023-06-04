using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;

public partial struct LifetimeSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
      => new VoxelRecycleJob()
           { Commands = SystemAPI.GetSingleton
               <BeginSimulationEntityCommandBufferSystem.Singleton>()
               .CreateCommandBuffer(state.WorldUnmanaged).AsParallelWriter() }.ScheduleParallel();
}

[BurstCompile]
partial struct VoxelRecycleJob : IJobEntity
{
    public EntityCommandBuffer.ParallelWriter Commands;

    void Execute([ChunkIndexInQuery] int chunkIndexInQuery,Entity entity, in LocalTransform xform, in Voxel voxel)
    {
        if (xform.Scale < 0.1f) Commands.DestroyEntity(chunkIndexInQuery, entity);
    }
}
