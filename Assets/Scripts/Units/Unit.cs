using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MyTonaShooterTest.Weapons;

namespace MyTonaShooterTest.Units
{
    public class Unit : MonoBehaviour
    {
        public float moveSpeed = 10; //скорость передвижения
        public CharacterController characterController;
        public WeaponHolder weaponHolder;
        public Rigidbody rb;
        public int health = 100;
        public Slider healthBar; //hp бар над ботом
        public bool isBot = false; //юнит является ботом
        public GameObject[] spawnWeapons; //оружие, которое юнит получает при спавне
        public float deadBodyLifetime = 3f; //время, которое будет существовать труп после смерти юнита
        public float respawnTime = 5f; //задержка перед респавном юнита после смерти

        int _teamid;

        public int teamid
        {
            get => _teamid;
        }

        public void SetTeam(int teamid)
        {
            _teamid = teamid;
        }

        // Update is called once per frame
        void Update()
        {
            //управление игроком
            if (!this.isBot)
            {
                CheckPlayerInputs();
            }

            //чтобы hp бар над головой был напротив камеры
            if (healthBar != null)
            {
                healthBar.gameObject.transform.LookAt(Camera.main.transform.position);
                healthBar.gameObject.transform.Rotate(0f, 180f, 0f);
            }


        }

        /// <summary>
        /// обрабатывает нажатия клавиш игрока
        /// </summary>
        void CheckPlayerInputs()
        {
            if (!isAlive) return;
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

            //смена оружия
            float mouseScroll = Input.GetAxis("Mouse ScrollWheel");
            if (mouseScroll != 0f)
            {
                if (mouseScroll > 0) weaponHolder.SelectNextWeapon();
                else weaponHolder.SelectPrevWeapon();
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
            UpdateHealthBar();//Health UI
            if (health <= 0) Die();
        }

        //обновление здоровья в UI
        public void UpdateHealthBar()
        {
            if (!isBot) ScreenGUI.instance.UpdateHealthBar(health); //обновление hp на экране
            //обновление hp в баре над юнитом
            if (healthBar != null)
            {
                healthBar.value = health / 100f;
            }
        }

        /// <summary>
        /// срабатывает в момент смерти персонажа
        /// </summary>
        public void Die()
        {
            characterController.enabled = false;
            this.enabled = false;
            weaponHolder.gameObject.SetActive(false);
            rb.velocity = Vector3.zero;
            Destroy(gameObject, deadBodyLifetime);
            UnitsHolder.units.Remove(this);

            //Респавн
            if (isBot)
            {
                Spawner.instance.SpawnUnit(Spawner.instance.botPrefab, teamid, respawnTime);
            }
        }

        public bool isAlive
        {
            get => (health > 0) ? true : false;
        }



    }

}