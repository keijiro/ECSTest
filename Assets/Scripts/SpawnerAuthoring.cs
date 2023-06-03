using Unity.Entities;
using UnityEngine;

public class SpawnerAuthoring : MonoBehaviour
{
    [SerializeField] GameObject _prefab = null;
    [SerializeField] float _spawnPerSecond = 10;

    class Baker : Baker<SpawnerAuthoring>
    {
        public override void Bake(SpawnerAuthoring authoring)
          => AddComponent(
               GetEntity(TransformUsageFlags.None),
               new Spawner{Prefab = GetEntity(authoring._prefab, TransformUsageFlags.Dynamic),
                           PerSecond = authoring._spawnPerSecond});
    }
}

struct Spawner : IComponentData
{
    public Entity Prefab;
    public float PerSecond;
    public float Timer;
    public uint Seed;
}
