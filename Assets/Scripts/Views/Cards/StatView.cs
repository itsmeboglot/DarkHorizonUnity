using System;
using System.Collections.Generic;
using System.Linq;
using Core.View.ViewPool;
using Entities.Card;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Views.Cards
{
    public class StatView : ViewPoolable
    {
        public event Action<CardStatsType> OnStatClicked;
        private static event Action<StatView> OnStatsClick;

        [SerializeField] private Button statButton;
        [SerializeField] private List<GameObject> progressBarLines = new List<GameObject>();
        [SerializeField] private TMP_Text valueText;
        [SerializeField] private TMP_Text typeText;
        [SerializeField] private Color selectionColor;
        [SerializeField] private Color boostedUpColor;
        [SerializeField] private int maxStatValue = 10;
        // [SerializeField] private Color boostedDownColor;

        public bool IsBoosted => _isBoosted;
        public bool IsSelected => _isSelected;
        public CardStatsType StatType => _statsType;
        
        private CardStatsType _statsType;
        private bool _isSelected;
        private bool _isBoosted;
        private int _value;
        private int _lastAddedValue;

        public void OnDestroy()
        {
            statButton.onClick.RemoveListener(OnStatClickedHandler);
            OnStatsClick -= HandleAnotherStatClicked;
        }

        public void SetData (CardStatData cardStatData)
        {
            statButton.onClick.RemoveListener(OnStatClickedHandler);
            OnStatsClick -= HandleAnotherStatClicked;
            
            OnStatsClick += HandleAnotherStatClicked;
            _value = cardStatData.Value;
            _statsType = cardStatData.StatType;
            valueText.text = _value.ToString();
            typeText.text = ((CardStatsTypeShort) cardStatData.StatType).ToString();
            statButton.onClick.AddListener(OnStatClickedHandler);

            SetProgressBar(cardStatData.Value);

            void SetProgressBar(int value)
            {
                var filledLinesCount = value * (progressBarLines.Count / maxStatValue);

                for (int i = 0; i < progressBarLines.Count; i++)
                {
                    progressBarLines[i].gameObject.SetActive(i <= filledLinesCount);
                }
            }
        }

        public void SetInteractable(bool isInteractable)
        {
            statButton.interactable = isInteractable;
        }

        public void Deselect()
        {
            statButton.image.color = Color.white;
            _isSelected = false;
        }

        public void BoostStat (int addedValue)
        {
            _lastAddedValue = addedValue;
            _isBoosted = true;
            statButton.image.color = boostedUpColor;
            valueText.text = $"{_value + _lastAddedValue}";
        }

        public void DeBoost ()
        {
            _isBoosted = false;
            valueText.text = $"{_value}";
            _lastAddedValue = 0;
        }
        
        private void OnStatClickedHandler ()
        {
            _isSelected = true;
            statButton.image.color = selectionColor;
            OnStatClicked?.Invoke(_statsType);
            OnStatsClick?.Invoke(this);
        }

        private void HandleAnotherStatClicked(StatView statView)
        {
            if (statView != this)
            {
                Deselect();
            }
        }
    }
}