using MyTonaShooterTest.UI;
using MyTonaShooterTest.Weapons;
using UnityEngine;

public class DeathMatch : GameMode
{
    private Player killLeader; //лидер по фрагам (либо тот, кто первым сделал Х убийств, если есть несколько игроков с этим числом фрагов)
    private int killLeader_kills;

    public DeathMatch(int players, int time) : base(players, time)
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

    public override void OnPlayerDeath(Player player, Player killer = null, Weapon weapon = null)
    {
        base.OnPlayerDeath(player, killer, weapon);

        //запоминание лидера по убийствам для отображения его в конце матча
        if (killer != null && killer.kills > killLeader_kills)
        {
            killLeader_kills = killer.kills;
            killLeader = killer;
        }
    }

    public override void OnMatchEnd()
    {
        Debug.Log($"Десматч завершен!");
        if (killLeader != null)
        {
            Debug.Log($"Победил {killLeader.nickname} с {killLeader_kills} убийствами");
        }

        foreach (Player player in players)
        {
            string result = $"{player.nickname} | Kills: {player.kills} | Deaths: {player.deaths}";
            Debug.Log(result);
        }        
    }
}
