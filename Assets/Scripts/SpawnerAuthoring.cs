using Unity.Entities;
using UnityEngine;

public class SpawnerAuthoring : MonoBehaviour
{
    [SerializeField] GameObject _prefab = null;
    [SerializeField] int _spawnCount = 10;

    class Baker : Baker<SpawnerAuthoring>
    {
        public override void Bake(SpawnerAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.None);
            var prefab = GetEntity(authoring._prefab, TransformUsageFlags.Dynamic);
            AddComponent(entity, new Spawner{Prefab = prefab, SpawnCount = authoring._spawnCount});
        }
    }
}

struct Spawner : IComponentData
{
    public Entity Prefab;
    public int SpawnCount;
}
