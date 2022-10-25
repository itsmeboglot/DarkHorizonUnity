using System;
using Core.UiScenario;
using DG.Tweening;
using Presenters.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Views.UI
{
    public class GameLoadView : Presenter.View<GameLoadPresenter>
    {
        [SerializeField] private Image background;
        [SerializeField] private float fadeDuration;

        public void FadeIn(Action doNext = null)
        {
            background.DOFade(1f, fadeDuration).OnComplete(() => doNext?.Invoke());
        }
        
        public void FadeOut(Action doNext = null)
        {
            background.DOFade(0f, fadeDuration).OnComplete(() => doNext?.Invoke());
        }
    }
}