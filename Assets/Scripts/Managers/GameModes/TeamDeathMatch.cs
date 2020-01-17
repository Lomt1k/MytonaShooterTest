using UnityEngine;

public class TeamDeathMatch : GameMode
{
    public TeamDeathMatch(int players, float time) : base(players, time)
    {
    }

    public override void OnGameModeInit()
    {
        _teams = 2;
        int tempTeamID = 0;
        foreach (var player in _players)
        {            
            player.SetTeam(tempTeamID);
            player.Respawn();

            tempTeamID++;
            if (tempTeamID >= _teams) tempTeamID = 0;
        }

    }

    public override void OnPlayerSpawn(Player player)
    {
        //перекрашиваем ботов в зависимости от их команды
        if (player.isBot)
        {
            if (player.teamID == 0)
            {
                player.unit.GetComponent<MeshRenderer>().material.color = Color.green;
            }
            else
            {
                player.unit.GetComponent<MeshRenderer>().material.color = Color.red;
            }
        }
        
    }
}
