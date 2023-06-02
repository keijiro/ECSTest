using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;

public class RotationAuthoring : MonoBehaviour
{
    [SerializeField] float Speed = 500;

    class Baker : Baker<RotationAuthoring>
    {
        public override void Bake(RotationAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity,
              new Rotation { Speed = math.radians(authoring.Speed) });
        }
    }
}

public struct Rotation : IComponentData
{
    public float Speed;
}
