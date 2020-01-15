using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyTonaShooterTest.Units
{
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

            SpawnUnit(playerPrefab, 0);
            //test
            for (int i = 0; i < 4; i++)
            {
                SpawnUnit(botPrefab, 1);
            }
        }

        // Update is called once per frame
        void Update()
        {

        }

        //спавн юнита
        public GameObject SpawnUnit(GameObject unitPrefab, int teamid)
        {
            //проверяем - является ли префаб юнитом
            if (unitPrefab.GetComponent<Unit>() == null) return null;

            //спавним юнита
            int rand = Random.Range(0, unitSpawns.Length);
            GameObject obj = Instantiate(unitPrefab, unitSpawns[rand].position, unitSpawns[rand].rotation);
            Unit unit = obj.GetComponent<Unit>();
            unit.SetTeam(teamid);
            UnitsHolder.units.Add(unit);
            //цепляем камеру за юнитом, если это игрок
            if (unit.isBot == false)
            {
                Camera.main.GetComponent<CameraFollow>().FollowTo(obj.transform);
            }
            return obj;
        }

        public void SpawnUnit(GameObject unitPrefab, int teamid, float time)
        {
            StartCoroutine(RequestSpawnUnit(unitPrefab, teamid, time));
        }

        private IEnumerator RequestSpawnUnit(GameObject unitPrefab, int teamid, float time)
        {
            yield return new WaitForSeconds(time);
            SpawnUnit(unitPrefab, teamid);
        }
    }

}
