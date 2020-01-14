using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/WeaponItem")]
public class WeaponData : ScriptableObject
{
    public new string name;
    public int damage; //урон от 1 пули
    public int bulletsPerShot = 1; //количество пуль за один выстрел
    public int magazineAmount; // количество патронов в обойме
    public float fireRate = 1f; //скорострельность
    public float reloadTime = 3f; //время перезарядки
    public GameObject bulletPrefab; //префаб пули (визуал)
    public float bulletLifetime = 1f; //время до уничтожения пули (визуал)
    public float bulletSpeed = 1500f; //скорость полета пули (визуал)

}
