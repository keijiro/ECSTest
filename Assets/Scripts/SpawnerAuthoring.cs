using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using Random = Unity.Mathematics.Random;

public class SpawnerAuthoring : MonoBehaviour
{
    [SerializeField] GameObject _prefab = null;
    [SerializeField] float3 _extent = 1;
    [SerializeField] float _frequency = 100;
    [SerializeField] uint _seed = 0xdeadbeef;

    class Baker : Baker<SpawnerAuthoring>
    {
        public override void Bake(SpawnerAuthoring authoring)
          => AddComponent
               (GetEntity(TransformUsageFlags.Dynamic),
                new Spawner
                {
                    Prefab = GetEntity(authoring._prefab,
                                       TransformUsageFlags.Dynamic),
                    Extent = authoring._extent,
                    Frequency = authoring._frequency,
                    Random = new Random(authoring._seed)
                });
    }
}

struct Spawner : IComponentData
{
    public Entity Prefab;
    public float3 Extent;
    public float Frequency;
    public Random Random;
    public float Timer;
}
