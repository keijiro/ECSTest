using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;

public class RotationAuthoring : MonoBehaviour
{
    [SerializeField] float _speed = 500;

    class Baker : Baker<RotationAuthoring>
    {
        public override void Bake(RotationAuthoring authoring)
          => AddComponent(
               GetEntity(TransformUsageFlags.Dynamic),
               new Rotation{Speed = math.radians(authoring._speed)});
    }
}

public struct Rotation : IComponentData
{
    public float Speed;
}
