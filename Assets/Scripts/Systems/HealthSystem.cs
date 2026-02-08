using Unity.Collections;
using Unity.Entities;

public partial struct HealthSystem : ISystem
{
    public void OnUpdate(ref SystemState state)
    {
        var ecb = new EntityCommandBuffer(Allocator.Temp);

        foreach (var (stats, entity) in
                 SystemAPI.Query<RefRO<UnitBaseStats>>().WithEntityAccess())
        {
            if (stats.ValueRO.Hp <= 0)
            {
                ecb.DestroyEntity(entity);
            }
        }

        ecb.Playback(state.EntityManager);
        ecb.Dispose();
    }
}