using System;
using System.Collections;
using System.Collections.Generic;
using Core.EventAggregator.Interface;
using Core.UiScenario;
using Presenters.UI;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Utils.Logger;
using Views.Cards;
using Button = UnityEngine.UI.Button;

namespace Views.UI
{
    public class FightersWindowView : Presenter.View<FightersWindowPresenter>, IPublisher<FightersWindowPresenter.ButtonClick, FightersWindowPresenter.CardButtonClick>
    {
        [SerializeField] private Button closeButton;
        [SerializeField] private Transform decksGrid;
        [SerializeField] private Transform topBorder;
        [SerializeField] private Transform bottomBorder;
        [SerializeField] private ScrollRect scroll;

        public Func<FightersWindowPresenter.ButtonClick> Event1 { private get; set; }
        public Func<FightersWindowPresenter.CardButtonClick> Event2 { private get; set; }

        public Transform TopBorder => topBorder;
        public Transform BottomBorder => bottomBorder;
        
        private List<CollectionCardView> _cards ;
        private Coroutine _hideShowRoutine;
        private float _topScrollBorderY, _botScrollBorderY;
        

        public override void Initialize()
        {
            Binder.Bind(this);
            closeButton.onClick.AddListener(() => Event1().Publish(FightersWindowPresenter.ButtonType.Close));
        }

        public override void OnPop()
        {
            _topScrollBorderY = topBorder.position.y;
            _botScrollBorderY = bottomBorder.position.y;
        }

        public override void OnPush()
        {
            _cards.ForEach(x => x.OnCardCollectionBtnClick -= HandleCardCollectionClick);
            _cards.Clear();
        }

        public void SetCardToGrid(CollectionCardView cardView)
        {
            _cards.Add(cardView);
            cardView.transform.SetParent(decksGrid);
            cardView.OnCardCollectionBtnClick += HandleCardCollectionClick;
        }
        
        public void SetCardViewsToGrid(List<CollectionCardView> cardViews)
        {
            _cards = cardViews;
            foreach (var cardView in _cards)
            {
                cardView.transform.SetParent(decksGrid);
                cardView.OnCardCollectionBtnClick += HandleCardCollectionClick;
                CustomLogger.Log(LogSource.Unity, $" CARD transform.position.y {transform.position.y}");

                if (cardView.transform.position.y < _topScrollBorderY && cardView.transform.position.y > _botScrollBorderY)
                {
                    CustomLogger.Log(LogSource.Unity, "Show on start");
                    cardView.Show();
                }
            }

            if (_hideShowRoutine != null)
            {
                StopCoroutine(_hideShowRoutine);
                _hideShowRoutine = null;
            }
            _hideShowRoutine = StartCoroutine(CheckPositions());
        }

        private IEnumerator CheckPositions()
        {
            while (true)
            {
                yield return new WaitForEndOfFrame();
                yield return new WaitForEndOfFrame();

                if (scroll.velocity.y == 0)
                {
                    CustomLogger.Log(LogSource.Unity, $"scroll.velocity.y == 0");
                    continue;
                }
                
                foreach (var cardView in _cards)
                {
                    //yield return new WaitForEndOfFrame();
                    
                    if (!cardView.IsHidden && 
                        (cardView.transform.position.y < _botScrollBorderY || cardView.transform.position.y > _topScrollBorderY))
                    {
                        CustomLogger.Log(LogSource.Unity, "Hide");
                        cardView.Hide();
                    }

                    if (cardView.IsHidden && cardView.transform.position.y < _topScrollBorderY && cardView.transform.position.y > _botScrollBorderY)
                    {
                        CustomLogger.Log(LogSource.Unity, "Show");
                        cardView.Show();
                    }
                }
            }
        }

        private void HandleCardCollectionClick(int id)
        {
            Event2().Publish(id);
        }
    }
}