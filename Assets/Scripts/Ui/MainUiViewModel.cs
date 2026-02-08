using Unity.Entities;

public class MainUiViewModel
{
    private World world;
    private EntityManager entityManager;

    public MainUiViewModel()
    {
        world = World.DefaultGameObjectInjectionWorld;
        entityManager = world.EntityManager;
    }

    public void RebuildFormation()
    {
        var request = entityManager.CreateEntity();

        entityManager.AddComponent<RebuildFormationRequest>(request);
    }

    public void DestroyAllUnits()
    {
        var request = entityManager.CreateEntity();

        entityManager.AddComponent<DestroyAllUnitsRequest>(request);
    }

    public void StartMove()
    {
        var request = entityManager.CreateEntity();

        entityManager.AddComponent<MoveUnitsRequest>(request);
    }
}