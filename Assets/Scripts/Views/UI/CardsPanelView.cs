using System;
using System.Collections.Generic;
using System.Linq;
using Core.EventAggregator.Interface;
using Core.UiScenario;
using Cysharp.Threading.Tasks;
using Entities.Card;
using Presenters.Cards;
using Unity.Settings;
using UnityEngine;
using Views.Cards;

namespace Views.UI
{
    public class CardsPanelView : Presenter.View<GameCardsPresenter>, IPublisher<GameCardsPresenter.CardStatButtonClick, GameCardsPresenter.StartTurnButtonClick>
    {
        [SerializeField] private CardView cardPrefab;
        [SerializeField] private StatView statPrefab;
        
        [SerializeField] private Transform cardContainer;
        [SerializeField] private Animator battleFieldAnimator;
        [SerializeField] private CanvasGroup cardsPanelCanvasGroup;
        [SerializeField] private Transform yourCardPlayPos;
        [SerializeField] private Transform opponentCardPlayPos;
        [SerializeField] private List<OpponentCardView> opponentCards = new List<OpponentCardView>();

        public Func<GameCardsPresenter.CardStatButtonClick> Event1 { get; set; }
        public Func<GameCardsPresenter.StartTurnButtonClick> Event2 { get; set; }
        
        public Transform CardCardContainer => cardContainer;
        public CardView CardPrefab => cardPrefab;
        public StatView StatPrefab => statPrefab;
        
        private List<CardView> _cardViews;

        #region Overrides

        public override void Initialize()
        {
            Binder.Bind(this);
        }

        public override void OnPush()
        {
            _cardViews?.ForEach(x => x.OnStatBtnClick -= OnStatBtnClick);
            _cardViews?.ForEach(x => x.StartTurnBtnClick -= OnStartTurnBtnClick);
            ReuseOpponentCards();
        }
        
        #endregion

        #region View Interface

        public async void BattleStartCardsAnim(Action callback)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(Time.deltaTime));
            callback();
        }
        
        public void BlockCardsPanel ()
        {
            cardsPanelCanvasGroup.interactable = false;
            cardsPanelCanvasGroup.blocksRaycasts = false;
        }
        
        public void UnblockCardsPanel ()
        {
            cardsPanelCanvasGroup.interactable = true;
            cardsPanelCanvasGroup.blocksRaycasts = true;
        }

        public void BlockCardsExcept(int id)
        {
            var inGameCards = _cardViews.Select(_ => _).Where(x => !x.IsUsed).ToList();
            inGameCards.ForEach(x => { x.BlockCard(); });
            inGameCards.Find(x => x.Id == id).UnblockCard();
        }
        
        public void UnBlockCards()
        {
            _cardViews.ForEach(x =>
            {
                if(!x.IsUsed)
                    x.UnblockCard();
            });
        }

        public void InitializeCardsAnimPreparations(IEnumerable<CardView> cardViews)
        {
            _cardViews = cardViews.ToList();
            foreach (var cardView in _cardViews)
            {
                cardView.Initialize();
                cardView.OnStatBtnClick += OnStatBtnClick;
                cardView.StartTurnBtnClick += OnStartTurnBtnClick;
            }
        }
        
        public void ShowCardStats()
        {
            _cardViews.ForEach(x =>
            {
                if(!x.IsUsed)
                    x.ShowCardStats();
            });
        }

        public void ShowCardStats(int id, params CardStatsType[] exceptionStats)
        {
            _cardViews.Find(x => x.Id == id).ShowCardStats(exceptionStats);
        }
        
        public void ShowCardFaces()
        {
            _cardViews.ForEach(x => x.ShowCardFace());
        } 
        
        public void FlipCards ()
        {
            _cardViews.ForEach(x =>
            {
                if(!x.IsUsed)
                    x.FlipCard();
            });
        }

        public void BlockCardStat (int cardId, CardStatsType statsType)
        {
            _cardViews.Find(x => x.Id == cardId).BlockStat(statsType);
        }

        public void BlockAllStatsExcept(CardStatsType type)
        {
            _cardViews.ForEach(x =>
            {
                if(!x.IsUsed)
                    x.BlockStatsExcept(type);
            });
        }

        public void UnBlockAllStats()
        {
            _cardViews.ForEach(x =>
            {
                if(!x.IsUsed)
                    x.UnblockStats();
            });
        }
        
        public void UseCard (int cardId)
        {
            var card = _cardViews.Find(x => x.Id == cardId);
            card.UseCard();
        }
        

        // Card select will use before stat select
        public void ShowCardSelectBtns ()
        {
            _cardViews.ForEach(x =>
            {
                if(!x.IsUsed)
                    x.ShowCardClickBtn();
            });
        }

        public void ShowGameBoard (bool isShow)
        {
             battleFieldAnimator.SetBool(Const.CardAnimatorValues.HideGameBoard, isShow);
        }

        public void UseBooster (int selectedCardId, CardStatsType statsType, int addedValue)
        {
            var selectedCard = _cardViews.Find(x => x.Id == selectedCardId);
            selectedCard.BoostStat(statsType, addedValue);
        }

        public void DeBoostCards()
        {
            _cardViews.ForEach(x => x.DeBoost());
        }
        
        public void DeselectAllStats()
        {
            _cardViews.ForEach(x => x.Deselect());
        }

        #endregion
        
        private void OnStatBtnClick(int cardId, CardStatsType statType)
        {
            Event1().Publish(cardId, statType);
        }
        
        private void OnStartTurnBtnClick()
        {
            _cardViews.ForEach(x =>
            {
                if (x.IsSelected)
                {
                    x.Deselect();
                    x.DownAnim();
                }
            });
            Event2().Publish();
        }

        public void LoseOpponentsCard()
        {
            var card = opponentCards.Find(x => !x.IsUsed);
            if (card != null)
            {
                card.UseCard(true);
            }
        }

        private void ReuseOpponentCards()
        {
            opponentCards.ForEach(_=>_.UseCard(false));
        }
    }
}