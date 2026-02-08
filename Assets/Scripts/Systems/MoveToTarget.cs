using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

public partial struct MoveToTargetSystem : ISystem
{
    public void OnUpdate(ref SystemState state)
    {
        var dt = SystemAPI.Time.DeltaTime;

        foreach (var (transform, stats, target, range) in
                 SystemAPI.Query<RefRW<LocalTransform>, RefRO<UnitBaseStats>, RefRO<Target>, RefRO<AttackRange>>())
        {
            if (!SystemAPI.Exists(target.ValueRO.Value))
                continue;
            if (!SystemAPI.HasComponent<LocalTransform>(target.ValueRO.Value))
                continue;

            var targetTransform = SystemAPI.GetComponent<LocalTransform>(target.ValueRO.Value);

            var dist = math.distance(targetTransform.Position, transform.ValueRW.Position);
            var dir = math.normalize(targetTransform.Position - transform.ValueRW.Position);

            if (dist <= range.ValueRO.Value)
                continue;

            transform.ValueRW.Position += dir * stats.ValueRO.Spd * dt;
        }
    }
}