using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public class SpawnerAuthoring : MonoBehaviour
{
    [SerializeField] GameObject _prefab = null;
    [SerializeField] float3 _extent = math.float3(3, 3, 5);
    [SerializeField] int2 _resolution = math.int2(160, 160);

    class Baker : Baker<SpawnerAuthoring>
    {
        public override void Bake(SpawnerAuthoring authoring)
          => AddComponent
               (GetEntity(TransformUsageFlags.Dynamic),
                new Spawner{Prefab = GetEntity(authoring._prefab,
                                               TransformUsageFlags.Dynamic),
                            Extent = authoring._extent,
                            Resolution = authoring._resolution});
    }
}

struct Spawner : IComponentData
{
    public Entity Prefab;
    public float3 Extent;
    public int2 Resolution;
}
