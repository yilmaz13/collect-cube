using States;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class GameWinUI : ModalUI
{
    public GameObject Panel;
    public Button NextLevelButton;
    public Button HomeButton;
    public GameObject Score;
    public GameObject Stars;

    public Sprite winImages;

    public float fadeTime = 1f;
    public CanvasGroup canvasGroup;
    public RectTransform rectTransfrom;

    public void Start()
    {
        SetButton();
    }

    public override void OnActivate()
    {
        Panel.SetActive(true);
        canvasGroup.alpha = 0f;
        canvasGroup.DOFade(1, fadeTime);
    }

    public override void OnDeactivate()
    {
        Panel.SetActive(false);
        canvasGroup.DOFade(0, fadeTime);
    }

    public void SetButton()
    {
        NextLevelButton.onClick.AddListener(GameWinState.Instance.NextLevel);
    }
}