using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;

public class UnitVisualBaker : Baker<UnitVisualAuthoring>
{
    public override void Bake(UnitVisualAuthoring authoring)
    {
        var entity = GetEntity(TransformUsageFlags.Renderable);

        AddComponent(entity, new URPMaterialPropertyBaseColor
        {
            Value = new float4(1f, 1f, 1f, 1f)
        });
    }
}