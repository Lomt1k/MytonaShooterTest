using System;
using UnityEngine;
using UnityEngine.UI;
using MyTonaShooterTest.Languages;

public enum GameModeType : byte
{
    DeathMatch,
    TeamDeathMatch
}

public class MainMenuUI : MonoBehaviour
{
    public SceneLoader sceneLoader;
    public Button buttonDM;
    public Button buttonTDM;
    public InputField playersField;
    public InputField timeField;
    public Color selectedColor;
    public Image langButtonImage;
    public Sprite langImageEng;
    public Sprite langImageRus;

    private GameModeType _selectedGameMode;

    public void ButtonDM()
    {
        _selectedGameMode = GameModeType.DeathMatch;
        buttonDM.image.color = selectedColor;
        buttonTDM.image.color = Color.white;
    }

    public void ButtonTDM()
    {
        _selectedGameMode = GameModeType.TeamDeathMatch;
        buttonTDM.image.color = selectedColor;
        buttonDM.image.color = Color.white;
    }

    public void PlayButton()
    {
        //players
        int players = 4;
        if (!String.IsNullOrEmpty(playersField.text))
        {
            players = Convert.ToInt32(playersField.text);
        }
        players = Mathf.Clamp(players, 1, 12);
        //time
        int time = 30;
        if (!String.IsNullOrEmpty(timeField.text))
        {
            time = Convert.ToInt32(timeField.text);
        }
        time = Mathf.Clamp(time, 30, 900);

        sceneLoader.StartGame(_selectedGameMode, players, time);
    }


    public void ExitButton()
    {
        Application.Quit();
    }

    public void SwitchLanguage()
    {
        switch (PlayerPrefs.GetString("Language", "English"))
        {
            case "Russian":
                Language.ChangeLanguage("English");
                break;
            case "English":
                Language.ChangeLanguage("Russian");
                break;

            default:
                Language.ChangeLanguage("English");
                break;
        }

    }

    private void Start()
    {
        _selectedGameMode = GameModeType.DeathMatch;
        buttonDM.image.color = selectedColor;

        switch (PlayerPrefs.GetString("Language", "English"))
        {
            case "Russian":
                langButtonImage.sprite = langImageRus;
                break;
            default:
                langButtonImage.sprite = langImageEng;
                break;
        }
    }
}
