using UnityEngine;
using MyTonaShooterTest.Units;

public class Player 
{
    public Unit unit;

    private GameObject _unitPrefab;
    private int _teamID;
    private bool _isBot;

    public static Player mine;
    public GameObject unitPrefab => _unitPrefab;
    public int teamID => _teamID;
    public bool isBot => _isBot;

    public Player(GameObject unitPrefab, bool isBot, int teamID = 0)
    {        
        _isBot = isBot;
        _unitPrefab = unitPrefab;
        _teamID = teamID;

        if (!isBot)
        {
            mine = this;
        }
    }

    public void SetTeam(int newTeam)
    {
        _teamID = newTeam;
    }

    public void Respawn()
    {
        if (unit != null)
        {
            GameObject.Destroy(unit.gameObject);
        }
        unit = Spawner.instance.SpawnUnit(this);
    }
    


}
