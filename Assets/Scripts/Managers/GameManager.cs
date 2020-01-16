using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyTonaShooterTest.VFX;

public class GameManager : MonoBehaviour
{
    public GameObject rifleBulletPrefab;

    private static GameManager _instance;
    private PoolManager<Bullet> _rifleBulletPool;

    public PoolManager<Bullet> rifleBulletPool => _rifleBulletPool;

    public static GameManager instance => _instance;

    private void Start()
    {
        _instance = this;
        //инициализируем bulletPool
        _rifleBulletPool = new PoolManager<Bullet>(rifleBulletPrefab, 5);
    }

}
