using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyTonaShooterTest.Units
{
    public class UnitStats
    {
        float _moveSpeed; //скорость движения
        float _attackSpeed; //скорость атаки
        float _incomingDamageMult; //входящий урон
        float _dagameMult; //исходящий урон

        public float moveSpeed
        {
            get => _moveSpeed;
            private set => _moveSpeed = value;
        }

        public float attackSpeed
        {
            get => _attackSpeed;
            private set => _attackSpeed = value;
        }

        public float incomingDamageMult
        {
            get => _incomingDamageMult;
            private set => _incomingDamageMult = value;
        }

        public float dagameMult
        {
            get => _dagameMult;
            private set => _dagameMult = value;
        }

        public UnitStats(UnitStatsData defaultUnitStats)
        {
            moveSpeed = defaultUnitStats.moveSpeed;
            attackSpeed = defaultUnitStats.attackSpeed;
            incomingDamageMult = defaultUnitStats.incomingDamageMult;
            dagameMult = defaultUnitStats.dagameMult;
        }
    }
}

