using Unity.Entities;
using UnityEngine;

public class VoxelizerAuthoring : MonoBehaviour
{
    [SerializeField] float _voxelSize = 0.05f;
    [SerializeField] float _voxelLife = 0.3f;
    [SerializeField] float _gravity = 0.2f;

    class Baker : Baker<VoxelizerAuthoring>
    {
        public override void Bake(VoxelizerAuthoring self)
          => AddComponent(GetEntity(TransformUsageFlags.None),
                          new Voxelizer(){VoxelSize = self._voxelSize,
                                          VoxelLife = self._voxelLife,
                                          Gravity = self._gravity});
    }
}

public struct Voxelizer : IComponentData
{
    public float VoxelSize;
    public float VoxelLife;
    public float Gravity;
}
