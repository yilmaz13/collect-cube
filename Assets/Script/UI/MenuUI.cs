using UnityEngine;
using UnityEngine.UI;
using Managers;

public class MenuUI : MonoBehaviour
{
    #region Public Value
 
    public Button playButton;      

    #endregion

    #region private Methods
    private void Start()
    {
        SetButton();      
    }         
       
    private void SetButton()
    {
        playButton.onClick.AddListener(GameManager.Instance.StartGame);           
    }

    #endregion

    #region public Methods

    public void OnActivate()
    {      
        playButton.interactable = true;
    }

    #endregion
}