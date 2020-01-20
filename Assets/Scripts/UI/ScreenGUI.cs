using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using MyTonaShooterTest.Weapons;
using MyTonaShooterTest.Units;
using MyTonaShooterTest.Languages;

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
        public GameObject killDeathPanel;
        public Text killDeathText;
        public GameObject matchTimePanel;
        public Text matchTime;
        public GameObject tdm_panel;
        public Text tdm_teamScoreA;
        public Text tdm_teamScoreB;
        public GameObject results_panel;
        public Text results_title;
        public Text results_sub;
        public Text results_header_name;
        public Text results_header_kills;
        public Text results_header_deaths;
        public Text results_names_list;
        public Text results_kills_list;
        public Text reslts_deaths_list;
        public Text results_back_to_menu;

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
                    ammoText.text = $"<color=red>{Language.data["screenGUI_reloading"]}</color>";
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
            killDeathText.text = $"{Language.data["screenGUI_kills"]}: {Player.mine.kills} \n{Language.data["screenGUI_deaths"]}: {Player.mine.deaths}";
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

        public void ShowResultsMenu()
        {
            deathPanel.SetActive(false);
            healthBar.gameObject.SetActive(false);
            ammoText.gameObject.SetActive(false);
            matchTimePanel.SetActive(false);
            tdm_panel.SetActive(false);
            killDeathPanel.SetActive(false);
            results_panel.SetActive(true);
        }

        public void BackToMenuButton()
        {
            SceneManager.LoadScene("MainMenu");
        }



        void Start()
        {
            _instance = this;
        }

    }
}
