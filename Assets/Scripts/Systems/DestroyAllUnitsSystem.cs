using Unity.Entities;
using Unity.Collections;

[UpdateInGroup(typeof(InitializationSystemGroup))]
public partial struct DestroyAllUnitsSystem : ISystem
{
    public void OnUpdate(ref SystemState state)
    {
        if (!SystemAPI.TryGetSingletonEntity<DestroyAllUnitsRequest>(out var request))
            return;

        var ecb = new EntityCommandBuffer(Allocator.Temp);

        foreach (var (data, entity) in
         SystemAPI.Query<RefRO<UnitData>>()
             .WithEntityAccess())
        {
            ecb.DestroyEntity(entity);
        }

        ecb.DestroyEntity(request);

        ecb.Playback(state.EntityManager);
        ecb.Dispose();
    }
}
