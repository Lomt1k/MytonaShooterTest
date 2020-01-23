using System.Collections;
using UnityEngine;

namespace MyTonaShooterTest.VFX
{
    public class Explosion : MonoBehaviour, IPoolObject
    {

        public void SetExplosionInfo(float destroyTime)
        {
            StartCoroutine(DestroyInTime(destroyTime));
        }

        // IPoolObject
        public void OnPop()
        {
            gameObject.SetActive(true);
        }
        public void OnPush()
        {
            gameObject.SetActive(false);
            GameManager.instance.explosionPool.Push(this);
        }

        private IEnumerator DestroyInTime(float time)
        {
            yield return new WaitForSeconds(time);
            OnPush();
        }
    }
}
