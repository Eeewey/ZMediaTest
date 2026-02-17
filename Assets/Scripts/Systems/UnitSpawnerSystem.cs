using Unity.Burst;
using Unity.Entities;
using Unity.Collections;
using Unity.Mathematics;


public partial struct UnitSpawnerSystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<UnitStatsConfigBlob>();
        state.RequireForUpdate<UnitPrefabs>();
        state.RequireForUpdate<UnitBaseStats>();
        state.RequireForUpdate<UnitStatsConfigBlob>();
        state.RequireForUpdate<TeamConfigBlob>();
        state.RequireForUpdate<AttackRange>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        if (!SystemAPI.TryGetSingletonEntity<RebuildFormationRequest>(out var request))
            return;

        var prefabs = SystemAPI.GetSingleton<UnitPrefabs>();
        var baseStats = SystemAPI.GetSingleton<UnitBaseStats>();
        var config = SystemAPI.GetSingleton<UnitStatsConfigBlob>();
        var teamConfig = SystemAPI.GetSingleton<TeamConfigBlob>();
        var attackRange = SystemAPI.GetSingleton<AttackRange>();

        ref var blob = ref config.Blob.Value;

        var seed = (uint)(
            (uint)state.WorldUnmanaged.SequenceNumber +
            UnityEngine.Time.frameCount
        );


        var random = Unity.Mathematics.Random.CreateFromIndex(seed);

        var ecb = new EntityCommandBuffer(Allocator.Temp);

        var totalUnits = teamConfig.UnitInTeamCount * teamConfig.Blob.Value.TeamPos.Length;
        var teamId = 0;

        for (int i = 0; i < totalUnits; ++i)
        {
            var form = (UnitFormType)random.NextInt(0, (int)UnitFormType.Count);
            var color = (UnitColorType)random.NextInt(0, (int)UnitColorType.Count);
            var size = (UnitSizeType)random.NextInt(0, (int)UnitSizeType.Count);

            if (i != 0 && i % teamConfig.UnitInTeamCount == 0)
            {
                teamId++;
            }

            Entity prefab;

            switch (form)
            {
                case UnitFormType.Cube:
                    prefab = prefabs.CubePrefab;
                    break;
                case UnitFormType.Sphere:
                    prefab = prefabs.SpherePrefab;
                    break;
                default:
                    prefab = prefabs.CubePrefab;
                    break;
            }

            var entity = ecb.Instantiate(prefab);

            var stats = baseStats;

            ApplyModifiers(ref stats, ref blob.FormStats[(int)form]);
            ApplyModifiers(ref stats, ref blob.ColorStats[(int)color]);
            ApplyModifiers(ref stats, ref blob.SizeStats[(int)size]);

            ecb.SetComponent(entity, stats);
            ecb.SetComponent(entity, new UnitData
            {
                FormType = form,
                ColorType = color,
                SizeType = size,
                TeamId = teamId
            });
            ecb.SetComponent(entity, new AttackRange { Value = attackRange.Value });

            ecb.SetComponent(entity, new UnitNeedFormationTag { });
            ecb.SetComponent(entity, new UnitNeedVisualUpdateTag { });
        }

        ecb.DestroyEntity(request);

        ecb.Playback(state.EntityManager);
        ecb.Dispose();
    }

    static void ApplyModifiers(
        ref UnitBaseStats stats,
        ref BlobArray<UnitStatModifier> mods)
    {
        for (int i = 0; i < mods.Length; i++)
        {
            switch (mods[i].StatType)
            {
                case UnitBaseStatType.Hp: stats.Hp += mods[i].Value; break;
                case UnitBaseStatType.Atk: stats.Atk += mods[i].Value; break;
                case UnitBaseStatType.Spd: stats.Spd += mods[i].Value; break;
                case UnitBaseStatType.AtkSpd: stats.AtkSpd += mods[i].Value; break;
            }
        }
    }
}