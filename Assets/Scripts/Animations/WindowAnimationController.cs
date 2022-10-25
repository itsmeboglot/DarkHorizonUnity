using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class WindowAnimationController : MonoBehaviour
{
    [SerializeField] protected CanvasGroup landingPageGroup;
    [SerializeField] protected float unlockDelay;
    [Header("Header")]
    [SerializeField] protected RectTransform header;
    [SerializeField] protected float headerMoveSpeed;
    [SerializeField] protected RectTransform headerStartPoint;
    [SerializeField] protected RectTransform headerPosition;
    [Header("Window")]
    [SerializeField] protected bool windowAnim;
    [SerializeField] protected RectTransform window;
    [SerializeField] ScrollRect scroll;

    protected virtual void OnEnable()
    {
        landingPageGroup.interactable = false;

        StartCoroutine(UnlockGroup());

        header.anchoredPosition = headerStartPoint.anchoredPosition;
        header.DOAnchorPos(headerPosition.anchoredPosition, headerMoveSpeed);

        //SetCardsScale();

        if (!windowAnim)
        {
            //window.localScale = Vector3.one * .1f;
            CanvasGroup windowCG = window.GetComponent<CanvasGroup>();
            windowCG.alpha = 0;
            windowCG.DOFade(1, 1);
            //DOTween.Sequence().Append(window.DOScale(Vector3.one * 1.1f, .4f)).Append(window.DOScale(Vector3.one, .1f)).AppendCallback(() => SetCardsScale());
        }
    }

    private void SetCardsScale()
    {
        foreach (Transform item in scroll.content)
        {
            item.localScale = Vector3.one;
        }
    }

    IEnumerator UnlockGroup()
    {
        yield return new WaitForSeconds(unlockDelay);

        landingPageGroup.interactable = true;
    }
}
