using System;
using System.Collections.Generic;
using UnityEngine;

namespace MyTonaShooterTest.Units
{
    [Serializable]
    public class UnitStats
    {
        public List<Ability> abilities;

        private UnitStatsData _defaultStats;
        private Unit _statOwner;

        [SerializeField]
        private float _moveSpeed; //скорость движения
        [SerializeField]
        private float _attackSpeed; //скорость атаки
        [SerializeField]
        private float _incomingDamageMult; //входящий урон
        [SerializeField]
        private float _dagameMult; //исходящий урон

        public Unit statOwner => _statOwner;        

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
            _defaultStats = defaultUnitStats;
            abilities = new List<Ability>();
            //! тут добавляем игроку абилки (если надо добавлять при спавне)
            RecalculateStats();            
        }

        public bool AddAbility(Ability ability)
        {
            if (!ability.canStack && abilities.Contains(ability)) return false;
            abilities.Add(ability);
            RecalculateStats();
            return true;
        }

        public void RemoveAbility(Ability ability, bool removeAll = false)
        {
            do
            {
                abilities.Remove(ability);
            } while (removeAll == true && abilities.Contains(ability));
            RecalculateStats();
        }

        public void RecalculateStats()
        {
            moveSpeed = _defaultStats.moveSpeed;
            attackSpeed = _defaultStats.attackSpeed;
            incomingDamageMult = _defaultStats.incomingDamageMult;
            dagameMult = _defaultStats.dagameMult;

            //сначала применяем к статам все модификаторы, которые добавляют значение
            foreach (var ab in abilities)
            {                
                foreach (var mod in ab.mods)
                {
                    if (mod.modType == Modificator.ModType.Additive)
                    {
                        switch (mod.statType)
                        {
                            case Modificator.StatType.moveSpeed:
                                moveSpeed += mod.value;
                                break;
                            case Modificator.StatType.attackSpeed:
                                attackSpeed += mod.value;
                                break;
                            case Modificator.StatType.incomingDamageMult:
                                incomingDamageMult += mod.value;
                                break;
                            case Modificator.StatType.damageMult:
                                dagameMult += mod.value;
                                break;
                        }
                    }
                }
            }

            //теперь применяем модификаторы, которые умножают значение
            foreach (var ab in abilities)
            {
                foreach (var mod in ab.mods)
                {
                    if (mod.modType == Modificator.ModType.Multiple)
                    {
                        switch (mod.statType)
                        {
                            case Modificator.StatType.moveSpeed:
                                moveSpeed *= mod.value;
                                break;
                            case Modificator.StatType.attackSpeed:
                                attackSpeed *= mod.value;
                                break;
                            case Modificator.StatType.incomingDamageMult:
                                incomingDamageMult *= mod.value;
                                break;
                            case Modificator.StatType.damageMult:
                                dagameMult *= mod.value;
                                break;
                        }
                    }
                }
            }

            //обновление статов у бота (на данный момент вызывается для обновления moveSpeed)
            if (statOwner != null && statOwner.isBot)
            {
                statOwner.GetComponent<BotAI>().LoadUnitStats();
            }

        }

        public void SetOwner(Unit owner)
        {
            _statOwner = owner;
        }



        
    }
}

