using UnityEngine;
using Unity.Entities;

public class LifetimeAuthoring : MonoBehaviour
{
    [SerializeField] float _life = 5;

    class Baker : Baker<LifetimeAuthoring>
    {
        public override void Bake(LifetimeAuthoring authoring)
          => AddComponent(
               GetEntity(TransformUsageFlags.Dynamic),
               new Lifetime{Life = authoring._life});
    }
}

public struct Lifetime : IComponentData
{
    public float Life;
}
