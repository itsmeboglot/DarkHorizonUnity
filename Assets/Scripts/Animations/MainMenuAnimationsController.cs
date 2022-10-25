using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class MainMenuAnimationsController : MonoBehaviour
{
    [Header("Welcome")]
    [SerializeField] TextMeshProUGUI welcomeText;
    [SerializeField] float typeSpeed;
    [Header("BattleButton")]
    [SerializeField] CanvasGroup battleImage;
    [SerializeField] float battleButtonShowDelay;
    [SerializeField] float fadeSpeed;
    [Header("PlayerPanel")]
    [SerializeField] RectTransform avatarPanel;
    [SerializeField] RectTransform startPoint;
    [SerializeField] RectTransform endPoint;
    [SerializeField] float panelDelay;
    [SerializeField] float panelMoveSpeed;

    private string welcomeString;
    void Start()
    {
        welcomeString = welcomeText.text;
        welcomeText.text = "";

        StartCoroutine(TextTyping());
        StartCoroutine(ButtonFadeIn());
        StartCoroutine(AvatarMoving());
    }

    IEnumerator AvatarMoving()
    {
        avatarPanel.anchoredPosition = startPoint.anchoredPosition;
        yield return new WaitForSeconds(panelDelay);

        avatarPanel.DOAnchorPos(endPoint.anchoredPosition, panelMoveSpeed);
    }

    IEnumerator ButtonFadeIn()
    {
        battleImage.interactable = false;
        battleImage.alpha = 0;
        yield return new WaitForSeconds(battleButtonShowDelay);

        battleImage.DOFade(1, fadeSpeed).OnComplete(() => battleImage.interactable = true);       
    }

    IEnumerator TextTyping()
    {
        for (int i = 0; i < welcomeString.Length; i++)
        {
            welcomeText.text += welcomeString[i];
            yield return new WaitForSeconds(typeSpeed);
        }
    }
}
