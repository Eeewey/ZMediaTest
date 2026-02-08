using Unity.Entities;
using Unity.Collections;
using Unity.Mathematics;

public class UnitStatsConfigBaker : Baker<UnitStatsConfigAuthoring>
{
    public override void Bake(UnitStatsConfigAuthoring authoring)
    {
        var config = authoring.Config;

        using var builder = new BlobBuilder(Allocator.Temp);
        using var builderTeam = new BlobBuilder(Allocator.Temp);
        using var builderVisual = new BlobBuilder(Allocator.Temp);

        ref var rootStats = ref builder.ConstructRoot<UnitStatsBlob>();
        ref var rootTeam = ref builderTeam.ConstructRoot<TeamStartsBlob>();
        ref var rootVisual = ref builderVisual.ConstructRoot<UnitVisualBlob>();

        BuildFormStats(ref rootStats, builder, config);
        BuildColorStats(ref rootStats, builder, config);
        BuildSizeStats(ref rootStats, builder, config);

        BuildTeamStartsBlob(ref rootTeam, builderTeam, config);
        BuildVisualBlob(ref rootVisual, builderVisual, config);

        var blobStatRef = builder.CreateBlobAssetReference<UnitStatsBlob>(
            Allocator.Persistent);

        var blobTeamRef = builderTeam.CreateBlobAssetReference<TeamStartsBlob>(
            Allocator.Persistent);
        var blobVisualRef = builderVisual.CreateBlobAssetReference<UnitVisualBlob>(
            Allocator.Persistent);

        AddBlobAsset(ref blobStatRef, out var hashStat);
        AddBlobAsset(ref blobTeamRef, out var hashTeam);
        AddBlobAsset(ref blobVisualRef, out var hashVis);

        var entity = GetEntity(TransformUsageFlags.None);

        AddComponent(entity, new UnitStatsConfigBlob
        {
            Blob = blobStatRef
        });

        AddComponent(entity, new UnitBaseStats
        {
            Hp = config.baseHp,
            Atk = config.baseAtk,
            Spd = config.baseSpd,
            AtkSpd = config.baseAtkSpd
        });

        AddComponent(entity, new AttackRange
        {
            Value = config.AttackRange
        });

        AddComponent(entity, new TeamConfigBlob
        {
            Blob = blobTeamRef,
            UnitInTeamCount = config.UnitInTeamCount
        });

        AddComponent(entity, new UnitsVisualConfigBlob
        {
            Blob = blobVisualRef
        });
    }


    void BuildFormStats(
        ref UnitStatsBlob root,
        BlobBuilder builder,
        UnitStatsConfig config)
    {
        var array = builder.Allocate(
            ref root.FormStats,
            (int)UnitFormType.Count);

        for (int i = 0; i < array.Length; ++i)
        {
            array[i] = default;
        }

        foreach (var stat in config.FormStats)
        {
            var target = builder.Allocate(
                ref array[(int)stat.FormType],
                stat.Params.Count);

            for (int i = 0; i < stat.Params.Count; ++i)
            {
                target[i] = new UnitStatModifier
                {
                    StatType = stat.Params[i].StatType,
                    Value = stat.Params[i].Value
                };
            }
        }
    }

    void BuildColorStats(
        ref UnitStatsBlob root,
        BlobBuilder builder,
        UnitStatsConfig config)
    {
        var array = builder.Allocate(
            ref root.ColorStats,
            (int)UnitColorType.Count);

        for (int i = 0; i < array.Length; ++i)
        {
            array[i] = default;
        }

        foreach (var stat in config.ColorStats)
        {
            var target = builder.Allocate(
                ref array[(int)stat.ColorType],
                stat.Params.Count);

            for (int i = 0; i < stat.Params.Count; ++i)
            {
                target[i] = new UnitStatModifier
                {
                    StatType = stat.Params[i].StatType,
                    Value = stat.Params[i].Value
                };
            }
        }
    }

    void BuildSizeStats(
        ref UnitStatsBlob root,
        BlobBuilder builder,
        UnitStatsConfig config)
    {
        var array = builder.Allocate(
            ref root.SizeStats,
            (int)UnitSizeType.Count);

        for (int i = 0; i < array.Length; ++i)
        {
            array[i] = default;
        }

        foreach (var stat in config.SizeStats)
        {
            var target = builder.Allocate(
                ref array[(int)stat.SizeType],
                stat.Params.Count);

            for (int i = 0; i < stat.Params.Count; ++i)
            {
                target[i] = new UnitStatModifier
                {
                    StatType = stat.Params[i].StatType,
                    Value = stat.Params[i].Value
                };
            }
        }
    }

    void BuildTeamStartsBlob(
        ref TeamStartsBlob root,
        BlobBuilder builder,
        UnitStatsConfig config)
    {
        var teamPoses = builder.Allocate(ref root.TeamPos, config.TeamStartPoses.Count);
        for (int i = 0; i < teamPoses.Length; ++i)
        {
            teamPoses[i] = (float3)config.TeamStartPoses[i];
        }
    }

    void BuildVisualBlob(
        ref UnitVisualBlob root,
        BlobBuilder builder,
        UnitStatsConfig config)
    {
        var colors = builder.Allocate(ref root.Colors, config.ColorsUnit.Count);
        for (int i = 0; i < colors.Length; ++i)
        {
            colors[i] = new float4(
                config.ColorsUnit[i].r,
                config.ColorsUnit[i].g,
                config.ColorsUnit[i].b,
                config.ColorsUnit[i].a);
        }

        var sizes = builder.Allocate(ref root.VisualScales, config.VisualScales.Count);
        for (int i = 0; i < sizes.Length; ++i)
        {
            sizes[i] = config.VisualScales[i];
        }
    }
}
