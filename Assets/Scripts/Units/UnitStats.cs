using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyTonaShooterTest.Units
{
    public class UnitStats
    {
        UnitStatsData _defaultStats;
        float _moveSpeed; //скорость движения
        float _attackSpeed; //скорость атаки
        float _incomingDamageMult; //входящий урон
        float _dagameMult; //исходящий урон

        public Modificator[] mods;

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

        public float incomingDamageMultiplicator
        {
            get => _incomingDamageMult;
            private set => _incomingDamageMult = value;
        }

        public float dagameMultiplicator
        {
            get => _dagameMult;
            private set => _dagameMult = value;
        }

        public UnitStats(UnitStatsData defaultUnitStats)
        {
            _defaultStats = defaultUnitStats;
            RecalculateStats();
        }

        public void RecalculateStats()
        {
            moveSpeed = _defaultStats.moveSpeed;
            attackSpeed = _defaultStats.attackSpeed;
            incomingDamageMultiplicator = _defaultStats.incomingDamageMult;
            dagameMultiplicator = _defaultStats.dagameMult;

            //to do
        }



        
    }
}

