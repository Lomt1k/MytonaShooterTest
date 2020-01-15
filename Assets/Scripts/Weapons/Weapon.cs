using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyTonaShooterTest.Units;

namespace MyTonaShooterTest.Weapons
{
        public abstract class Weapon : MonoBehaviour
        {
            public WeaponData weaponData;
            public Transform shotOrigin;
            public Animator animator;

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
                if (_ammo <= 0)
                {
                    TryReload();
                    return false;
                }
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
                if (!_weaponOwner.isBot) ScreenGUI.instance.UpdateAmmoText(_weaponOwner);
            }

            /// <summary>
            /// вызывает попытку перезарядить оружие
            /// </summary>
            /// <returns>Возвращает true в случае, если началась перезарядка</returns>
            public virtual bool TryReload()
            {
                if (isReloading) return false;
                if (_ammo >= weaponData.magazineAmount) return false;
                StartCoroutine(Reload());
                return true;
            }

            /// <summary>
            /// перезаряжает оружие
            /// </summary>
            protected virtual IEnumerator Reload()
            {
                isReloading = true;
                if (!_weaponOwner.isBot) ScreenGUI.instance.UpdateAmmoText(_weaponOwner);
                if (animator != null) animator.SetTrigger("StartReload");
                yield return new WaitForSeconds(weaponData.reloadTime);
                isReloading = false;
                Ammo = weaponData.magazineAmount;
                if (!_weaponOwner.isBot) ScreenGUI.instance.UpdateAmmoText(_weaponOwner);
                if (animator != null) animator.SetTrigger("EndReload");
            }

            public void SetOwner(Unit owner)
            {
                _weaponOwner = owner;
            }
        }


}

