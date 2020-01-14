using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        StartCoroutine(UpdateGUI(0.1f)); //через карутин для избежания исключения

        //выдаём юниту оружие, которое у него прописано в классе Unit
        foreach (var weapon in _weaponOwner.weapons)
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
        if (setArmedWeapon == true) armedWeapon = weapon;
    }

    protected IEnumerator UpdateGUI(float delay = 0f)
    {
        yield return new WaitForSeconds(delay);
        ScreenGUI.instance.UpdateAmmoText(_weaponOwner);
    }

}
