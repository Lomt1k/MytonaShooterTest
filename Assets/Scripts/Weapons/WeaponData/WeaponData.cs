﻿using UnityEngine;
using MyTonaShooterTest.Units;

namespace MyTonaShooterTest.Weapons
{
    [CreateAssetMenu(menuName = "Data/WeaponItem")]
    public class WeaponData : ScriptableObject
    {
        public new string name;
        public float damage; //урон от 1 пули
        public int bulletsPerShot = 1; //количество пуль за один выстрел
        public int magazineAmount; // количество патронов в обойме
        public float fireRate = 1f; //скорострельность
        public float reloadTime = 3f; //время перезарядки
        public float shotDistance = 100f; //дальность полета пуль
        public Ability shootingAbility; //способность (баф), который накладывается на игрока при стрельбе
        public GameObject bulletPrefab; //префаб пули (визуал)
        public float bulletLifetime = 1f; //время до уничтожения пули (визуал)
        public float bulletSpeed = 1500f; //скорость полета пули (визуал)
        public Sprite icon; //иконка оружия (для килл-листа)
        public float explosiveRange; //радиус взрыва
        public float explolsionTime; //время, которое будет отображаться взрыв
    }
}

