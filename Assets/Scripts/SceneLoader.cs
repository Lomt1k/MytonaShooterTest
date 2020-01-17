using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    private static SceneLoader _insance;
    private GameModeType _gameModeType;
    private int _players;
    private float _time;

    public static SceneLoader instance => _insance;
    public GameModeType gameModeType => _gameModeType;
    public int players => _players;
    public float time => _time;

    public void StartGame(GameModeType gm, int players, int time)
    {
        _gameModeType = gm;
        _players = players;
        _time = (int)time;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    void Start()
    {
        _insance = this;
        DontDestroyOnLoad(gameObject);
    }

}
