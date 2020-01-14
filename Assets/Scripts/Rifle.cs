using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rifle : Weapon
{

    // Update is called once per frame
    void Update()
    {
        
    }

    protected override void Shot()
    {
        base.Shot();

        Ray ray = new Ray(shotOrigin.position, shotOrigin.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.gameObject.CompareTag("Unit"))
            {
                hit.transform.gameObject.GetComponent<Unit>().TakeDamage(_weaponOwner, this, weaponData.damage);
            }
        }

    }
}
