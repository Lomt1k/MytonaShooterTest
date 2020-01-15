using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyTonaShooterTest.Units;

namespace MyTonaShooterTest.Weapons
{
        public class WeaponHolder : MonoBehaviour
        {
            List<Weapon> _weapons;
            Weapon _armedWeapon;

            public Unit _weaponOwner; //юнит, которому принадлежит оружие
            public Transform weaponPos;

            public Weapon armedWeapon
            {
                get => _armedWeapon;
                private set => _armedWeapon = value;
            }

            // Start is called before the first frame update
            void Start()
            {
                _weapons = new List<Weapon>();

                //выдаём юниту оружие, которое у него прописано в классе Unit
                foreach (var weapon in _weaponOwner.spawnWeapons)
                {
                    AddWeapon(weapon);
                }
            }

            // Выдает оружие юниту
            public void AddWeapon(GameObject weaponPrefab, bool setArmedWeapon = true)
            {
                if (weaponPrefab.GetComponent<Weapon>() == null) return;

                GameObject weaponObj = Instantiate(weaponPrefab, weaponPos.position, weaponPos.rotation, transform);
                Weapon weapon = weaponObj.GetComponent<Weapon>();
                weapon.SetOwner(_weaponOwner);
                _weapons.Add(weapon);
                if (setArmedWeapon == true) SetArmedWeapon(weapon);
            }

            //выбрать следующее оружие
            public bool SelectNextWeapon()
            {
                if (_weapons.Count < 2) return false;
                int i = _weapons.IndexOf(armedWeapon) + 1;
                if (i >= _weapons.Count) i = 0;
                SetArmedWeapon(_weapons[i]);
                return true;
            }

            //выбрать предыдущее оружие
            public bool SelectPrevWeapon()
            {
                if (_weapons.Count < 2) return false;
                int i = _weapons.IndexOf(armedWeapon) - 1;
                if (i <= -1) i = _weapons.Count - 1;
                SetArmedWeapon(_weapons[i]);
                return true;
            }

            //экипировать оружие
            public void SetArmedWeapon(Weapon weapon)
            {
                if (weapon == null) return;
                if (armedWeapon != null)
                {
                    if (armedWeapon.isReloading) return;
                    armedWeapon.gameObject.SetActive(false);
                }

                armedWeapon = weapon;
                armedWeapon.gameObject.SetActive(true);
                StartCoroutine(UpdateGUI(0.1f)); //через карутин для избежания исключения
            }

            protected IEnumerator UpdateGUI(float delay = 0f)
            {
                yield return new WaitForSeconds(delay);
                ScreenGUI.instance.UpdateAmmoText(_weaponOwner);
            }


        }

}