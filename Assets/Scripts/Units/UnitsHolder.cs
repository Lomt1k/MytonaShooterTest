using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyTonaShooterTest.Units
{
        public static class UnitsHolder
        {
            static List<Unit> _units;

            public static List<Unit> units
            {
                get => _units;
            }

            static UnitsHolder()
            {
                _units = new List<Unit>();
            }

        }

}
