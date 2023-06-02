using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;

public partial struct RotationSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var dt = SystemAPI.Time.DeltaTime;
        foreach (var rot in SystemAPI.Query<RotationAspect>()) rot.Rotate(dt);
    }
}

readonly partial struct RotationAspect : IAspect
{
    readonly RefRW<LocalTransform> _xform;
    readonly RefRO<RotationSpeed> _speed;

    public void Rotate(float dt)
      => _xform.ValueRW = _xform.ValueRO.RotateY(_speed.ValueRO.Speed * dt);
}
