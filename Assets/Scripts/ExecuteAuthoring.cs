using Unity.Entities;
using UnityEngine;

namespace Execute {
    public struct MainThread : IComponentData
    {
    }

    public class ExecuteAuthoring : MonoBehaviour
    {
        class Baker : Baker<ExecuteAuthoring>
        {
            public override void Bake(ExecuteAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.None);
                AddComponent<MainThread>(entity);
            }
        }
    }
}
