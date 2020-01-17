using UnityEngine;
using System.Collections;

namespace MyTonaShooterTest.VFX
{
    public class Bullet : MonoBehaviour, IPoolObject
    {
        private float moveSpeed;
        private float maxDistance = 1000f; //максимальная дистанция, которую пуля еще может пролететь (от shotDistance оружия)

        public void SetBulletInfo(float speed, float shotDistance)
        {
            moveSpeed = speed;
            maxDistance = shotDistance;
        }

        public void SetBulletInfo(float speed, Vector3 hitPos)
        {
            moveSpeed = speed;
            //вычисляем время до попадания пули в цель и уничтожаем её в тот момент
            float distance = Vector3.Distance(transform.position, hitPos);
            float timeToHit = distance / moveSpeed * Time.deltaTime;
            StartCoroutine(DestroyBulletInTime(timeToHit));
        }


        // IPoolObject
        public void OnPop()
        {
            gameObject.SetActive(true);
        }
        public void OnPush()
        {
            gameObject.SetActive(false);
            GameManager.instance.rifleBulletPool.Push(this);            
        }

        private void Update()
        {
            transform.Translate(-transform.forward * moveSpeed);
            maxDistance -= moveSpeed;
            if (maxDistance <= 0f) OnPush();
        }

        private IEnumerator DestroyBulletInTime(float time)
        {
            yield return new WaitForSeconds(time);
            OnPush();
        }     
    }


}
