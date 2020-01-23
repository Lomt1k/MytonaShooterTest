using System;

namespace MyTonaShooterTest.Units
{
    public enum ModType
    {
        Additive,
        Multiple
    }


    [Serializable]
    public class Modificator 
    {
        public ModType modType;  
        public StatType statType;
        public float value;
    }

}
