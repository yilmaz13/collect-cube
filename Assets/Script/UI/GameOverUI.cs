using UnityEngine;
using UnityEngine.UI;

public class GameOverUI : ModalUI
{
    public GameObject Panel;
    public Button ReStartButton;
    public Button HomeButton;    

    public override void OnActivate()
    {     
        Panel.SetActive(true);
    }

    public override void OnDeactivate()
    {
        Panel.SetActive(false);
    }   
}