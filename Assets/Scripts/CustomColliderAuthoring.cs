using Unity.Entities;
using UnityEngine;

public class CustomColliderAuthoring : MonoBehaviour
{
    class Baker : Baker<CustomColliderAuthoring>
    {
        public override void Bake(CustomColliderAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new CustomCollider());
        }
    }
}

public struct CustomCollider : IComponentData
{
    public uint Counter;
}
