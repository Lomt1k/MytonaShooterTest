using UnityEngine;

namespace MyTonaShooterTest.VFX
{
    public class Bullet : MonoBehaviour
    {
        float moveSpeed;
        float maxDistance = float.MaxValue; //максимальная дистанция, которую пуля еще может пролететь (от shotDistance оружия)

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
            Destroy(gameObject, timeToHit);
        }

        void Update()
        {
            transform.Translate(-transform.forward * moveSpeed);
            maxDistance -= moveSpeed;
            if (maxDistance <= 0f) Destroy(gameObject);
        }


    }


}
