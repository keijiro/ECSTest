using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Physics;

public partial struct CustomColliderSystem : ISystem
{
    public void OnUpdate(ref SystemState state)
    {
        foreach (var (data, xform) in
                 SystemAPI.Query<RefRW<CustomCollider>,
                                 RefRO<LocalTransform>>())
        {
            if (data.ValueRO.Counter == 0)
            {
                var componentTypes = new ComponentType []
                {
                    typeof(LocalTransform),
                    typeof(PhysicsCollider),
                    typeof(PhysicsWorldIndex)
                };

                var entity = state.EntityManager.CreateEntity(componentTypes);

                state.EntityManager.SetComponentData(entity, xform.ValueRO);


                var geo = new SphereGeometry
                  { Center = xform.ValueRO.Position, Radius = 1 };
                var collider = SphereCollider.Create(geo, CollisionFilter.Default);

                state.EntityManager.SetComponentData
                  (entity, new PhysicsCollider{ Value = collider });

                state.EntityManager.AddSharedComponentManaged
                  (entity, new PhysicsWorldIndex{ Value = 0 });
            }

            data.ValueRW.Counter++;
        }
    }
}
