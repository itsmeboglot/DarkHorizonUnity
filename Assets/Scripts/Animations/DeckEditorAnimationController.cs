using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class DeckEditorAnimationController : WindowAnimationController
{   
    [Header("BackButton")]
    [SerializeField] RectTransform backButton;
    [SerializeField] float backBtnSpeed;
    [SerializeField] RectTransform backBtnStartPoint;
    [SerializeField] RectTransform backBtnPosition;

    protected override void OnEnable()
    {
        base.OnEnable();       

        backButton.anchoredPosition = backBtnStartPoint.anchoredPosition;
        backButton.DOAnchorPos(backBtnPosition.anchoredPosition, backBtnSpeed);
    }     
}
