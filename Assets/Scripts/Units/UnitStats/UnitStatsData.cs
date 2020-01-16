using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyTonaShooterTest.Units
{
    [CreateAssetMenu(menuName = "Data/UnitStats")]
    public class UnitStatsData : ScriptableObject
    {
        public float moveSpeed = 10f; //скорость передвижения
        public float attackSpeed = 1f; //скорость атаки
        public float incomingDamageMult = 1f; //входящий урон
        public float dagameMult = 1f; //исходящий урон
    }
}

