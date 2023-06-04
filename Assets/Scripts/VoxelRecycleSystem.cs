using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;

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

