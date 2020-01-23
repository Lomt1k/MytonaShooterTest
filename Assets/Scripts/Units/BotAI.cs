using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace MyTonaShooterTest
{
    namespace Units
    {
        public class BotAI : MonoBehaviour
        {
            public NavMeshAgent agent;
            public Unit unit;
            public float viewDistance = 30f;
            public float enemyCheckTime = 0.5f;
            public LayerMask wallMask;

            private Unit targetEnemy;

            /// <summary>
            /// запускает следование бота к следующей точке на карте
            /// </summary>
            public void MoveToNextWayPoint()
            {
                int randomWay = Random.Range(0, Spawner.instance.unitSpawns.Length);
                agent.SetDestination(Spawner.instance.unitSpawns[randomWay].position);
            }

            private void Start()
            {
                if (!unit.player.isBot)
                {
                    agent.enabled = false;
                    this.enabled = false;
                    return;
                }

                MoveToNextWayPoint();
                StartCoroutine(CheckForEnemy(enemyCheckTime));
                unit.unitStats.ReloadStats();
            }


            private void Update()
            {
                MovementUpdate();

                if (targetEnemy != null && unit.isAlive)
                {
                    Vector3 lookPos = new Vector3(targetEnemy.transform.position.x, transform.position.y, targetEnemy.transform.position.z);
                    gameObject.transform.LookAt(lookPos);
                    unit.weaponHolder.armedWeapon.TryShot();
                }
            }

            private void MovementUpdate()
            {
                if (Vector3.Distance(transform.position, agent.destination) < 10f)
                {
                    MoveToNextWayPoint();
                }
            }

            private IEnumerator CheckForEnemy(float delay)
            {
                while (true)
                {
                    yield return new WaitForSeconds(delay);
                    //если противник для атаки уже выбран - проверяем что по нему еще можно атаковать (сбрасываем targetEnemy если нельзя)                      
                    if (targetEnemy != null)
                    {
                        if (!targetEnemy.isAlive || Vector3.Distance(targetEnemy.transform.position, transform.position) > viewDistance)
                        {
                            targetEnemy = null;
                        }
                        else
                        {
                            //проверяем, что ничего не мешает стрелять во врага
                            Ray ray = new Ray(transform.position, targetEnemy.transform.position);
                            RaycastHit hit;
                            if (Physics.Raycast(ray, out hit, wallMask))
                            {
                                targetEnemy = null;
                            }
                        }
                    }

                    if (targetEnemy == null)
                    {
                        FindEnemy();
                    }
                }


            }

            //ищет противника, доступного для атаки
            private bool FindEnemy()
            {
                float closestEnemyDistance = float.MaxValue;
                Unit closestEnemy = null;

                foreach (var unit in UnitsHolder.units)
                {
                    if (unit == this.unit || unit.player.teamID == this.unit.player.teamID)
                    {
                        continue;
                    }
                    if (Vector3.Distance(unit.transform.position, transform.position) > closestEnemyDistance)
                    {
                        continue; //уже найден доступный противник на более близкой дистанции
                    }
                    if (Vector3.Distance(unit.transform.position, transform.position) > viewDistance)
                    {
                        continue;
                    }

                    //проверяем, что ничего не мешает стрелять во врага
                    Ray ray = new Ray(transform.position, unit.transform.position);
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit, wallMask))
                    {
                        continue;
                    }
                    //записываем найденного юнита в ближайшего для атаки
                    closestEnemy = unit;
                    closestEnemyDistance = Vector3.Distance(unit.transform.position, transform.position);
                }
                if (closestEnemy == null)
                {
                    return false;
                }
                targetEnemy = closestEnemy;
                return true;
            }

        }

    }
}