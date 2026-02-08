using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;

public class UnitBaker : Baker<UnitAuthoring>
{
    public override void Bake(UnitAuthoring authoring)
    {
        var entity = GetEntity(TransformUsageFlags.Dynamic);

        AddComponent<UnitData>(entity);
        AddComponent<UnitBaseStats>(entity);
        AddComponent<UnitNeedFormationTag>(entity);

        AddComponent<Target>(entity);
        AddComponent<AttackCooldown>(entity);
        AddComponent<AttackRange>(entity);
        AddComponent<UnitState>(entity);

        var visualEntity = GetEntity(authoring.VisualGO, TransformUsageFlags.Renderable);

        AddComponent(entity, new UnitVisulRef
        {
            Value = visualEntity
        });

        AddComponent<UnitNeedVisualUpdateTag>(entity);
    }
}
