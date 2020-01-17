using UnityEngine;

public class DeathMatch : GameMode
{
    public DeathMatch(int players, float time) : base(players, time)
    {        
    }

    public override void OnGameModeInit()
    {
        foreach (var player in players)
        {
            player.Respawn();
        }
    }

    public override void OnPlayerSpawn(Player player)
    {
        if (player.isBot)
        {
            player.unit.GetComponent<MeshRenderer>().material.color = Color.gray;
        }
    }
}
