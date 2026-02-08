using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;


public partial struct TeamFormationSystem : ISystem
{
    public void OnUpdate(ref SystemState state)
    {
        var ecb = new EntityCommandBuffer(Allocator.Temp);
        var teamConfig = SystemAPI.GetSingleton<TeamConfigBlob>();

        var spacing = 1.5f;
        var lineWidth = 5;

        int leftIndex = 0;
        int rightIndex = 0;

        foreach (var (transform, data, entity) in
                 SystemAPI.Query<RefRW<LocalTransform>, RefRO<UnitData>>()
                    .WithAll<UnitNeedFormationTag>()
                    .WithEntityAccess())
        {
            var basePos = teamConfig.Blob.Value.TeamPos[data.ValueRO.TeamId];
            var index = 0;

            if (data.ValueRO.TeamId == 0)
            {
                index = leftIndex++;
            }
            else
            {
                index = rightIndex++;
            }

            int row = index / lineWidth;
            int col = index % lineWidth;

            var startPos = basePos + new float3(
                col * spacing,
                0,
                row * spacing
            );

            transform.ValueRW.Position = startPos;

            ecb.RemoveComponent<UnitNeedFormationTag>(entity);
        }

        ecb.Playback(state.EntityManager);
        ecb.Dispose();
    }
}
