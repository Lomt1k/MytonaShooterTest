using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHolder : MonoBehaviour
{
    Weapon _armedWeapon; 

    public Weapon armedWeapon
    {
        get => _armedWeapon;
        private set => _armedWeapon = value;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        //hardcode временно
        armedWeapon = transform.Find("Rifle").GetComponent<Rifle>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
