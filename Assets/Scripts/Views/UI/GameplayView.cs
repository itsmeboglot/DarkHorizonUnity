using System;
using System.Collections;
using System.Collections.Generic;
using Core.EventAggregator.Interface;
using Core.UiScenario;
using DG.Tweening;
using Presenters.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils.Logger;
using Views.WithoutPresenter;

namespace Views.UI
{
    public class GameplayView : Presenter.View<GameplayWindowPresenter>, IPublisher<GameplayWindowPresenter.ButtonClick>
    {
        [Serializable]
        private struct ProfileView
        {
            public TMP_Text nicknameTmp;
            public Image avatar;
        }
        [SerializeField] private TMP_Text rseTokensBalanceText;

        [SerializeField] private Button closeBtn;
        [SerializeField] private CanvasGroup endOfTurnCanvasGroup;
        [SerializeField] private CanvasGroup showdownCanvasGroup;
        [SerializeField] private CanvasGroup timerCG;
        [SerializeField] private ShowdownView showdownView;
        [SerializeField] private ProfileView myProfileView;
        [SerializeField] private ProfileView opponentProfileView;
        
        [Header("Colored view elements")]
        [SerializeField] private List<Image> coloredElements = new List<Image>();
        [SerializeField] private Color normalColor;
        [SerializeField] private Color winColor;
        [SerializeField] private Color loseColor;
        [SerializeField] private Color fightColor;

        public Func<GameplayWindowPresenter.ButtonClick> Event1 { private get; set; }

        public override void Initialize()
        {
            Binder.Bind(this);
        }

        public override void OnPop()
        {
            closeBtn.onClick.AddListener(() => Event1().Publish(GameplayWindowPresenter.ButtonType.Close));
        }

        public override void OnPush()
        {
            closeBtn.onClick.RemoveAllListeners();
        }

        public void OpenShowDownView (ShowdownData showdownData, bool isMyWin)
        {
            SetActiveCanvasGroup(timerCG, false);
            SetActiveCanvasGroup(showdownCanvasGroup, true);
            showdownView.SetData(showdownData, isMyWin);
            showdownView.Show(() =>
            {
                if (!isMyWin)
                {
                    SetLoseColor();
                }
                else
                {
                    SetWinColor();
                }
            });
        }
        
        public void HideShowDownView ()
        {
            SetActiveCanvasGroup(showdownCanvasGroup, false);
            showdownView.Hide();
            SetActiveCanvasGroup(timerCG, true);
        }

        public void ShowEndOfTurn ()
        {
            SetActiveCanvasGroup(endOfTurnCanvasGroup, true);
        }
        
        public void HideEndOfTurn ()
        {
            SetActiveCanvasGroup(endOfTurnCanvasGroup, false);
        }

        public void UpdateYourProfile(string nickname, Sprite sprite)
        {
            myProfileView.nicknameTmp.text = nickname;
            myProfileView.avatar.sprite = sprite;
        }
        
        public void UpdateOpponentProfile(string nickname, Sprite sprite)
        {
            opponentProfileView.nicknameTmp.text = nickname;
            opponentProfileView.avatar.sprite = sprite;
        }
        
        public void SetNormalColor()
        {
            coloredElements.ForEach(x=> x.DOColor(normalColor, 0.2f));
        }

        public void SetWinColor()
        {
            coloredElements.ForEach(x=>x.DOColor(winColor, 0.2f));
        }
        
        public void SetLoseColor()
        {
            coloredElements.ForEach(x=>x.DOColor(Color.red, 0.5f));
        }
        
        public void SetFightColor()
        {
            coloredElements.ForEach(x=>x.DOColor(fightColor, 0.2f));
        }

        public void SetRseTokensBalance(int balance)
        {
            rseTokensBalanceText.text = balance.ToString();
        }

        #region Private Methods

        private void SetActiveCanvasGroup(CanvasGroup canvasGroup, bool isActive)
        {
            //ToDo: will make with animations
            canvasGroup.alpha = isActive ? 1 : 0;
            canvasGroup.interactable = isActive;
            canvasGroup.blocksRaycasts = isActive;
        }

        #endregion
    }
}