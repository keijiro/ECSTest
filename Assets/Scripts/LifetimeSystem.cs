using Unity.Burst;
using Unity.Entities;

public partial struct LifetimeSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var ecbSingleton = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>();
        var ecb = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged);
        var dt = SystemAPI.Time.DeltaTime;

        foreach (var (lifetime, entity) in
                 SystemAPI.Query<RefRW<Lifetime>>().WithEntityAccess())
        {
            lifetime.ValueRW.Life -= dt;
            if (lifetime.ValueRO.Life <= 0) ecb.DestroyEntity(entity);
        }
    }
}
