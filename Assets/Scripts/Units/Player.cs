using UnityEngine;
using MyTonaShooterTest.Units;

public class Player 
{
    public Unit unit;
    public int kills;
    public int deaths;

    private GameObject _unitPrefab;
    private int _teamID;
    private bool _isBot;
    private string _nickname;

    public static Player mine;
    public GameObject unitPrefab => _unitPrefab;
    public int teamID => _teamID;
    public bool isBot => _isBot;
    public string nickname => _nickname;

    private string[] botnames =
    {
        "Eddy",
        "Freddy",
        "Teddy",
        "Ivan",
        "Feodor",
        "Alex",
        "Denis",
        "Stan",
        "Kyle",
        "Eric",
        "Kenny",
        "Max",
        "Dozer",
        "Tank",
        "Morpheus",
        "Neo"
    };

    public Player(GameObject unitPrefab, bool isBot, int teamID = 0)
    {        
        _isBot = isBot;
        _unitPrefab = unitPrefab;
        _teamID = teamID;

        if (!isBot)
        {
            mine = this;
            _nickname = "Player";            
        }
        else
        {
            _nickname = botnames[Random.Range(0, botnames.Length)];
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
