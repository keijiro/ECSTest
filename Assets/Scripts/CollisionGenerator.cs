using UnityEngine;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
using MeshCollider = Unity.Physics.MeshCollider;

public class CollisionGenerator : MonoBehaviour
{
    [SerializeField] Mesh _mesh = null;

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
            Scale = transform.localScale.x
        };

        manager.SetComponentData(entity, xform);

        using var vtx = new NativeArray<Vector3>(_mesh.vertices, Allocator.Temp);
        using var idx = new NativeArray<int>(_mesh.triangles, Allocator.Temp);

        var filter = CollisionFilter.Default;
        filter.CollidesWith = (uint)gameObject.layer;

        var collider = MeshCollider.Create
          (vtx.Reinterpret<float3>(),
           idx.Reinterpret<int3>(sizeof(int)),
           filter);

        manager.SetComponentData
          (entity, new PhysicsCollider{ Value = collider });

        manager.AddSharedComponentManaged
          (entity, new PhysicsWorldIndex{ Value = 0 });
    }
}
