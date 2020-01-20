using UnityEngine;

namespace MyTonaShooterTest.Units
{
    [CreateAssetMenu(menuName = "Data/Unit Ability")]
    public class Ability : ScriptableObject
    {
        public new string name; //название способности
        public Modificator[] mods; //все модификаторы, из которых состоит способность
    }
}

