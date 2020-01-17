using System.Collections;
using UnityEngine;
using MyTonaShooterTest.VFX;

public class GameManager : MonoBehaviour
{
    public GameObject rifleBulletPrefab;

    private static GameManager _instance;
    private GameMode _gameMode;
    private PoolManager<Bullet> _rifleBulletPool;

    public static GameManager instance => _instance;
    public GameMode gameMode => _gameMode;
    public PoolManager<Bullet> rifleBulletPool => _rifleBulletPool;

    

    private void Start()
    {
        _instance = this;
        //инициализируем bulletPool
        _rifleBulletPool = new PoolManager<Bullet>(rifleBulletPrefab, 5);

        StartCoroutine(InitGameMode(1));
    }

    private IEnumerator InitGameMode(float time)
    {
        yield return new WaitForSeconds(time);
        switch (SceneLoader.instance.gameModeType)
        {
            case GameModeType.DeathMatch:
                _gameMode = new DeathMatch(SceneLoader.instance.players, SceneLoader.instance.time);
                break;
            case GameModeType.TeamDeathMatch:
                _gameMode = new TeamDeathMatch(SceneLoader.instance.players, SceneLoader.instance.time);
                break;
        }
        
    }

}
