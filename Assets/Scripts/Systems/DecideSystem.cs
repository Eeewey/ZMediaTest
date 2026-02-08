using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

public partial struct DecideStateSystem : ISystem
{
    public void OnUpdate(ref SystemState state)
    {
        foreach (var (transform, target, range, unitState) in
                 SystemAPI.Query<
                    RefRO<LocalTransform>,
                    RefRO<Target>,
                    RefRO<AttackRange>,
                    RefRW<UnitState>>())
        {
            if (!SystemAPI.Exists(target.ValueRO.Value))
            {
                unitState.ValueRW.Value = UnitStateEnum.Move;
                continue;
            }

            if (!SystemAPI.HasComponent<LocalTransform>(target.ValueRO.Value))
                continue;

            var targetTransform = SystemAPI.GetComponent<LocalTransform>(target.ValueRO.Value);

            var dist = math.distance(transform.ValueRO.Position, targetTransform.Position);

            if (dist <= range.ValueRO.Value)
                unitState.ValueRW.Value = UnitStateEnum.Attack;
            else
                unitState.ValueRW.Value = UnitStateEnum.Move;
        }
    }
}
