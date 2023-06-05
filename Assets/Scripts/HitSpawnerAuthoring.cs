using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using Random = Unity.Mathematics.Random;

public class HitSpawnerAuthoring : MonoBehaviour
{
    [SerializeField] GameObject _prefab = null;
    [SerializeField] float3 _extent = math.float3(3, 3, 5);
    [SerializeField] float _frequency = 100;
    [SerializeField] uint _seed = 123;

    class Baker : Baker<HitSpawnerAuthoring>
    {
        public override void Bake(HitSpawnerAuthoring authoring)
          => AddComponent
               (GetEntity(TransformUsageFlags.Dynamic),
                new HitSpawner
                {
                    Prefab = GetEntity(authoring._prefab,
                                       TransformUsageFlags.Dynamic),
                    Extent = authoring._extent,
                    Frequency = authoring._frequency,
                    Random = new Random(authoring._seed)
                });
    }
}

struct HitSpawner : IComponentData
{
    public Entity Prefab;
    public float3 Extent;
    public float Frequency;
    public Random Random;
    public float Timer;
}
