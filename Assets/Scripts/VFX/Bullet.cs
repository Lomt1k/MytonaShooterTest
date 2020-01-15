using UnityEngine;

namespace MyTonaShooterTest.VFX
{
    public class Bullet : MonoBehaviour
    {
        void OnCollisionEnter(Collision collision)
        {
            Destroy(gameObject);
        }
    }


}
