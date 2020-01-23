using System.Collections;
using UnityEngine;
using MyTonaShooterTest.Units;
using MyTonaShooterTest.Weapons;
using MyTonaShooterTest.UI;

public class PowerUp : MonoBehaviour
{
    public Ability ability; //способность, которая выдается при подборе powerUp (если не null)
    public GameObject weapon; //оружие, которое выдается при подборе пикапа (если не null)
    public float respawnTime = 30f; //время респавна powerUp после его подбора

    public Collider col;
    public MeshRenderer meshRenderer;

    private void OnTriggerEnter(Collider other)
    {
        Unit unit = other.GetComponent<Unit>();
        if (unit != null)
        {
            StartCoroutine(OnPickup(unit, respawnTime));
        }
    }

    private IEnumerator OnPickup(Unit unit, float time)
    {
        if (ability != null)
        {
            unit.unitStats.AddAbility(ability);
        }
        if (weapon != null && weapon.gameObject.GetComponent<Weapon>() != null)
        {
            unit.weaponHolder.AddWeapon(weapon);
            if (!unit.player.isBot)
            {
                ScreenGUI.instance.UpdateAmmoText(unit);
            }
        }
        
        //исчезновение и респавн
        col.enabled = false;
        meshRenderer.enabled = false;
        yield return new WaitForSeconds(time);
        col.enabled = true;
        meshRenderer.enabled = true;
    }
}
