using System;
using System.Collections.Generic;
using UnityEngine;

public enum UnitFormType
{
    Cube,
    Sphere,
    Count
}

public enum UnitColorType
{
    Blue,
    Green,
    Red,
    Count
}
public enum UnitSizeType
{
    Big,
    Small,
    Count
}

public enum UnitBaseStatType
{
    Hp,
    Atk,
    Spd,
    AtkSpd
}

[Serializable]
public class ParamData
{
    public UnitBaseStatType StatType;
    public float Value;
}

[Serializable]
public class FormStat
{
    public UnitFormType FormType;
    public List<ParamData> Params = new();
}

[Serializable]
public class ColorStat
{
    public UnitColorType ColorType;
    public List<ParamData> Params = new();
}

[Serializable]
public class SizeStat
{
    public UnitSizeType SizeType;
    public List<ParamData> Params = new();
}

[CreateAssetMenu(fileName = "UnitStatsConfig", menuName = "Create UnitStatsConfig")]
public class UnitStatsConfig : ScriptableObject
{
    public int UnitInTeamCount = 20;

    public List<Vector3> TeamStartPoses = new();

    public float baseHp = 100f;
    public float baseAtk = 10f;
    public float baseSpd = 5f;
    public float baseAtkSpd = 1f;
    public float AttackRange = 1f;

    public List<FormStat> FormStats = new();
    public List<ColorStat> ColorStats = new();
    public List<SizeStat> SizeStats = new();

    public List<Color> ColorsUnit;
    public List<float> VisualScales = new();
}