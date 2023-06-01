using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;

public class RotationSpeedAuthoring : MonoBehaviour
{
    [SerializeField] float Speed = 500;

    class Baker : Baker<RotationSpeedAuthoring>
    {
        public override void Bake(RotationSpeedAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity,
              new RotationSpeed { Speed = math.radians(authoring.Speed) });
        }
    }
}

public struct RotationSpeed : IComponentData
{
    public float Speed;
}
