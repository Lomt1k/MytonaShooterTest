using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyTonaShooterTest.Units
{
    public static class UnitsHolder
    {
        public static List<Unit> units => _units;

        static List<Unit> _units;

        static UnitsHolder()
        {
            _units = new List<Unit>();
        }

    }

}
