using UnityEngine;
using Unity.Entities;
using Unity.Physics;
using Unity.Transforms;
using SphereCollider = Unity.Physics.SphereCollider;

public class CollisionGenerator : MonoBehaviour
{
    void Start()
    {
        var manager = World.DefaultGameObjectInjectionWorld.EntityManager;

        var componentTypes = new ComponentType []
        {
            typeof(LocalTransform),
            typeof(PhysicsCollider),
            typeof(PhysicsWorldIndex)
        };

        var entity = manager.CreateEntity(componentTypes);

        var xform = new LocalTransform
        {
            Position = transform.position,
            Rotation = transform.rotation,
            Scale = 1
        };

        manager.SetComponentData(entity, xform);

        var geo = new SphereGeometry
          { Center = transform.position, Radius = 1 };
        var collider = SphereCollider.Create(geo, CollisionFilter.Default);

        manager.SetComponentData
          (entity, new PhysicsCollider{ Value = collider });

        manager.AddSharedComponentManaged
          (entity, new PhysicsWorldIndex{ Value = 0 });
    }
}
