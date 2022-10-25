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

namespace Views.UI
{
    public class BattleResultsData
    {
        public bool IsYouWinner;
        public int WinCount;
        public int LoseCount;
        public Sprite OpponentAvatar;
        public Sprite MyAvatar;
    }

    public class BattleResultsView : Presenter.View<BattleResultsPresenter>, IPublisher<BattleResultsPresenter.ButtonClick>
    {
        [SerializeField] private Button closeButton;
        [SerializeField] RectTransform winnerPanel;
        [SerializeField] RectTransform loserPanel;
        [SerializeField] private Image winnerAvatar;
        [SerializeField] private Image looserAvatar;
        //[SerializeField] private TMP_Text rankingText;
        //[SerializeField] private TMP_Text winLosePercentText;
        [SerializeField] private TMP_Text wonCardsText;
        [SerializeField] private TMP_Text loseCardsText;

        public Func<BattleResultsPresenter.ButtonClick> Event1 { get; set; }

        public override void Initialize()
        {
            Binder.Bind(this);
        }

        public override void OnPush()
        {
            // closeButton.onClick.RemoveListener(() => Event1().Publish());
        }

        /*
        private void OnEnable()
        {
            SetData(null);
        }*/

        public void SetData(BattleResultsData data)
        {
            if (data != null)
            {
                closeButton.onClick.AddListener(OnClose);

                StartCoroutine(TextTyping($"Won cards {data.WinCount}", wonCardsText, .1f, 0f));
                StartCoroutine(TextTyping($"Lose cards {data.LoseCount}", loseCardsText, .1f, 1f));

                //winLosePercentText.text = (data.WinCount / data.LoseCount).ToString();
                winnerAvatar.sprite = data.IsYouWinner ? data.MyAvatar : data.OpponentAvatar;
                looserAvatar.sprite = data.IsYouWinner ? data.OpponentAvatar : data.MyAvatar;
            }

            Vector2 startPosition = loserPanel.anchoredPosition;
            loserPanel.anchoredPosition = winnerPanel.anchoredPosition;
            CanvasGroup loserCG = loserPanel.GetComponent<CanvasGroup>();
            loserCG.alpha = 0;

            winnerPanel.localScale = Vector3.one * .1f;
            CanvasGroup winnerCG = winnerPanel.GetComponent<CanvasGroup>();
            winnerCG.alpha = 0;

            winnerCG.DOFade(1, .25f);
            winnerPanel.DOScale(Vector3.one, .5f).OnComplete(() =>
            {
                loserCG.DOFade(1, 1f);
                loserPanel.DOAnchorPos(startPosition, 1f);
            });          
        }

        IEnumerator TextTyping(string s, TMP_Text text, float typeSpeed, float startDelay)
        {
            text.text = "";
            yield return new WaitForSeconds(startDelay);

            for (int i = 0; i < s.Length; i++)
            {
                text.text += s[i];
                yield return new WaitForSeconds(typeSpeed);
            }
        }

        private void OnClose()
        {
            Event1?.Invoke().Publish();
        }
    }
}