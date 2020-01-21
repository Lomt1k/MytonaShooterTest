using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using MyTonaShooterTest.Weapons;
using MyTonaShooterTest.UI;
using MyTonaShooterTest.Languages;

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
        KillMessage(player, killer, weapon);

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
        //сортируем игроков по победившей команде, по наибольшему количеству убийств и наименьшему числу смертей
        List<Player> sortedPlayers;
        if (team_kills[0] == team_kills[1])
        {
            sortedPlayers = players.OrderByDescending(x => x.kills).ThenBy(x => x.deaths).ToList();
            string sub = string.Format(Language.data["tdm_draw"], team_kills[0], team_kills[1]);
            ScreenGUI.instance.results_sub.text = sub;
        }
        else if (team_kills[0] > team_kills[1])
        {
            sortedPlayers = players.OrderBy(x => x.teamID).ThenByDescending(x => x.kills).ThenBy(x => x.deaths).ToList();
            string sub = string.Format(Language.data["tdm_win_green"], team_kills[0], team_kills[1]);
            ScreenGUI.instance.results_sub.text = sub;
        }
        else
        {
            sortedPlayers = players.OrderByDescending(x => x.teamID).ThenByDescending(x => x.kills).ThenBy(x => x.deaths).ToList();
            string sub = string.Format(Language.data["tdm_win_red"], team_kills[1], team_kills[0]);
            ScreenGUI.instance.results_sub.text = sub;
        }        

        //отображаем результаты матча
        ScreenGUI.instance.results_title.text = Language.data["tdm_ends"];
        ScreenGUI.instance.results_header_name.text = Language.data["dm_name"];
        ScreenGUI.instance.results_header_kills.text = Language.data["screenGUI_kills"];
        ScreenGUI.instance.results_header_deaths.text = Language.data["screenGUI_deaths"];
        ScreenGUI.instance.results_back_to_menu.text = Language.data["screnGUI_backToMenu"];

        string list_names = "";
        string list_kills = "";
        string list_deaths = "";
        foreach (Player player in sortedPlayers)
        {
            if (player.teamID == 0)
            {
                list_names += $"<color=green>{player.nickname}</color>\n";
                list_kills += $"<color=green>{player.kills}</color>\n";
                list_deaths += $"<color=green>{player.deaths}</color>\n";
            }
            else
            {
                list_names += $"<color=red>{player.nickname}</color>\n";
                list_kills += $"<color=red>{player.kills}</color>\n";
                list_deaths += $"<color=red>{player.deaths}</color>\n";
            }            
        }
        ScreenGUI.instance.results_names_list.text = list_names;
        ScreenGUI.instance.results_kills_list.text = list_kills;
        ScreenGUI.instance.reslts_deaths_list.text = list_deaths;
    }

    private void KillMessage(Player player, Player killer = null, Weapon weapon = null)
    {
        //victim
        string victim_str = "";
        if (player.teamID == 0)
        {
            victim_str = $"<color=green>{player.nickname}</color>";
        }
        else
        {
            victim_str = $"<color=red>{player.nickname}</color>";
        }
        //killer
        string killer_str = "";
        if (killer != null)
        {
            if (killer.teamID == 0)
            {
                killer_str = $"<color=green>{killer.nickname}</color>";
            }
            else
            {
                killer_str = $"<color=red>{killer.nickname}</color>";
            }
        }
        Sprite weapon_icon = null;
        if (weapon != null)
        {
            weapon_icon = weapon.weaponData.icon;
        }
        KillList.instance.ShowDeathMessage(victim_str, weapon_icon, killer_str);
    }
}
