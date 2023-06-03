using Unity.Entities;
using UnityEngine;

public class VoxelAuthoring : MonoBehaviour
{
    class Baker : Baker<VoxelAuthoring>
    {
        public override void Bake(VoxelAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new Voxel());
        }
    }
}

public struct Voxel : IComponentData
{
    public uint ID;
}
