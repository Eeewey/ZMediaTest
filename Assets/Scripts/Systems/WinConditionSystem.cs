using System;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;

[UpdateAfter(typeof(HealthSystem))]
public partial struct WinConditionSystem : ISystem
{
    public static event Action OnGameOver;

    public void OnUpdate(ref SystemState state)
    {
        // if (SystemAPI.TryGetSingleton<GameOver>(out var gameOver))
        //     return;


        var teamsCount = new Dictionary<int, int>();

        foreach (var data in SystemAPI.Query<RefRO<UnitData>>())
        {
            if (!teamsCount.ContainsKey(data.ValueRO.TeamId))
                teamsCount[data.ValueRO.TeamId] = 0;

            teamsCount[data.ValueRO.TeamId]++;
        }

        var survivedTeams = 0;
        var lastTeamAlive = -1;

        foreach (var kvp in teamsCount)
        {
            if (kvp.Value > 0)
            {
                survivedTeams++;
                lastTeamAlive = kvp.Key;
            }
        }

        if (survivedTeams == 1)
        {
            DeclareWinner(ref state);

            OnGameOver?.Invoke();
        }
    }

    private void DeclareWinner(ref SystemState state)
    {
        var ecb = new EntityCommandBuffer(Allocator.Temp);

        ecb.Playback(state.EntityManager);
        ecb.Dispose();
    }
}
