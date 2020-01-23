using System;
using System.Collections.Generic;
using MyTonaShooterTest.Units;
using MyTonaShooterTest.UI;
using MyTonaShooterTest.Weapons;
using UnityEngine;

[Serializable]
public abstract class GameMode 
{
    protected List<Player> _players; //список всех игроков
    protected int _timeToEnd; //время до конца игры
    protected int _teams; //число команд

    public List<Player> players => _players;
    public int timeToEnd => _timeToEnd;
    public int teams => _teams;

    public GameMode(int players, int timeToEnd)
    {
        _teams = players;
        _timeToEnd = timeToEnd;

        //создаем игроков (по умолчанию каждый в отдельной команде) и добавляем их в листы
        _players = new List<Player>();        
        _players.Add(new Player(Spawner.instance.unitPrefab, false));
        for (int i = 1; i < players; i++)
        {
            _players.Add(new Player(Spawner.instance.unitPrefab, true, i));
        }

        ScreenGUI.instance.UpdateMatchTime(timeToEnd);
        ScreenGUI.instance.UpdatePlayerScore();
        OnGameModeInit();
    }

    public abstract void OnGameModeInit();
    public abstract void OnPlayerSpawn(Player player);
    public abstract void OnMatchEnd();

    public virtual void OnPlayerDeath(Player player, Player killer = null, Weapon weapon = null)
    {
        player.deaths++;
        if (killer != null && killer.teamID != player.teamID)
        {
            killer.kills++;
        }

        if (player == Player.mine || killer == Player.mine)
        {
            ScreenGUI.instance.UpdatePlayerScore();
        }
    }

    public virtual void OnSecUpdate()
    {
        _timeToEnd--;
        ScreenGUI.instance.UpdateMatchTime(timeToEnd);
        if (_timeToEnd == 0)
        {
            OnMatchEnd();

            //останавливаем игру
            foreach (var unit in UnitsHolder.units)
            {
                GameObject.Destroy(unit.gameObject);
            }
            ScreenGUI.instance.ShowResultsMenu();
        }
    }

}
