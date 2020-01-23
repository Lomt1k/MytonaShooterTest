using UnityEngine;
using MyTonaShooterTest.Weapons;
using MyTonaShooterTest.Units;
using MyTonaShooterTest.VFX;

public class Rifle : Weapon
{

    protected override void Shot()
    {
        base.Shot();

        //создаем пулю (визуал)
        Bullet bullet = GameManager.instance.rifleBulletPool.GetPoolObject();
        bullet.transform.position = shotOrigin.position;
        bullet.transform.rotation = weaponData.bulletPrefab.transform.rotation;
        bullet.transform.localEulerAngles += new Vector3(0f, _weaponOwner.transform.localEulerAngles.y, 0f);

        Ray ray = new Ray(shotOrigin.position, shotOrigin.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, weaponData.shotDistance))
        {
            bullet.SetBulletInfo(weaponData.bulletSpeed, hit.transform.position); //пуля создается с уничтожением при попадании в цель
            if (hit.transform.gameObject.CompareTag("Unit"))
            {                
                hit.transform.gameObject.GetComponent<Unit>()?.TakeDamage(_weaponOwner, this, weaponData.damage);
            }
        }     
        else
        {
            bullet.SetBulletInfo(weaponData.bulletSpeed, weaponData.shotDistance); //пуля создается с уничтожением по прохождению shotDistance
        }
    }
}
