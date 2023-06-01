using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;

public partial struct RotationSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
      => state.RequireForUpdate<Execute.MainThread>();

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var dt = SystemAPI.Time.DeltaTime;

        foreach (var (xform, speed) in SystemAPI.Query<RefRW<LocalTransform>, RefRO<RotationSpeed>>())
            xform.ValueRW = xform.ValueRO.RotateY(speed.ValueRO.Speed * dt);
    }
}
