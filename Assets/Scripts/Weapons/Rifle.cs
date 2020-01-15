using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyTonaShooterTest.Weapons;
using MyTonaShooterTest.Units;

public class Rifle : Weapon
{

    protected override void Shot()
    {
        base.Shot();

        Ray ray = new Ray(shotOrigin.position, shotOrigin.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.gameObject.CompareTag("Unit"))
            {
                hit.transform.gameObject.GetComponent<Unit>()?.TakeDamage(_weaponOwner, this, weaponData.damage);
            }
        }
        //создаем пулю (визуал)
        GameObject bullet = Instantiate(weaponData.bulletPrefab, shotOrigin.position, weaponData.bulletPrefab.transform.rotation);
        bullet.transform.localEulerAngles += new Vector3(0f, _weaponOwner.transform.localEulerAngles.y, 0f);
        bullet.GetComponent<Rigidbody>().AddForce(bullet.transform.up * weaponData.bulletSpeed);
        Destroy(bullet, weaponData.bulletLifetime);

    }
}
