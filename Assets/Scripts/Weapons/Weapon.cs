using System.Collections;
using UnityEngine;
using MyTonaShooterTest.Units;
using MyTonaShooterTest.UI;

namespace MyTonaShooterTest.Weapons
{
    public abstract class Weapon : MonoBehaviour
    {
        public WeaponData weaponData;
        public Transform shotOrigin;
        public Animator animator;

        private int _ammo; //текущее количество патронов в обойме
        private float _lastShotTime; //момент, когда был произведен последний выстрел
        private bool _isReloading = false;
        private bool _isShooting = false;

        protected Unit _weaponOwner; //юнит, которому принадлежит оружие

        public int Ammo => _ammo;
        public bool isReloading => _isReloading;
        public bool isShooting => _isShooting;

        public void SetOwner(Unit owner)
        {
            _weaponOwner = owner;
        }

        /// <summary>
        /// вызывает попытку выстрелить из оружия
        /// </summary>
        /// <returns>Возвращает true в случае успешного выстрела</returns>
        public virtual bool TryShot()
        {
            if (isReloading)
            {
                return false;
            }
            if (_ammo <= 0)
            {
                TryReload();
                return false;
            }
            if (Time.time < 1f / (weaponData.fireRate * _weaponOwner.unitStats.attackSpeed.value) + _lastShotTime)
            {
                return false;
            }
            Shot();
            return true;
        }

        /// <summary>
        /// вызывает попытку перезарядить оружие
        /// </summary>
        /// <returns>Возвращает true в случае, если началась перезарядка</returns>
        public virtual bool TryReload()
        {
            if (isReloading || _ammo >= weaponData.magazineAmount)
            {
                return false;
            }
            StartCoroutine(Reload());
            return true;
        }

        protected virtual void OnStart()
        {
            _ammo = weaponData.magazineAmount;
        }

        /// <summary>
        /// выстрел из оружия
        /// </summary>
        protected virtual void Shot()
        {
            _ammo--;
            _lastShotTime = Time.time;
            if (isShooting == false)
            {
                StartCoroutine(CheckForStootingEnd());
                OnShootingStart();
            }
            if (!_weaponOwner.player.isBot)
            {
                ScreenGUI.instance.UpdateAmmoText(_weaponOwner);
            }
        }

        /// <summary>
        /// перезаряжает оружие
        /// </summary>
        protected virtual IEnumerator Reload()
        {
            _isReloading = true;
            if (!_weaponOwner.player.isBot)
            {
                ScreenGUI.instance.UpdateAmmoText(_weaponOwner);
            }
            if (animator != null)
            {
                animator.SetTrigger("StartReload");
            }
            yield return new WaitForSeconds(weaponData.reloadTime);
            _isReloading = false;
            _ammo = weaponData.magazineAmount;
            if (!_weaponOwner.player.isBot)
            {
                ScreenGUI.instance.UpdateAmmoText(_weaponOwner);
            }
            if (animator != null)
            {
                animator.SetTrigger("EndReload");
            }
        }

        /// <summary>
        /// когда игрок начинает стрелять
        /// </summary>
        protected virtual void OnShootingStart()
        {
            _weaponOwner.unitStats.AddAbility(weaponData.shootingAbility);
        }

        /// <summary>
        /// когда игрок завершил стрельбу (срабатывает если fireRate уже позволяет выстрелить снова, но игрок не стреляет)
        /// </summary>
        protected virtual void OnShootingEnd()
        {
            _weaponOwner.unitStats.RemoveAbility(weaponData.shootingAbility);
        }

        private void Start()
        {
            OnStart();
        }

        private IEnumerator CheckForStootingEnd()
        {
            _isShooting = true;
            while (_isShooting)
            {
                //умножаем на 1.1 потому что ждать нужно буквально чуть дольше, чем интервал между выстрелами при зажатой клавише
                yield return new WaitForSeconds((1 / weaponData.fireRate) * 1.1f);
                if (Time.time - _lastShotTime > 1 / weaponData.fireRate)
                {
                    _isShooting = false;
                    OnShootingEnd();
                }
            }            
        }

    }
}

