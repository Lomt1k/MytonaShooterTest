﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyTonaShooterTest.Units
{
    public class Spawner : MonoBehaviour
    {
        static Spawner _instance;

        public GameObject unitPrefab;
        public Transform[] unitSpawns;


        public static Spawner instance
        {
            get => _instance;
        }       

        //спавн юнита
        public Unit SpawnUnit(Player player)
        {
            //спавним юнита
            int rand = Random.Range(0, unitSpawns.Length);
            GameObject go = Instantiate(player.unitPrefab, unitSpawns[rand].position, unitSpawns[rand].rotation);
            Unit unit = go.GetComponent<Unit>();
            unit.SetPlayer(player);
            UnitsHolder.units.Add(unit);
            //цепляем камеру за юнитом, если это игрок
            if (player.isBot == false)
            {
                Camera.main.GetComponent<CameraFollow>().FollowTo(go.transform);
            }
            return unit;
        }

        void Start()
        {
            _instance = this;
        }

    }

}
