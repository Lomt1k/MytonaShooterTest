
namespace MyTonaShooterTest.Units
{
    public class Modificator 
    {
        public enum ModType
        {
            Additive,
            Multiple
        }
        public enum StatType
        {
            moveSpeed,
            attackSpeed,
            incomingDamageMult,
            damageMult
        }

        public ModType modType;        
        public StatType statType;
        public float value;
    }
}
