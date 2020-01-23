using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace MyTonaShooterTest.UI
{
    public class KillListEvent : MonoBehaviour
    {
        public Text killerText;
        public Image weaponIcon;
        public Text victimText;

        private int _lifetime;

        public int lifetime => _lifetime;

        public void SetMessage(string victim, Sprite icon = null, string killer = null, int lifetime = 10)
        {
            victimText.text = victim;
            weaponIcon.sprite = icon;
            if (icon == null)
            {
                weaponIcon.gameObject.SetActive(false);
            }
            else
            {                
                weaponIcon.gameObject.SetActive(true);
            }            
            killerText.text = killer;
            _lifetime = lifetime;
            gameObject.SetActive(true);
        }

        private void Start()
        {
            StartCoroutine(SecUpdate());
        }

        private IEnumerator SecUpdate()
        {
            while (true)
            {
                yield return new WaitForSeconds(1f);
                if (lifetime > 0)
                {
                    _lifetime--;
                    if (lifetime <= 0)
                    {
                        KillList.instance.OnOlderEvent();
                    }
                }
            }
                     
        }
    }
}
