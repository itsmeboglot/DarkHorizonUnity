using System;
using Core.View.ViewPool;
using Entities.Card;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Views.Cards
{

    public class BoosterView : ViewPoolable
    {
        [SerializeField] private Button boosterButton;
        [SerializeField] private Image mainImage;
        [SerializeField] private TMP_Text descriptionText;
        [SerializeField] private TMP_Text addedValueText;
        [SerializeField] private TMP_Text countText;

        public event Action<int> OnBoosterClick;

        private int _boosterCount;
        private int _boosterId = -1;
        private bool _multipleUse;

        public override void OnPush()
        {
            boosterButton.onClick.RemoveAllListeners(); 
            gameObject.SetActive(false);
        }

        public void SetData(BoosterViewData data)
        {
            gameObject.SetActive(true);
            _multipleUse = data.Type == BoosterType.MultipleUse;
            _boosterId = data.Id;
            mainImage.sprite = data.SpriteValue;

            if (data is StatBoosterViewData)
            {
                var tmpData = data as StatBoosterViewData;
                descriptionText.text = tmpData.StatType.ToString();
                var sign = tmpData.Value > 0? "+" : "";
                addedValueText.text = $"{sign}{tmpData.Value}";
                _boosterCount = tmpData.Count;
                countText.text = _boosterCount.ToString();
            }
            
            boosterButton.onClick.AddListener(OnBoosterClickHandler); 
        }

        private void OnBoosterClickHandler()
        {
            _boosterCount--;
            boosterButton.interactable = _multipleUse && _boosterCount > 0;
            countText.text = _boosterCount.ToString();
            OnBoosterClick?.Invoke(_boosterId);
        }
    }
}