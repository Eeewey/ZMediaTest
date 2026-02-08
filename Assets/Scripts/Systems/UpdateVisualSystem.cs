using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;

public partial struct UpdateVisualSystem : ISystem
{
    public void OnUpdate(ref SystemState state)
    {
        var ecb = new EntityCommandBuffer(Allocator.Temp);
        var visualConfig = SystemAPI.GetSingleton<UnitsVisualConfigBlob>();

        foreach (var (data, visualEntity, transform, entity) in SystemAPI.Query<
                RefRO<UnitData>,
                RefRO<UnitVisulRef>,
                RefRW<LocalTransform>
            >()
            .WithAll<UnitNeedVisualUpdateTag>()
            .WithEntityAccess())
        {
            if (!SystemAPI.HasComponent<URPMaterialPropertyBaseColor>(visualEntity.ValueRO.Value))
                continue;

            var color = SystemAPI.GetComponentRW<URPMaterialPropertyBaseColor>(visualEntity.ValueRO.Value);


            ref var blob = ref visualConfig.Blob.Value;

            var colorIndex = (int)data.ValueRO.ColorType;

            if (colorIndex >= blob.Colors.Length)
                continue;

            var newColor = blob.Colors[colorIndex];

            color.ValueRW.Value = newColor;


            int scaleIndex = (int)data.ValueRO.SizeType;
            if (scaleIndex < blob.VisualScales.Length)
            {
                float scale = blob.VisualScales[scaleIndex];

                var entityTr = transform.ValueRW;

                entityTr.Scale = scale;
                transform.ValueRW = entityTr;
            }

            ecb.RemoveComponent<UnitNeedVisualUpdateTag>(entity);
        }

        ecb.Playback(state.EntityManager);
        ecb.Dispose();
    }
}