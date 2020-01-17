using System;
using UnityEngine;
using UnityEngine.UI;

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

    private GameModeType _selectedGameMode;

    public void ButtonDM()
    {
        _selectedGameMode = GameModeType.DeathMatch;
    }

    public void ButtonTDM()
    {
        _selectedGameMode = GameModeType.TeamDeathMatch;
    }

    public void PlayButton()
    {
        //players
        int players = 1;
        if (!String.IsNullOrEmpty(playersField.text))
        {
            players = Convert.ToInt32(playersField.text);
        }        
        if (players < 1) players = 1;
        else if (players > 8) players = 8;
        //time
        int time = 30;
        if (!String.IsNullOrEmpty(timeField.text))
        {
            time = Convert.ToInt32(timeField.text);
        }
        if (time < 30) time = 30;
        else if (time > 900) time = 900;

        sceneLoader.StartGame(_selectedGameMode, players, time);
    }


    public void ExitButton()
    {
        Application.Quit();
    }

    private void Start()
    {
        buttonDM.Select();
    }
}
