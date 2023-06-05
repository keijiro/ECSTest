using Unity.Entities;
using UnityEngine;

public class BoxAuthoring : MonoBehaviour
{
    [SerializeField] float _lifetime = 1;

    class Baker : Baker<BoxAuthoring>
    {
        public override void Bake(BoxAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new Box(){ Lifetime = authoring._lifetime });
        }
    }
}

public struct Box : IComponentData
{
    public float Lifetime;
}
