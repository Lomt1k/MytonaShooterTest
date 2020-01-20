using UnityEngine;
using UnityEngine.UI;
using MyTonaShooterTest.Weapons;
using MyTonaShooterTest.Units;

namespace MyTonaShooterTest.UI
{
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
        public Text killDeathText;
        public Text matchTime;
        public GameObject tdm_panel;
        public Text tdm_teamScoreA;
        public Text tdm_teamScoreB;

        public int storedPlayerTeamid; //сюда кладется teamid игрока при смерти, который присваивается игроку при респавне

        public void UpdateHealthBar(float hp)
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

        public void UpdatePlayerScore()
        {
            killDeathText.text = $"Kills: {Player.mine.kills} \nDeaths: {Player.mine.deaths}";
        }

        public void UpdateMatchTime(int time)
        {
            int minutes = time / 60;
            int seconds = time - minutes * 60;
            matchTime.text = minutes + ":" + ((seconds / 10 > 0) ? seconds.ToString() : "0" + seconds);   
        }

        public void UpdateTeamScore()
        {
            if (GameManager.instance.gameMode is TeamDeathMatch)
            {
                TeamDeathMatch tdm = (TeamDeathMatch)GameManager.instance.gameMode;
                tdm_teamScoreA.text = tdm.team_kills[0].ToString();
                tdm_teamScoreB.text = tdm.team_kills[1].ToString();
            }
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
            Player.mine.Respawn();
        }



        void Start()
        {
            _instance = this;
        }

    }
}
