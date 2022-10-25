using System;
using System.Collections.Generic;
using Core.View.ViewPool;
using DG.Tweening;
using Entities.Card;
using Unity.Settings;
using UnityEngine;
using Button = UnityEngine.UI.Button;
using Image = UnityEngine.UI.Image;

namespace Views.Cards
{
    public class CardView : ViewPoolable//, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private Animator cardAnimator;
        [SerializeField] private CanvasGroup faceCardCanvasGroup;
        [SerializeField] private CanvasGroup usedCardCanvasGroup;
        [SerializeField] private CanvasGroup selectCardBtnCanvasGroup;

        [SerializeField] private Transform cardBody;
        [Header("Face")] 
        [SerializeField] private Image cardMainImage;
        [SerializeField] private Button cardSelectBtn;

        [Header("Back")] 
        [SerializeField] private Image cardBackAvatar;

        [Header("Card Stats")] 
        [SerializeField] private Transform cardStatsContainer;
        [SerializeField] private CanvasGroup statsCanvasGroup;
        [SerializeField] private Image cardFrameImage;
        [SerializeField] private Color selectedCardColor;
        [SerializeField] private Color normalCardColor;
        
        [SerializeField] private Button startTurnBtn;
        
        [Header("Colored view elements")]
        [SerializeField] private List<Image> coloredElements = new List<Image>();

        private static event Action<CardView> OnCardClick;
        public event Action<int, CardStatsType> OnStatBtnClick; // int - card id
        public event Action StartTurnBtnClick; // int - card id

        public int Id => _cardEntity.Id;
        public Transform CardStatsContainer => cardStatsContainer;
        public bool IsBoosted => _isBoosted;
        public bool IsSelected => _isSelected;
        public bool IsUsed => _used;

        private List<StatView> _statViews;
        private Card _cardEntity;
        private Vector3 _startPos;
        private bool _initialized;
        private bool _used;
        private bool _isBlocked;
        private bool _isSelected;
        private bool _isBoosted;

        public void Start()
        {
            startTurnBtn.onClick.AddListener(HandleStartTurn);
            cardSelectBtn.onClick.AddListener(HandleCardSelect);
            
            ShowCardFace();

            OnCardClick += HandleAnotherCardShowStats;
        }

        public void OnDestroy()
        {
            startTurnBtn.onClick.RemoveAllListeners();
            cardSelectBtn.onClick.RemoveAllListeners();
            OnCardClick -= HandleAnotherCardShowStats;

            DOTween.Kill(this);
        }

        public void SetData(CardViewData data, List<StatView> statViews)
        {
            SetSprite(data.Sprite);
            _cardEntity = data.Entity;
            _statViews = statViews;
            _statViews.ForEach(x => x.OnStatClicked += OnStatButtonClicked);
        }

        public void FlipCard()
        {
            cardAnimator.SetBool(Const.CardAnimatorValues.FlipCard, true);
        }
        
        public void ShowCardStats(params CardStatsType[] blocks)
        {
            if(_isBlocked || _used)
                return;
            
            cardAnimator.SetBool(Const.CardAnimatorValues.ShowAvatar, false);

            faceCardCanvasGroup.alpha = 0;
            faceCardCanvasGroup.interactable = false;
            faceCardCanvasGroup.blocksRaycasts = false;

            statsCanvasGroup.alpha = 1;
            statsCanvasGroup.interactable = true;
            statsCanvasGroup.blocksRaycasts = true;

            foreach (var stat in blocks)
            {
                BlockStat(stat);
            }
            
            startTurnBtn.gameObject.SetActive(true);
        }

        public void ShowCardFace()
        {
            cardAnimator.SetBool(Const.CardAnimatorValues.ShowAvatar, true);

            faceCardCanvasGroup.alpha = 1;
            faceCardCanvasGroup.interactable = true;
            faceCardCanvasGroup.blocksRaycasts = true;

            statsCanvasGroup.alpha = 0;
            statsCanvasGroup.interactable = false;
            statsCanvasGroup.blocksRaycasts = false;
            startTurnBtn.gameObject.SetActive(false);
        }

        public void UseCard()
        {
            ShowCardFace();
            DownAnim();
            statsCanvasGroup.interactable = false;
            statsCanvasGroup.gameObject.SetActive(false);
            usedCardCanvasGroup.blocksRaycasts = false;
            cardAnimator.SetBool(Const.CardAnimatorValues.UseCard, true);

            _used = true;
        }

        public void BlockCard()
        {
            _isBlocked = true;
            faceCardCanvasGroup.interactable = false;
        }
        
        public void UnblockCard()
        {
            if(_used)
                return;
            
            _isBlocked = false;
            faceCardCanvasGroup.interactable = false;
        }

        public void Initialize()
        {
            _initialized = true;
            _startPos = cardBody.localPosition;
        }

        public void BlockStat (CardStatsType type)
        {
            foreach (var container in _statViews)
            {
                if (container.StatType != type)
                    continue;

                container.SetInteractable(false);
            }
        }
        
        public void BlockStatsExcept (CardStatsType type)
        {
            foreach (var container in _statViews)
            {
                if (container.StatType == type)
                    continue;

                container.SetInteractable(false);
            }
        }

        public void UnblockStats()
        {
            foreach (var container in _statViews)
            {
                container.SetInteractable(true);
            }
        }
        
        public void ShowCardClickBtn()
        {
            selectCardBtnCanvasGroup.alpha = 1;
            selectCardBtnCanvasGroup.interactable = true;
            selectCardBtnCanvasGroup.blocksRaycasts = true;
        }
        
        public void HideCardClickBtn()
        {
            selectCardBtnCanvasGroup.alpha = 0;
            selectCardBtnCanvasGroup.interactable = false;
            selectCardBtnCanvasGroup.blocksRaycasts = false;
        }

        public void Deselect()
        {
            SelectCard(false);
            DownAnim();
            ShowCardClickBtn();
        }

        public void BoostStat (CardStatsType type, int addedValue)
        {
            _isBoosted = true;
            var stat = _statViews.Find(x => x.StatType == type);
            stat.BoostStat(addedValue);
        }

        public void DeBoost()
        {
            var stat = _statViews.Find(x => x.IsBoosted);
            if(stat != null)
            {
                stat.DeBoost();
                _isBoosted = false;
            }
        }
        
        private void SelectCard (bool isSelected)
        {
            _isSelected = isSelected;
             cardFrameImage.color = _isSelected? selectedCardColor : normalCardColor;
        }

        private void SetSprite(Sprite sprite)
        {
            cardMainImage.sprite = sprite;
            cardBackAvatar.sprite = sprite;
        }

        private void OnStatButtonClicked(CardStatsType statValue)
        {
            if (_initialized && !_used)
            {
                OnStatBtnClick?.Invoke(_cardEntity.Id, statValue);
                if (_isSelected)
                {
                    UpAnim();
                }
            }
        }

        private void HandleAnotherCardShowStats(CardView clickedView)
        {
            if (clickedView != this && !_used && !_isBlocked)
            {
                clickedView._statViews.ForEach(x =>
                {
                    x.Deselect();
                });
                DownAnim();
                ShowCardClickBtn();
                cardFrameImage.color = Color.white;
            }
            SelectCard(clickedView == this);
        }
        
        private void HandleStartTurn()
        {
            StartTurnBtnClick?.Invoke();
        }

        private void HandleCardSelect()
        {
            HideCardClickBtn();
            OnCardClick?.Invoke(this);
        }

        private void UpAnim()
        {
            DOTween.Kill(this);
            DOTween.Sequence().Append(cardBody.DOLocalMove(_startPos + Vector3.up * 60f, 0.2f));
        }

        public void DownAnim()
        {
            DOTween.Kill(this);
            DOTween.Sequence().Append(cardBody.DOLocalMove(_startPos, 0.4f));
        }
        
        
        // public void OnPointerEnter(PointerEventData eventData)
        // {
        //     if (_initialized && !_used && !_isBlocked )
        //         UpAnim();
        // }
        //
        // public void OnPointerExit(PointerEventData eventData)
        // {
        //     if (_initialized && !_used && !_isBlocked )
        //         DownAnim();
        // }

    }
}