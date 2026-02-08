using Unity.Entities;

public class UnitPrefabBaker : Baker<UnitPrefabAuthoring>
{
    public override void Bake(UnitPrefabAuthoring authoring)
    {
        var entity = GetEntity(TransformUsageFlags.None);

        AddComponent(entity, new UnitPrefabs
        {
            CubePrefab = GetEntity(authoring.CubePrefabGO, TransformUsageFlags.Dynamic),
            SpherePrefab = GetEntity(authoring.SpherePrefabGO, TransformUsageFlags.Dynamic)
        });
    }
}