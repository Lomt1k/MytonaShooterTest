using System;
using System.Collections.Generic;
using UnityEngine;

namespace MyTonaShooterTest.Units
{
    [Serializable]
    public class UnitStats
    {
        public Stat moveSpeed;
        public Stat attackSpeed;
        public Stat incomingDamageMult;
        public Stat damageMult;

        private Unit _statOwner;

        public Unit statOwner => _statOwner;        

        public UnitStats(UnitStatsData defaultUnitStats)
        {
            moveSpeed = new Stat(defaultUnitStats.moveSpeed);
            attackSpeed = new Stat(defaultUnitStats.attackSpeed);
            incomingDamageMult = new Stat(defaultUnitStats.incomingDamageMult);
            damageMult = new Stat(defaultUnitStats.dagameMult);

            ReloadStats();
        }

        public void AddAbility(Ability ability)
        {
            foreach (var mod in ability.mods)
            {
                switch (mod.statType)
                {
                    case StatType.moveSpeed:
                        moveSpeed.ChangeValue(mod.modType, mod.value);
                        break;
                    case StatType.attackSpeed:
                        attackSpeed.ChangeValue(mod.modType, mod.value);
                        break;
                    case StatType.incomingDamageMult:
                        incomingDamageMult.ChangeValue(mod.modType, mod.value);
                        break;
                    case StatType.damageMult:
                        damageMult.ChangeValue(mod.modType, mod.value);
                        break;
                }
            }
            ReloadStats();
        }

        public void RemoveAbility(Ability ability)
        {
            foreach (var mod in ability.mods)
            {
                switch (mod.statType)
                {
                    case StatType.moveSpeed:
                        moveSpeed.ChangeValue(mod.modType, -mod.value);
                        break;
                    case StatType.attackSpeed:
                        attackSpeed.ChangeValue(mod.modType, -mod.value);
                        break;
                    case StatType.incomingDamageMult:
                        incomingDamageMult.ChangeValue(mod.modType, -mod.value);
                        break;
                    case StatType.damageMult:
                        damageMult.ChangeValue(mod.modType, -mod.value);
                        break;
                }
            }
            ReloadStats();
        }

        public void ReloadStats()
        {
            if (statOwner == null) return;

            if (statOwner.isBot)
            {
                statOwner.GetComponent<BotAI>().agent.speed = moveSpeed.value;
            }
        }

        public void SetOwner(Unit owner)
        {
            _statOwner = owner;
        }



        
    }
}

