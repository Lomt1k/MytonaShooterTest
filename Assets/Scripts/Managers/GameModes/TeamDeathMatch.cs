using UnityEngine;
using MyTonaShooterTest.Weapons;
using MyTonaShooterTest.UI;

public class TeamDeathMatch : GameMode
{
    private int[] _team_kills; //количество убийств у команды

    public int[] team_kills => _team_kills;

    public TeamDeathMatch(int players, int time) : base(players, time)
    {
    }

    public override void OnGameModeInit()
    {
        _teams = 2;
        _team_kills = new int[_teams];
        int tempTeamID = 0;
        foreach (var player in _players)
        {            
            player.SetTeam(tempTeamID);
            player.Respawn();

            tempTeamID++;
            if (tempTeamID >= _teams) tempTeamID = 0;
        }

        ScreenGUI.instance.tdm_panel.SetActive(true);
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

    public override void OnPlayerDeath(Player player, Player killer = null, Weapon weapon = null)
    {
        base.OnPlayerDeath(player, killer, weapon);

        if (killer != null)
        {
            if (killer.teamID != player.teamID)
            {
                _team_kills[killer.teamID]++;
                ScreenGUI.instance.UpdateTeamScore();
            }
        }
    }

    public override void OnMatchEnd()
    {
        Debug.Log("Командный Десматч завершен!");
        if (team_kills[0] == team_kills[1])
        {
            Debug.Log($"Результат: Ничья (Счет {team_kills[0]}:{team_kills[1]})");
        }
        else if (team_kills[0] > team_kills[1])
        {
            Debug.Log($"Результат: Победила зеленая команда (Счет {team_kills[0]}:{team_kills[1]})");
        }
        else
        {
            Debug.Log($"Результат: Победила красная команда (Счет {team_kills[1]}:{team_kills[0]})");
        }

        foreach (Player player in players)
        {
            string result = $"{player.nickname} | Kills: {player.kills} | Deaths: {player.deaths}";
            Debug.Log(result);
        }
    }
}
