using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public WeaponData weaponData;
    public Transform shotOrigin;

    int _ammo; //текущее количество патронов в обойме
    float _lastShotTime; //момент, когда был произведен последний выстрел
    bool _isReloading = false;
    
    protected Unit _weaponOwner; //юнит, которому принадлежит оружие

    public int Ammo
    {
        get => _ammo;
        private set => _ammo = value;
    }

    public bool isReloading
    {
        get => _isReloading;
        private set => _isReloading = value;
    }

    void Start()
    {
        OnStart();
    }

    protected virtual void OnStart()
    {
        _ammo = weaponData.magazineAmount;
    }

    /// <summary>
    /// вызывает попытку выстрелить из оружия
    /// </summary>
    /// <returns>Возвращает true в случае успешного выстрела</returns>
    public virtual bool TryShot()
    {
        if (_ammo <= 0) return false; 
        if (Time.time < 1f / weaponData.fireRate + _lastShotTime) return false;
        if (isReloading) return false;
        Shot();
        return true;
    }

    /// <summary>
    /// выстрел из оружия
    /// </summary>
    protected virtual void Shot()
    {
        _ammo--;
        _lastShotTime = Time.time;
        ScreenGUI.instance.UpdateAmmoText(_weaponOwner);
        print(_weaponOwner.gameObject.name + " выстрелил из " + weaponData.name);
        //создаем пулю (визуал)
        GameObject bullet = Instantiate(weaponData.bulletPrefab, shotOrigin.position, weaponData.bulletPrefab.transform.rotation);
        bullet.transform.localEulerAngles += new Vector3(0f, _weaponOwner.transform.localEulerAngles.y, 0f);
        bullet.GetComponent<Rigidbody>().AddForce(bullet.transform.up * weaponData.bulletSpeed);
        Destroy(bullet, weaponData.bulletLifetime);
    }

    /// <summary>
    /// вызывает попытку перезарядить оружие
    /// </summary>
    /// <returns>Возвращает true в случае, если началась перезарядка</returns>
    public virtual bool TryReload()
    {
        if (_ammo >= weaponData.magazineAmount) return false;
        if (isReloading) return false;
        StartCoroutine(Reload());      
        return true;
    }

    /// <summary>
    /// перезаряжает оружие
    /// </summary>
    protected virtual IEnumerator Reload()
    {
        isReloading = true;
        print(_weaponOwner.gameObject.name + " перезаряжает " + weaponData.name);
        yield return new WaitForSeconds(weaponData.reloadTime);
        isReloading = false;
        Ammo = weaponData.magazineAmount;
        ScreenGUI.instance.UpdateAmmoText(_weaponOwner);
        print(_weaponOwner.gameObject.name + " закончил перезарядку " + weaponData.name);
    }

    public void SetOwner(Unit owner)
    {
        _weaponOwner = owner;
    }


}
