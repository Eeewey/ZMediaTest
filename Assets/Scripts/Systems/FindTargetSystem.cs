using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

public partial struct FindTargetSystem : ISystem
{
    private EntityQuery _unitsQuery;

    public void OnCreate(ref SystemState state)
    {
        _unitsQuery = state.GetEntityQuery(
            ComponentType.ReadOnly<UnitBaseStats>(),
            ComponentType.ReadOnly<UnitData>(),
            ComponentType.ReadOnly<LocalTransform>()
        );
    }

    public void OnUpdate(ref SystemState state)
    {
        var enemies = _unitsQuery.ToEntityArray(Allocator.Temp);
        var enemyTeams = _unitsQuery.ToComponentDataArray<UnitData>(Allocator.Temp);
        var enemyPositions = _unitsQuery.ToComponentDataArray<LocalTransform>(Allocator.Temp);

        foreach (var (transform, data, entity) in
                 SystemAPI.Query<RefRO<LocalTransform>, RefRO<UnitData>>().WithEntityAccess())
        {
            var minDist = float.MaxValue;
            var closest = Entity.Null;

            for (int i = 0; i < enemies.Length; ++i)
            {
                if (enemyTeams[i].TeamId == data.ValueRO.TeamId)
                    continue;

                var dist = math.distance(transform.ValueRO.Position, enemyPositions[i].Position);

                if (dist < minDist)
                {
                    minDist = dist;
                    closest = enemies[i];
                }
            }

            if (closest != Entity.Null)
            {
                state.EntityManager.SetComponentData(entity, new Target
                {
                    Value = closest
                });
            }
        }

        enemies.Dispose();
        enemyTeams.Dispose();
        enemyPositions.Dispose();
    }
}