using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

public partial struct AttackSystem : ISystem
{
    public void OnUpdate(ref SystemState state)
    {
        float dt = SystemAPI.Time.DeltaTime;

        foreach (var (stats, cooldown, target, unitState, range, transform) in
                 SystemAPI.Query<
                     RefRO<UnitBaseStats>,
                     RefRW<AttackCooldown>,
                     RefRO<Target>,
                     RefRO<UnitState>,
                     RefRO<AttackRange>,
                     RefRO<LocalTransform>>())
        {
            if (cooldown.ValueRW.TimeLeft >= 0f)
            {
                cooldown.ValueRW.TimeLeft -= dt;
                continue;
            }

            if (unitState.ValueRO.Value != UnitStateEnum.Attack)
                continue;

            if (!SystemAPI.Exists(target.ValueRO.Value))
                continue;

            if (!SystemAPI.HasComponent<LocalTransform>(target.ValueRO.Value))
                continue;

            var targetTransform =
                SystemAPI.GetComponent<LocalTransform>(target.ValueRO.Value);

            float dist = math.distance(
                transform.ValueRO.Position,
                targetTransform.Position);

            if (dist > range.ValueRO.Value)
                continue;

            var enemyStats = SystemAPI.GetComponentRW<UnitBaseStats>(target.ValueRO.Value);

            enemyStats.ValueRW.Hp -= stats.ValueRO.Atk;
            cooldown.ValueRW.TimeLeft = stats.ValueRO.AtkSpd;
        }
    }
}
