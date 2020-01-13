using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public float moveSpeed = 10; //скорость передвижения

    bool _isBot = false; //юнит под управлением игрока

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!this._isBot)
        {
            Vector3 moveDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));
            transform.position += moveDir * moveSpeed * Time.deltaTime;
        }
        
        
    }
}
