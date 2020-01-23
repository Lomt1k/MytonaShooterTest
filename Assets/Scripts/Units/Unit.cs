using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using MyTonaShooterTest.Weapons;
using MyTonaShooterTest.UI;

namespace MyTonaShooterTest.Units
{
    public class Unit : MonoBehaviour
    {
        public CharacterController characterController;
        public WeaponHolder weaponHolder;
        public float health = 100f;
        public Slider healthBar; //hp бар над ботом
        public Image healthBarFill; 
        public GameObject[] spawnWeapons; //оружие, которое юнит получает при спавне
        public float respawnTime = 5f; //время до респавна после смерти (у ботов)
        public float deadBodyForce = 100f; //сила, применяемая к Rigidbody в момент смерти юнита (для падения юнита по физике)
        public UnitStats unitStats; //класс, который хранит и обрабатывает статы игрока
        public UnitStatsData defaultUnitStats; //Scriptable Object класс, из которого подгружаются статы по умолначию
        public Transform crosshair; //прицел

        private Player _player; //игрок, которому принадлежит юнит

        public Player player => _player;

        public bool isAlive
        {
            get => (health > 0) ? true : false;
        }

        public void SetPlayer(Player player)
        {
            _player = player;
        }

        /// <summary>
        /// обрабатывает входящий урон
        /// </summary>
        /// <param name="attacker">юнит, от которого получен урон</param>
        /// <param name="weapon">оружие, при помощи которого нанесен урон</param>
        /// <param name="damage">количество полученного урона</param>
        public void TakeDamage(Unit attacker, Weapon weapon, float damage)
        {
            if (health <= 0)
            {
                return;
            }
            //применяем модификаторы урона
            damage *= attacker.unitStats.damageMult.value * unitStats.incomingDamageMult.value;
            health -= damage;
            UpdateHealthBar();//Health UI
            if (health <= 0)
            {
                Die(attacker, weapon);
            }
        }

        //обновление здоровья в UI
        public void UpdateHealthBar()
        {
            if (!player.isBot)
            {
                ScreenGUI.instance.UpdateHealthBar(health); //обновление hp на экране
            }
            //обновление hp в баре над юнитом
            if (healthBar != null)
            {
                healthBar.value = health / 100f;
            }
        }

        // срабатывает в момент смерти персонажа
        public void Die(Unit attacker = null, Weapon weapon = null)
        {
            //отключаем лишние компоненты
            characterController.enabled = false;
            this.enabled = false;
            weaponHolder.gameObject.SetActive(false);
            NavMeshAgent agent = GetComponent<NavMeshAgent>();
            if (agent != null)
            {
                agent.enabled = false;
            }
            if (healthBar != null)
            {
                healthBar.gameObject.SetActive(false);
            }
            //добавляем RiridBody для падения трупа по физике
            Rigidbody rb = gameObject.AddComponent<Rigidbody>();
            rb.AddForce(-transform.forward * deadBodyForce);

            //отправляем игровому режиму информацию о смерти (убийстве)
            if (attacker == null)
            {
                GameManager.instance.gameMode.OnPlayerDeath(player);
            }
            else
            {
                GameManager.instance.gameMode.OnPlayerDeath(player, attacker.player, weapon);
            }

            //респавн
            if (player.isBot)
            {
                StartCoroutine(RequestRespawnAtTime(respawnTime));
            }
            else
            {
                ScreenGUI.instance.ShowDeathMenu();
            }
        }

        private void Start()
        {
            if (!player.isBot)
            {
                ScreenGUI.instance.UpdateHealthBar(health);
                healthBar.gameObject.SetActive(false);
                crosshair.gameObject.SetActive(true);
            }
            gameObject.name = "unit " + player.nickname;
            //инициализируем unitStats, подгружая в него значения статов по умолчанию из defaultUnitStats
            unitStats = new UnitStats(defaultUnitStats);
            unitStats.SetOwner(this);

            //применяем логику спавна из конкретного игрового режима
            GameManager.instance.gameMode.OnPlayerSpawn(player);

            //зеленый цвет hp бара у союзников
            if (player.teamID == Player.mine.teamID)
            {
                healthBarFill.color = Color.green;
            }
        }

        // Update is called once per frame
        private void Update()
        {
            //управление игроком
            if (!player.isBot)
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
        private void CheckPlayerInputs()
        {
            if (!isAlive)
            {
                return;
            }
            //передвижение
            Vector3 moveDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));
            characterController.Move(moveDir * unitStats.moveSpeed.value * Time.deltaTime);

            //поворот игрока в сторону курсора
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                Vector3 lookPos = new Vector3(hit.point.x, transform.position.y, hit.point.z);
                transform.LookAt(lookPos);
                crosshair.transform.position = lookPos;
            }

            //стрельба из оружия
            if (Input.GetMouseButton(0))
            {
                weaponHolder.TryShot();
            }

            //перезарядка
            if (Input.GetKeyDown(KeyCode.R))
            {
                weaponHolder.TryReload();
            }

            //смена оружия
            float mouseScroll = Input.GetAxis("Mouse ScrollWheel");
            if (mouseScroll != 0f)
            {
                if (mouseScroll > 0)
                {
                    weaponHolder.SelectNextWeapon();
                }
                else
                {
                    weaponHolder.SelectPrevWeapon();
                }
            }

            //гранаты
            if (Input.GetKeyDown(KeyCode.G))
            {
                weaponHolder.GrenadeStartAiming();
            }
            else if (Input.GetKeyUp(KeyCode.G))
            {
                weaponHolder.GrenadeStopAiming();
            }

        }

        private void OnDestroy()
        {
            //удаляем юнита из списка юнитов
            UnitsHolder.units.Remove(this);
        }

        private IEnumerator RequestRespawnAtTime(float time)
        {
            yield return new WaitForSeconds(time);
            player.Respawn();
        }



    }

}