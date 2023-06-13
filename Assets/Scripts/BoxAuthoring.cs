using Unity.Entities;
using UnityEngine;

public class BoxAuthoring : MonoBehaviour
{
    class Baker : Baker<BoxAuthoring>
    {
        public override void Bake(BoxAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new Box());
        }
    }
}

public struct Box : IComponentData
{
    public float Time;
}
