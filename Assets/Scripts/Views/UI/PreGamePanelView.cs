using System;
using System.Collections.Generic;
using System.Linq;
using Core.EventAggregator.Interface;
using Core.UiScenario;
using Entities.Card;
using Presenters.UI;
using UnityEngine;
using UnityEngine.UI;
using Views.Cards;

namespace Views.UI
{
    public class PreGamePanelView : Presenter.View<PreGamePresenter>, IPublisher<PreGamePresenter.ButtonClick, PreGamePresenter.CardButtonClick>
    {
        [SerializeField] private GameObject container;
        [SerializeField] private Button playButton;
        [SerializeField] private Button closeButton;
        [SerializeField] private Button saveDeckButton;
        
       [Header("Deck editor")]
       [SerializeField] private List<DeckCardView> _currentDeckCards = new List<DeckCardView>();
       
        public Func<PreGamePresenter.ButtonClick> Event1 { private get; set; }
        public Func<PreGamePresenter.CardButtonClick> Event2 { private get; set; }

        public List<int> CurrentCardIds => _currentDeckCards.Select(x => x.Id).ToList();
        
        public override void Initialize()
        {
            Binder.Bind(this);
        }

        public override void OnPop()
        {
            playButton.onClick.AddListener(() => Event1().Publish(PreGamePresenter.ButtonType.Play));
            closeButton.onClick.AddListener(() => Event1().Publish(PreGamePresenter.ButtonType.Close));
            saveDeckButton.onClick.AddListener(() => Event1().Publish(PreGamePresenter.ButtonType.SaveDeck));
        }

        public override void OnPush()
        {
            playButton.onClick.RemoveAllListeners();
            closeButton.onClick.RemoveAllListeners();
            _currentDeckCards.ForEach(x =>
            {
                x.UpAnim();
                x.OnChangeDeckCardClicked -= HandleOnChangeCardClicked;
                x.OnCardClicked -= HandleCardClicked;
            });
        }

        public void InitWithData (List<CardViewData> cardViews)
        {
            int index = 0;
            foreach (var deckCardView in _currentDeckCards)
            {
                deckCardView.OnChangeDeckCardClicked += HandleOnChangeCardClicked;
                deckCardView.OnCardClicked += HandleCardClicked;
                deckCardView.SetCardData(cardViews[index], index);
                index++;
            }

           
        }

        public void UpdateCardByIndex(int index, CardViewData data)
        {
            if(index == -1)
                return;
            
            //ToDo: change to multiple decks later
            _currentDeckCards[index].SetCardData(data, index);
        }

        public void EnablePlayButton()
        {
            playButton.interactable = true;
            //closeButton.interactable = true;
        }

        public void DisablePlayButton()
        {
            playButton.interactable = false;
        }

        public void EnableCloseButton()
        {
            closeButton.interactable = true;
        }
        
        public void DisableCloseBtn()
        {
            closeButton.interactable = false;
        }

        private void HandleCardClicked(int cardIndex)
        {
            for (int i = 0; i < _currentDeckCards.Count; i++)
            {
                if (i != cardIndex)
                    _currentDeckCards[i].UpAnim();
            }
        }
        
        private void HandleOnChangeCardClicked(int cardIndex)
        {
            Event2().Publish(cardIndex, _currentDeckCards[cardIndex].Id);
        }

        public void Show()
        {
            container.SetActive(true);
        }
        
        public void Hide()
        {
            container.SetActive(false);
        }
    }
}