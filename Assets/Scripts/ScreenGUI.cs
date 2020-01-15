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
    public GameObject deathPanel;

    public int storedPlayerTeamid; //сюда кладется teamid игрока при смерти, который присваивается игроку при респавне

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

    public void ShowDeathMenu()
    {
        deathPanel.SetActive(true);
        healthBar.gameObject.SetActive(false);
        ammoText.gameObject.SetActive(false);
    }

    public void RespawnButtonClick()
    {
        deathPanel.SetActive(false);
        healthBar.gameObject.SetActive(true);
        ammoText.gameObject.SetActive(true);
        Spawner.instance.SpawnUnit(Spawner.instance.playerPrefab, storedPlayerTeamid);
    }

}
