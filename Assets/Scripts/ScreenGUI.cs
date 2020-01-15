using UnityEngine;
using UnityEngine.UI;
using MyTonaShooterTest.Weapons;
using MyTonaShooterTest.Units;

public class ScreenGUI : MonoBehaviour
{
    static ScreenGUI _instance;

    public static ScreenGUI instance
    {
        get => _instance;
    }

    public Slider healthBar;
    public Text ammoText;

    // Start is called before the first frame update
    void Start()
    {
        _instance = this;
    }

    public void UpdateHealthBar(int hp)
    {
        healthBar.value = hp / 100f;
    }
    

    public void UpdateAmmoText(Unit unit)
    {
        Weapon weap = unit.weaponHolder.armedWeapon;
        if (weap != null)
        {
            if (weap.isReloading)
            {
                ammoText.text = "<color=red>[ПЕРЕЗАРЯДКА]</color>";
            }
            else
            {
                ammoText.text = $"{weap.Ammo} / {weap.weaponData.magazineAmount}";
            }
        }
        else ammoText.text = "";
    }

}
