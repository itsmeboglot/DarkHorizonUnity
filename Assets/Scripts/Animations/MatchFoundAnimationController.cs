using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchFoundAnimationController : MonoBehaviour
{
    [SerializeField] RectTransform leftAvatar;
    [SerializeField] RectTransform rightAvatar;
    [SerializeField] RectTransform leftStartPoint;
    [SerializeField] RectTransform rightStartPoint;
    [SerializeField] RectTransform leftMovePoint;
    [SerializeField] RectTransform rightMovePoint;
    [SerializeField] float moveSpeed;

    void OnEnable()
    {
        leftAvatar.anchoredPosition = leftStartPoint.anchoredPosition;
        rightAvatar.anchoredPosition = rightStartPoint.anchoredPosition;

        var leftAvatarCG = leftAvatar.GetComponent<CanvasGroup>();
        var rightAvatarCG = rightAvatar.GetComponent<CanvasGroup>();

        leftAvatarCG.alpha = 0;
        leftAvatarCG.DOFade(1, moveSpeed);

        rightAvatarCG.alpha = 0;
        rightAvatarCG.DOFade(1, moveSpeed);

        leftAvatar.DOAnchorPos(leftMovePoint.anchoredPosition, moveSpeed);
        rightAvatar.DOAnchorPos(rightMovePoint.anchoredPosition, moveSpeed);
    }    
}
