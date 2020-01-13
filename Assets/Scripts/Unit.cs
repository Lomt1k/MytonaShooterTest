using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public float moveSpeed = 10; //скорость передвижения
    public CharacterController characterController;
    public WeaponHolder weaponHolder;
    public int health = 100;
    public bool isBot = false; //юнит является ботом

    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //управление игроком
        if (!this.isBot)
        {
            CheckPlayerInputs();
        }
        
        
    }

    /// <summary>
    /// обрабатывает нажатия клавиш игрока
    /// </summary>
    void CheckPlayerInputs()
    {
        //передвижение
        Vector3 moveDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));
        characterController.Move(moveDir * moveSpeed * Time.deltaTime);

        //поворот игрока в сторону курсора
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            Vector3 lookPos = new Vector3(hit.point.x, transform.position.y, hit.point.z);
            transform.LookAt(lookPos);
        }

        //стрельба из оружия
        if (Input.GetMouseButton(0))
        {
            weaponHolder.armedWeapon?.TryShot();
        }

        //перезарядка
        if (Input.GetKeyDown(KeyCode.R))
        {
            weaponHolder.armedWeapon?.TryReload();
        }
    }

    /// <summary>
    /// обрабатывает входящий урон
    /// </summary>
    /// <param name="attacker">юнит, от которого получен урон</param>
    /// <param name="weapon">оружие, при помощи которого нанесен урон</param>
    /// <param name="damage">количество полученного урона</param>
    public void TakeDamage(Unit attacker, Weapon weapon, int damage)
    {
        if (health <= 0) return;
        health -= damage;
        print($"{attacker.gameObject.name} нанес {damage} единиц урона {gameObject.name}. Осталось HP: {health} ");
        if (health <= 0) Die();
    }

    /// <summary>
    /// срабатывает в момент смерти персонажа
    /// </summary>
    public void Die()
    {
        print(gameObject.name + " скончался");
    }



}
