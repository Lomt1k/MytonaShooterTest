using System.Linq;
using System.Collections.Generic;
using MyTonaShooterTest.UI;
using MyTonaShooterTest.Weapons;
using MyTonaShooterTest.Languages;
using UnityEngine;

public class DeathMatch : GameMode
{
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

    public override void OnMatchEnd()
    {
        //сортируем игроков по наибольшему количеству убийств и наименьшему числу смертей
        List<Player> sortedPlayers = players.OrderByDescending(x => x.kills).ThenBy(x => x.deaths).ToList();

        //отображаем результаты матча
        ScreenGUI.instance.results_title.text = Language.data["dm_ends"];
        string sub = string.Format(Language.data["dm_wins"], sortedPlayers[0].nickname, sortedPlayers[0].kills);            
        ScreenGUI.instance.results_sub.text = sub;

        ScreenGUI.instance.results_header_name.text = Language.data["dm_name"];
        ScreenGUI.instance.results_header_kills.text = Language.data["screenGUI_kills"];
        ScreenGUI.instance.results_header_deaths.text = Language.data["screenGUI_deaths"];
        ScreenGUI.instance.results_back_to_menu.text = Language.data["screnGUI_backToMenu"];
        
        string list_names = "";
        string list_kills = "";
        string list_deaths = "";
        foreach (Player player in sortedPlayers)
        {
            list_names += $"{player.nickname}\n";
            list_kills += $"{player.kills}\n";
            list_deaths += $"{player.deaths}\n";
        }
        ScreenGUI.instance.results_names_list.text = list_names;
        ScreenGUI.instance.results_kills_list.text = list_kills;
        ScreenGUI.instance.reslts_deaths_list.text = list_deaths;

    }
}
