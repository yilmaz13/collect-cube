using TMPro;
using UnityEngine;

namespace UI
{
    public class GamePlayUI : ModalUI
    {       
        public GameObject Panel;       
        public Joystick Joystick;
        public TextMeshProUGUI scorePlayerText;
        public TextMeshProUGUI scoreEnemyText;
        public TextMeshProUGUI timeText;
        public TextMeshProUGUI levelText;       

        public override void OnActivate()
        {
            Panel.SetActive(true);
        }

        public override void OnDeactivate()
        {
            Panel.SetActive(false);
        }       
    }
}