using System.Collections.Generic;

namespace MyTonaShooterTest.Units
{
    public static class UnitsHolder
    {
        public static List<Unit> units => _units;

        private static List<Unit> _units;

        static UnitsHolder()
        {
            _units = new List<Unit>();
        }

    }

}
