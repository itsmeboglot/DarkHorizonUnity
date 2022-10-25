using System;
using System.Collections.Generic;
using Core.View.ViewPool;
using Entities.Card;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Views.Cards
{
    public class CollectionCardView : ViewPoolable
    {
        public event Action<int> OnCardCollectionBtnClick;
        
        [SerializeField] private RectTransform rectTransform;
        [SerializeField] private CanvasGroup container;
        [SerializeField] private Image[] avatars;
        [SerializeField] private TMP_Text cardId;
        [SerializeField] private Button chooseToDeckButton;
        [SerializeField] private Button faceCardButton;
        [SerializeField] private List<StatView> stats;

        public int Id => _currentCardId;
        public bool IsHidden => _isHidden;
        
        private Card _cardEntity;
        private int _currentCardId = -1;
        private bool _isHidden;
        private float _topBorder;
        private float _bottomBorder;

        // private void LateUpdate()
        // {
        //     // if (_isHidden && transform.position.y >= _bottomBorder && transform.position.y <= _topBorder)
        //     // {
        //     //     //Show();
        //     // }
        //     // if(!_isHidden && (transform.position.y < _bottomBorder || transform.position.y > _topBorder))
        //     // {
        //     //     //Hide();
        //     // }
        // }
        
        public void Hide()
        {
            container.alpha = 0;
            container.interactable = false;
            container.blocksRaycasts = false;
            _isHidden = true;
        }

        public void Show()
        {
            container.alpha = 1;
            container.interactable = true;
            container.blocksRaycasts = true;
            _isHidden = false;
        }

        public void Initialize(CardViewData cardViewData, bool isDeckEditing, bool containsInDeck, float topBorder, float bottomBorder)
        {
            _cardEntity = cardViewData.Entity;
            _currentCardId = _cardEntity.Id;
            cardId.text = $"#{_cardEntity.Id}";
            foreach (var avatar in avatars)
            {
                avatar.sprite = cardViewData.Sprite;
            }
            
            chooseToDeckButton.gameObject.SetActive(isDeckEditing && !containsInDeck);
            if (isDeckEditing)
            {
                chooseToDeckButton.onClick.AddListener(HandleChooseClick);
            }
            else
            {
                faceCardButton.onClick.AddListener(HandleCardBtnClick);
            }

            transform.localScale = Vector3.one;
            _topBorder = topBorder;
            _bottomBorder = bottomBorder;
        }

        

        public override void OnPush()
        {
            transform.localScale = Vector3.one;

            chooseToDeckButton.onClick.RemoveAllListeners();
            faceCardButton.onClick.RemoveAllListeners();
        }

        private void HandleChooseClick()
        {
            chooseToDeckButton.onClick.RemoveListener(HandleChooseClick);
            OnCardCollectionBtnClick?.Invoke(_currentCardId);
        }
        
        private void HandleCardBtnClick()
        {
            
        }
    }
}