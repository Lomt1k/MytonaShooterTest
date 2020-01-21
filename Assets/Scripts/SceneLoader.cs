using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    private static SceneLoader _insance;
    private GameModeType _gameModeType;
    private int _players;
    private int _time;

    public static SceneLoader instance => _insance;
    public GameModeType gameModeType => _gameModeType;
    public int players => _players;
    public int time => _time;

    public void StartGame(GameModeType gm, int players, int time)
    {
        _gameModeType = gm;
        _players = players;
        _time = time;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    private void Start()
    {
        _insance = this;
        DontDestroyOnLoad(gameObject);
    }

}
