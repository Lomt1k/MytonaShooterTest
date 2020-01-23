using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyTonaShooterTest.Units;
using MyTonaShooterTest.UI;

namespace MyTonaShooterTest.Weapons
{
    public class WeaponHolder : MonoBehaviour
    {
        public Unit _weaponOwner; //юнит, которому принадлежит оружие
        public Transform weaponPos;
        public GrenadeTrajectoryCalc grenadeController;

        private List<Weapon> _weapons;
        private List<Grenade> _grenades;
        private Weapon _armedWeapon;
        private Grenade _armedGrenade;

        public Weapon armedWeapon => _armedWeapon;
        public Grenade armedGrenade => _armedGrenade;

        public int grenadeCount => _grenades.Count;

        // Выдает оружие юниту
        public void AddWeapon(GameObject weaponPrefab, bool setArmedWeapon = true)
        {
            if (weaponPrefab.GetComponent<Weapon>() == null)
            {
                return;
            }

            GameObject weaponObj = Instantiate(weaponPrefab, weaponPos.position, weaponPos.rotation, transform);
            Weapon weapon = weaponObj.GetComponent<Weapon>();
            weapon.gameObject.SetActive(false);
            weapon.SetOwner(_weaponOwner);          
            
            if (weapon is Grenade)
            {
                _grenades.Add((Grenade)weapon);
            }
            else
            {
                _weapons.Add(weapon);
                if (setArmedWeapon == true)
                {
                    SetArmedWeapon(weapon);
                }
            }
        }

        //выбрать следующее оружие
        public bool SelectNextWeapon()
        {
            if (_weapons.Count < 2)
            {
                return false;
            }
            int i = _weapons.IndexOf(armedWeapon) + 1;

            if (i >= _weapons.Count)
            {
                i = 0;
            }
            SetArmedWeapon(_weapons[i]);
            return true;
        }

        //выбрать предыдущее оружие
        public bool SelectPrevWeapon()
        {
            if (_weapons.Count < 2)
            {
                return false;
            }
            int i = _weapons.IndexOf(armedWeapon) - 1;
            if (i <= -1) i = _weapons.Count - 1;
            SetArmedWeapon(_weapons[i]);
            return true;
        }

        //экипировать оружие
        public void SetArmedWeapon(Weapon weapon)
        {           
            if (weapon == null)
            {
                return;
            }
            if (armedGrenade != null)
            {
                return;
            }
            if (_armedWeapon != null)
            {
                if (_armedWeapon.isReloading)
                {
                    return;
                }
                if (_armedWeapon.isShooting)
                {
                    return;
                }
                _armedWeapon.gameObject.SetActive(false);
            }

            _armedWeapon = weapon;
            _armedWeapon.gameObject.SetActive(true);
            if (!_weaponOwner.player.isBot)
            {
                StartCoroutine(UpdateGUI(0.1f)); //через карутин для избежания исключения
            }
        }

        public void TryShot()
        {
            if (armedGrenade != null)
            {
                armedGrenade.TryShot();
                return;
            }
            armedWeapon?.TryShot();
        }

        public void TryReload()
        {
            if (armedGrenade != null)
            {
                return;
            }
            armedWeapon?.TryReload();
        }

        public void GrenadeStartAiming()
        {
            if (_grenades.Count < 1 || (armedWeapon != null && armedWeapon.isReloading))
            {
                return;
            }
            //берем в руки гранату
            _armedGrenade = _grenades[0];
            _armedGrenade.gameObject.SetActive(true);
            grenadeController.StartAiming(_armedGrenade.weaponData.shotDistance, _armedGrenade.weaponData.explosiveRange);
            //временно убираем активное оружие
            if (armedWeapon != null)
            {
                armedWeapon.gameObject.SetActive(false);
            }            
        }

        public void GrenadeStopAiming()
        {
            if (armedGrenade == null)
            {
                return;
            }
            //убираем гранату
            grenadeController.StopAiming();
            _armedGrenade.gameObject.SetActive(false);
            _armedGrenade = null;
            //возвращаем выбранное оружие (если оно было)
            if (armedWeapon != null)
            {
                armedWeapon.gameObject.SetActive(true);
            }     
        }

        public void OnGrenadeDraw()
        {
            grenadeController.StopAiming();
            _grenades.Remove(_armedGrenade);
            _armedGrenade = null;
            //возвращаем выбранное оружие (если оно было)
            if (armedWeapon != null)
            {
                armedWeapon.gameObject.SetActive(true);
            }
        }

        protected IEnumerator UpdateGUI(float delay = 0f)
        {
            yield return new WaitForSeconds(delay);        
            ScreenGUI.instance.UpdateAmmoText(_weaponOwner);
        }

        private void Start()
        {
            _weapons = new List<Weapon>();
            _grenades = new List<Grenade>();

            //выдаём юниту оружие, которое у него прописано в классе Unit
            foreach (var weapon in _weaponOwner.spawnWeapons)
            {
                AddWeapon(weapon);
            }
        }

    }
}