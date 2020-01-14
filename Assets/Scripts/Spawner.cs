using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    static Spawner _instance;

    public GameObject playerPrefab;
    public GameObject botPrefab;
    public Transform[] unitSpawns;


    public static Spawner instance
    {
        get => _instance;
    }


    // Start is called before the first frame update
    void Start()
    {
        _instance = this;

        SpawnUnit(playerPrefab);
        //test
        for (int i = 0; i < 5; i++)
        {
            SpawnUnit(botPrefab);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public GameObject SpawnUnit(GameObject unitPrefab)
    {
        //проверяем - является ли префаб юнитом
        if (unitPrefab.GetComponent<Unit>() == null) return null;

        //спавним юнита
        int rand = Random.Range(0, unitSpawns.Length);
        GameObject obj = Instantiate(unitPrefab, unitSpawns[rand].position, unitSpawns[rand].rotation);
        Unit unit = obj.GetComponent<Unit>();
        UnitsHolder.units.Add(unit);
        //цепляем камеру за юнитом, если это игрок
        if (unit.isBot == false)
        {
            Camera.main.GetComponent<CameraFollow>().FollowTo(obj.transform);
        }
        return obj;
    }
}
