using System;
using System.Collections;
using System.Collections.Generic;
using Core.View.ViewPool;
using DG.Tweening;
using Entities.Card;
using UnityEngine;
using UnityEngine.UI;

namespace Views.Cards
{
    public class DeckCardView : ViewPoolable
    {
        [SerializeField] private Image avatar;
        [SerializeField] private Button changeCardBtn;
        [SerializeField] private Button animCardBtn;
        [SerializeField] private Transform cardContainer;

        [Header("Card Stats")] 
        [SerializeField] private CanvasGroup faceCanvasGroup;
        [SerializeField] private CanvasGroup statsCanvasGroup;
        [SerializeField] private List<StatView> statViews;

        public event Action<int> OnChangeDeckCardClicked;
        public event Action<int> OnCardClicked;

        public int Id
        {
            get
            {
                if (_cardEntity == null)
                    return -1;
                return _cardEntity.Id;
            }
        }

        private Card _cardEntity;
        private Vector3 _startPos;
        private int _index = -1;
        private bool _isClicked;

        private void Start()
        {
            _startPos = cardContainer.localPosition;
        }

        public void OnEnable()
        {
            changeCardBtn.onClick.AddListener(HandleChangeCardClicked);
            animCardBtn.onClick.AddListener(HandleOnFaceCardBtnClicked);
        }

        public void OnDisable()
        {
            changeCardBtn.onClick.RemoveListener(HandleChangeCardClicked);
            animCardBtn.onClick.RemoveListener(HandleOnFaceCardBtnClicked);
        }

        public void SetCardData (CardViewData cardData, int cardIndex)
        {
            _index = cardIndex;
            UpAnim();
            
            if(cardData == null)
                return;

            
            _cardEntity = cardData.Entity;
            avatar.sprite = cardData.Sprite;

            for (int i = 0; i < statViews.Count; i++)
            {
                statViews[i].SetData(cardData.StatViewData[i]);
            }
            int index = 0;
            statViews.ForEach(x =>
            {
                x.SetData(cardData.StatViewData[index]);
                index++;
            });
            
            ShowStats();
        }
        
        public void UpAnim()
        {
            // DOTween.Kill(this);
            // DOTween.Sequence().Append(cardContainer.DOLocalMove(_startPos, 0.3f));
            //
            _isClicked = false;
        }
        
        private void DownAnim()
        {
            // DOTween.Kill(this);
            // var finPos = _startPos;
            // finPos.y -= 50;
            // DOTween.Sequence().Append(cardContainer.DOLocalMove(finPos, 0.3f));
        }

        private void HandleOnFaceCardBtnClicked()
        {
            OnCardClicked?.Invoke(_index);
            if (_isClicked)
            {
                UpAnim();
                _isClicked = false;
                return;
            }
            DownAnim();
            _isClicked = true;
        }
        
        private void HandleChangeCardClicked()
        {
            OnChangeDeckCardClicked?.Invoke(_index);
        }

        private void ShowStats()
        {
            faceCanvasGroup.alpha = 0;
            faceCanvasGroup.interactable = false;
            faceCanvasGroup.blocksRaycasts = false;
            
            statsCanvasGroup.alpha = 1;
            statsCanvasGroup.interactable = true;
            statsCanvasGroup.blocksRaycasts = true;

        }
    }
}