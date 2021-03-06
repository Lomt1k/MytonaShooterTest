﻿using UnityEngine;
using MyTonaShooterTest.Weapons;
using MyTonaShooterTest.Units;

public class Shotgun : Weapon
{
    public float shotSpread = 60f;

    Quaternion _shotOriginDefaultRot;

    protected override void OnStart()
    {
        base.OnStart();
        _shotOriginDefaultRot = shotOrigin.localRotation;
    }

    protected override void Shot()
    {
        base.Shot();

        for (int i = 0; i < weaponData.bulletsPerShot; i++)
        {
            float randY = Random.Range(-shotSpread, shotSpread);
            shotOrigin.Rotate(0f, randY, 0f);

            Ray ray = new Ray(shotOrigin.position, shotOrigin.forward);
            Debug.DrawRay(shotOrigin.position, shotOrigin.forward);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, weaponData.shotDistance))
            {
                if (hit.transform.gameObject.CompareTag("Unit"))
                {
                    hit.transform.gameObject.GetComponent<Unit>()?.TakeDamage(_weaponOwner, this, weaponData.damage);
                }
            }            

            shotOrigin.localRotation = _shotOriginDefaultRot;
        }

        //создаем пулю (визуал)
        GameObject bulletObj = Instantiate(weaponData.bulletPrefab, shotOrigin.position, weaponData.bulletPrefab.transform.rotation, shotOrigin);
        bulletObj.transform.localEulerAngles += new Vector3(0f, _weaponOwner.transform.localEulerAngles.y, 0f);
        bulletObj.transform.localPosition += weaponData.bulletPrefab.transform.position;
        Destroy(bulletObj, weaponData.bulletLifetime);


    }
}
