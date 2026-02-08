using Unity.Entities;
using Unity.Mathematics;

public struct UnitBaseStats : IComponentData
{
    public float Hp;
    public float Atk;
    public float Spd;
    public float AtkSpd;
}

public struct UnitStatModifier
{
    public UnitBaseStatType StatType;
    public float Value;
}


public struct UnitPrefabs : IComponentData
{
    public Entity CubePrefab;
    public Entity SpherePrefab;
}
public struct UnitData : IComponentData
{
    public UnitFormType FormType;
    public UnitColorType ColorType;
    public UnitSizeType SizeType;
    public int TeamId;
}

public enum UnitStateEnum : byte
{
    Move,
    Attack
}

public struct UnitNeedFormationTag : IComponentData { }
public struct UnitNeedVisualUpdateTag : IComponentData { }

public struct Target : IComponentData
{
    public Entity Value;
}

public struct AttackCooldown : IComponentData
{
    public float TimeLeft;
}
public struct AttackRange : IComponentData
{
    public float Value;
}

public struct UnitState : IComponentData
{
    public UnitStateEnum Value;
}

public struct UnitVisulRef : IComponentData
{
    public Entity Value;
}