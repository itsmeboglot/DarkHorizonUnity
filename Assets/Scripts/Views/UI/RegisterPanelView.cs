using System;
using Core.EventAggregator.Interface;
using Core.UiScenario;
using Presenters.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils.Extensions;

namespace Views.UI
{
    public class RegisterPanelView : Presenter.View<RegisterPanelPresenter>, IPublisher<RegisterPanelPresenter.ButtonClick, RegisterPanelPresenter.TextInput>
    {
        [SerializeField] private Button submitButton;
        [SerializeField] private Button closeButton;
        [SerializeField] private Button backCloseButton;
        [SerializeField] private TMP_InputField loginField;
        [SerializeField] private TMP_InputField passwordField;
        [SerializeField] private TMP_InputField confirmPasswordField;
        
        public Func<RegisterPanelPresenter.ButtonClick> Event1 { private get; set; }
        public Func<RegisterPanelPresenter.TextInput> Event2 { private get; set; }

        public override void Initialize()
        {
            Binder.Bind(this);
        }
        
        public override void OnPop()
        {
            submitButton.onClick.AddListener(() => Event1().Publish(RegisterPanelPresenter.ButtonType.Submit));
            closeButton.onClick.AddListener(() => Event1().Publish(RegisterPanelPresenter.ButtonType.Close));
            backCloseButton.onClick.AddListener(() => Event1().Publish(RegisterPanelPresenter.ButtonType.Close));
            loginField.onValueChanged.AddListener(value => Event2().Publish(RegisterPanelPresenter.InputType.Login, value));
            passwordField.onValueChanged.AddListener(value => Event2().Publish(RegisterPanelPresenter.InputType.Password, value));
            confirmPasswordField.onValueChanged.AddListener(value => Event2().Publish(RegisterPanelPresenter.InputType.ConfirmPass, value));
        }

        public override void OnPush()
        {
            submitButton.onClick.RemoveAllListeners();
            closeButton.onClick.RemoveAllListeners();
            backCloseButton.onClick.RemoveAllListeners();
            loginField.onValueChanged.RemoveAllListeners();
            passwordField.onValueChanged.RemoveAllListeners();
            confirmPasswordField.onValueChanged.RemoveAllListeners();
        }
        
        public void EnableBackBlocker()
        {
            backCloseButton.gameObject.SetActive(true);
        }

        public void DisableBackBlocker()
        {
            backCloseButton.gameObject.SetActive(false);
        }

        public void TranslateCorrectPasswordState(bool isCorrect)
        {
            confirmPasswordField.textComponent.color = isCorrect ? Color.green : Color.red;
        }
    }
}