using System;
using System.Collections;
using System.Collections.Generic;
using Core.UiScenario;
using Presenters.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Views.UI
{
    public class TimerView : Presenter.View<TimerPresenter>
    {
        [SerializeField] private TMP_Text timerTmp;
        [SerializeField] private TMP_Text topText;
        [SerializeField] private GameObject timerContainer;
        
        [Header("Colored view elements")]
        [SerializeField] private List<Image> coloredElements = new List<Image>();
        [SerializeField] private Color yourTurnColor;
        [SerializeField] private Color opponentTurnColor;

        public int SecondsLeft { get; private set; }
        private Coroutine _timerCor;

        public void AnimateTimer(int seconds, string textTop)
        {
            timerContainer.SetActive(true);
            SecondsLeft = seconds;
            topText.text = textTop;
            var waiter = new WaitForSecondsRealtime(1);
            
            if(_timerCor != null)
                StopCoroutine(_timerCor);

            _timerCor = StartCoroutine(RunTimer(seconds, waiter));
        }

        public void HideTimer()
        {
            timerContainer.SetActive(false);
        }

        public void SetYourTurnColor()
        {
            coloredElements.ForEach(x=>x.color = yourTurnColor);
        }
        
        public void SetOpponentTurnColor()
        {
            coloredElements.ForEach(_=>_.color = opponentTurnColor);
        }

        private void SetTimerValue(int seconds)
        {
            timerTmp.text = TimeSpan.FromSeconds(seconds).ToString(@"mm\:ss");
        }

        private IEnumerator RunTimer(int seconds, WaitForSecondsRealtime waiter)
        {
            SetTimerValue(seconds);
            
            while (seconds > 0)
            {
                yield return waiter;
                seconds--;
                SecondsLeft = seconds;
                SetTimerValue(seconds);
            }
        }
    }
}