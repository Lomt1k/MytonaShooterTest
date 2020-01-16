using UnityEngine;

namespace MyTonaShooterTest.Units
{
    [CreateAssetMenu(menuName = "Data/Unit Ability")]
    public class Ability : ScriptableObject
    {
        public new string name; //название способности
        public Modificator[] mods; //все модификаторы, из которых состоит способность
        public bool canStack; //может ли способность использоваться несколько раз одновременно (двойной урон + двойной урон = урон х4)
    }
}

