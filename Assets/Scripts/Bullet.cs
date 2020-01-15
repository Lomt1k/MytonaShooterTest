﻿using UnityEngine;

public class Bullet : MonoBehaviour
{

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Weapon")) return;
        Destroy(gameObject);
    }
}
