using UnityEngine;
using UnityEngine.UI;

namespace MyTonaShooterTest.Languages
{
    public class LangLoader_Menu : MonoBehaviour
    {
        public Text inputFieldPlayers;
        public Text inputFieldTime;
        public Text playButton;

        // Start is called before the first frame update
        void Start()
        {
            inputFieldPlayers.text = Language.data["menu_inputFieldPlayers"];
            inputFieldTime.text = Language.data["menu_inputFieldTime"];            
            playButton.text = Language.data["menu_playButton"];
        }
    }
}