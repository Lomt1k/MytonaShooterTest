using System;
using UnityEngine;

namespace MyTonaShooterTest.Units
{
    public enum StatType
    {
        moveSpeed,
        attackSpeed,
        incomingDamageMult,
        damageMult
    }


    [Serializable]
    public class Stat
    {
        [SerializeField]
        private float _value;
        private float _defaultValue;
        private float _add_value;
        private float _mult_value;

        public float value => _value;

        public Stat(float defaultValue)
        {
            _defaultValue = defaultValue;
            _value = defaultValue;
        }

        public void ChangeValue(ModType modType, float deltaValue)
        {
            switch (modType)
            {
                case ModType.Additive:
                    _add_value += deltaValue;
                    break;
                case ModType.Multiple:
                    _mult_value += deltaValue;
                    break;
            }

            _value = _defaultValue + _add_value;
            if (_mult_value != 0) _value *= _mult_value;
        }


    }
    
}
