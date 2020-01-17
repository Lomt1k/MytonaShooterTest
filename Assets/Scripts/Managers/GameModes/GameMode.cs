using System;
using System.Collections.Generic;
using MyTonaShooterTest.Units;

[Serializable]
public abstract class GameMode 
{
    protected List<Player> _players; //список всех игроков
    protected float _timeToEnd; //время до конца игры
    protected int _teams; //число команд

    public List<Player> players => _players;
    public float timeToEnd => _timeToEnd;
    public int teams => _teams;

    public GameMode(int players, float timeToEnd)
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

        OnGameModeInit();
    }

    public abstract void OnGameModeInit();
    public abstract void OnPlayerSpawn(Player player);

}
