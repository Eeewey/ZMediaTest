using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public struct UnitStatsBlob
{
    public BlobArray<BlobArray<UnitStatModifier>> FormStats;
    public BlobArray<BlobArray<UnitStatModifier>> ColorStats;
    public BlobArray<BlobArray<UnitStatModifier>> SizeStats;
}

public struct TeamStartsBlob
{
    public BlobArray<float3> TeamPos;
}

public struct UnitVisualBlob
{
    public BlobArray<float4> Colors;
    public BlobArray<float> VisualScales;
}

public struct UnitStatsConfigBlob : IComponentData
{
    public BlobAssetReference<UnitStatsBlob> Blob;
}

public struct TeamConfigBlob : IComponentData
{
    public BlobAssetReference<TeamStartsBlob> Blob;
    public int UnitInTeamCount;
}

public struct UnitsVisualConfigBlob : IComponentData
{
    public BlobAssetReference<UnitVisualBlob> Blob;
}