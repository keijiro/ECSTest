using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using Random = Unity.Mathematics.Random;

public class RandomSpawnerAuthoring : MonoBehaviour
{
    [SerializeField] GameObject _prefab = null;
    [SerializeField] float3 _extent = math.float3(3, 3, 5);
    [SerializeField] float _perSecond = 10;
    [SerializeField] uint _seed = 123;

    class Baker : Baker<RandomSpawnerAuthoring>
    {
        public override void Bake(RandomSpawnerAuthoring authoring)
          => AddComponent
               (GetEntity(TransformUsageFlags.Dynamic),
                new RandomSpawner
                {
                    Prefab = GetEntity(authoring._prefab,
                                       TransformUsageFlags.Dynamic),
                    Extent = authoring._extent,
                    PerSecond = authoring._perSecond,
                    Random = new Random(authoring._seed)
                });
    }
}

struct RandomSpawner : IComponentData
{
    public Entity Prefab;
    public float3 Extent;
    public float PerSecond;
    public Random Random;
    public float Timer;
}
