using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using MyTonaShooterTest.Weapons;

namespace MyTonaShooterTest.Units
{
    public class Unit : MonoBehaviour
    {
        public CharacterController characterController;
        public WeaponHolder weaponHolder;
        public float health = 100f;
        public Slider healthBar; //hp бар над ботом
        public bool isBot = false; //юнит является ботом
        public GameObject[] spawnWeapons; //оружие, которое юнит получает при спавне
        public float deadBodyLifetime = 3f; //время, которое будет существовать труп после смерти юнита
        public float deadBodyForce = 100f; //сила, применяемая к Rigidbody в момент смерти юнита (для падения юнита по физике)
        public float respawnTime = 5f; //задержка перед респавном юнита после смерти
        public UnitStats unitStats; //класс, который хранит и обрабатывает статы игрока
        public UnitStatsData defaultUnitStats; //Scriptable Object класс, из которого подгружаются статы по умолначию

        int _teamid;

        public int teamid
        {
            get => _teamid;
        }

        public void SetTeam(int teamid)
        {
            _teamid = teamid;
        }

        void Start()
        {
            if (!isBot) ScreenGUI.instance.UpdateHealthBar(health);
            //инициализируем unitStats, подгружая в него значения статов по умолчанию из defaultUnitStats
            unitStats = new UnitStats(defaultUnitStats);
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
            characterController.Move(moveDir * unitStats.moveSpeed * Time.deltaTime);

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
        public void TakeDamage(Unit attacker, Weapon weapon, float damage)
        {
            //применяем модификаторы урона
            damage *= attacker.unitStats.dagameMultiplicator * unitStats.incomingDamageMultiplicator;

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

        // срабатывает в момент смерти персонажа
        public void Die()
        {
            //отключаем лишние компоненты
            characterController.enabled = false;
            this.enabled = false;
            weaponHolder.gameObject.SetActive(false);
            NavMeshAgent agent = GetComponent<NavMeshAgent>();
            if (agent != null) agent.enabled = false;
            if (healthBar != null) healthBar.gameObject.SetActive(false);
            //добавляем RiridBody для падения трупа по физике
            Rigidbody rb = gameObject.AddComponent<Rigidbody>();
            rb.AddForce(-transform.forward * deadBodyForce);
            Destroy(gameObject, deadBodyLifetime);
            if (!isBot) ScreenGUI.instance.storedPlayerTeamid = teamid; //не лучший способ передачи teamid для респавна игрока..
            //удаляем юнита из списка юнитов
            UnitsHolder.units.Remove(this);

            //Респавн
            if (isBot) Spawner.instance.SpawnUnit(Spawner.instance.botPrefab, teamid, respawnTime);
            else ScreenGUI.instance.ShowDeathMenu();
        }

        public bool isAlive
        {
            get => (health > 0) ? true : false;
        }



    }

}