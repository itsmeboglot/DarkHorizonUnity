using System;
using Core.EventAggregator.Interface;
using Core.UiScenario;
using JetBrains.Annotations;
using Presenters.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Views.UI
{
    public class MainMenuCentralView : Presenter.View<MainMenuCentralPresenter>, IPublisher<MainMenuCentralPresenter.ButtonClick>
    {
        [SerializeField] private TMP_Text welcomeTmp;
        [SerializeField] private TMP_Text profileName;
        [SerializeField] private Image profileImage;
        [SerializeField] private Button joinLobbyBtn;
        [SerializeField] private Button fightersButton;
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private TMP_Text resTokensCountText;

        public Func<MainMenuCentralPresenter.ButtonClick> Event1 { private get; set; }

        public override void Initialize()
        {
            Binder.Bind(this);
            
            joinLobbyBtn.onClick.AddListener(() => Event1().Publish(MainMenuCentralPresenter.ButtonType.PreGame));
            fightersButton.onClick.AddListener(() => Event1().Publish(MainMenuCentralPresenter.ButtonType.Fighters));
        }

        public void SetButtonState(bool state)
        {
            joinLobbyBtn.gameObject.SetActive(state);
        }

        public void UpdateProfileInfo(string nickname, Sprite avatar, int resTokensCount)
        {
            welcomeTmp.text = $"Welcome {nickname}";
            profileName.text = nickname;
            profileImage.sprite = avatar;
            resTokensCountText.text = resTokensCount.ToString();
        }

        public void SetInteractable(bool isInteractable)
        {
            canvasGroup.interactable = isInteractable;
        }
    }
}
