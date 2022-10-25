using System;
using DG.Tweening;
using UnityEngine;

namespace Utils.Tweens
{
    public static class AnimationHelper
    {
        public static void ShowPopup_Scale(Transform target, float duration = 0.2f, Action doNext = null)
        {
            target.localScale = Vector3.zero;

            DOTween.Kill(target);
            DOTween.Sequence().SetId(target)
                .Append(target.DOScale(Vector3.one, duration))
                .OnComplete(() => doNext?.Invoke());
        }

        public static void HidePopup_Scale(Transform target, float duration = 0.2f, Action doNext = null)
        {
            DOTween.Kill(target);
            DOTween.Sequence().SetId(target)
                .Append(target.DOScale(Vector3.zero, duration))
                .OnComplete(() => doNext?.Invoke());
        }

        public static void Move(Transform target, Vector2 fightPosition, float duration = 0.2f, Action doNext = null)
        {
            DOTween.Kill(target);
            DOTween.Sequence().SetId(target)
                .Append(target.DOMove(fightPosition, duration))
                .OnComplete(() => doNext?.Invoke());
        }

        public static void Fight(Transform target, Vector2 fightPosition, float scaleValue = 1.2f, float duration = 0.2f, Action doNext = null)
        {
            DOTween.Kill(target);
            DOTween.Sequence().SetId(target)
                .Append(target.DOMove(fightPosition, duration))
                .Append(target.DOScale(Vector3.one * scaleValue, duration))
                .OnComplete(() => doNext?.Invoke());
        }
        
        public static void MoveCardsOnFight(Transform[] targets, Vector2 fightPosition, float duration = 0.2f, Action doNext = null)
        {
            foreach (Transform target in targets)
            {
                DOTween.Kill(target);
                DOTween.Sequence().SetId(target)
                    .Append(target.DOMove(fightPosition, duration))
                    .OnComplete(() => doNext?.Invoke());
            }
        }
    }
}
