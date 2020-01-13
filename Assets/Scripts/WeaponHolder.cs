using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHolder : MonoBehaviour
{
    Weapon _armedWeapon;

    protected Unit _weaponOwner; //юнит, которому принадлежит оружие

    public Weapon armedWeapon
    {
        get => _armedWeapon;
        private set => _armedWeapon = value;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        _weaponOwner = transform.GetComponentInParent<Unit>();
        StartCoroutine(UpdateGUI(0.1f)); //через карутин для избежания исключения

        //hardcode временно
        armedWeapon = transform.Find("Rifle").GetComponent<Rifle>();
    }

    protected IEnumerator UpdateGUI(float delay = 0f)
    {
        yield return new WaitForSeconds(delay);
        ScreenGUI.instance.UpdateAmmoText(_weaponOwner);
    }

}
