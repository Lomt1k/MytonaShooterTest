using System;
using UnityEngine;

namespace MyTonaShooterTest.UI
{
    public class KillList : MonoBehaviour
    {
        public KillListEvent[] killListEvents;
        public int messageLifetime = 5;

        private static KillList _instance;
        private int _lastActiveEvent = -1;

        public static KillList instance => _instance;

        public void ShowDeathMessage(string victim, Sprite icon = null, string killer = null)
        {
            if (killer == victim)
            {
                killer = null; //если убил сам себя - поле убийцы пустое
            }

            _lastActiveEvent++;
            if (_lastActiveEvent >= killListEvents.Length)
            {
                _lastActiveEvent = killListEvents.Length - 1;
                //перемещаем прежние сообщение на 1 вверх
                for (int i = 0; i < _lastActiveEvent; i++)
                {
                    killListEvents[i].SetMessage(
                        killListEvents[i + 1].victimText.text, 
                        killListEvents[i + 1].weaponIcon.sprite, 
                        killListEvents[i + 1].killerText.text,
                        killListEvents[i + 1].lifetime
                     );
                }
            }
            killListEvents[_lastActiveEvent].SetMessage(victim, icon, killer, messageLifetime);
        }

        //удаление самого верхнего события из килл - листа
        public void OnOlderEvent()
        {
            killListEvents[0].gameObject.SetActive(false);           

            //перемещаем последующие сообщения (если они были) на 1 вверх
            for (int i = 0; i < _lastActiveEvent; i++)
            {
                killListEvents[i].SetMessage(
                    killListEvents[i + 1].victimText.text,
                    killListEvents[i + 1].weaponIcon.sprite,
                    killListEvents[i + 1].killerText.text,
                    killListEvents[i + 1].lifetime
                );
            }
            if (_lastActiveEvent < killListEvents.Length)
            {
                killListEvents[_lastActiveEvent].gameObject.SetActive(false);
            }
            _lastActiveEvent--;
        }

        private void Start()
        {
            _instance = this;
        }

    }
}