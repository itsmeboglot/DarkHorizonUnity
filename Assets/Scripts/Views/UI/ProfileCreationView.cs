using System;
using System.Collections.Generic;
using Core.EventAggregator.Interface;
using Core.UiScenario;
using Entities.User;
using Presenters.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Views.UI
{
    public class ProfileCreationView : 
        Presenter.View<ProfileCreationPresenter>, 
        IPublisher<ProfileCreationPresenter.ButtonClick, ProfileCreationPresenter.TextInput, ProfileCreationPresenter.AvatarVariantButtonClick>
    {
        [SerializeField] private Button createButton;
        [SerializeField] private Button avatarButton;
        [SerializeField] private Button avatarsCloseButton;
        [SerializeField] private TMP_InputField nickNameInputField;
        [SerializeField] private TMP_InputField bioInputField;

        [Header("Avatars Scroll")] [SerializeField]
        private GameObject avatarsScroll;

        [SerializeField] private Transform avatarsScrollContent;
        [SerializeField] private Button avatarButtonPrefab;

        public Func<ProfileCreationPresenter.ButtonClick> Event1 { private get; set; }
        public Func<ProfileCreationPresenter.TextInput> Event2 { private get; set; }
        public Func<ProfileCreationPresenter.AvatarVariantButtonClick> Event3 { private get; set; }


        private List<Button> _avatarVariants;

        public override void Initialize()
        {
            Binder.Bind(this);
            HideAvatarsScroll();
        }
        
        public override void OnPop()
        {
            createButton.onClick.AddListener(() => Event1().Publish(ProfileCreationPresenter.ButtonType.Create));
            avatarButton.onClick.AddListener(() => Event1().Publish(ProfileCreationPresenter.ButtonType.Avatar));
            avatarsCloseButton.onClick.AddListener(() =>
                Event1().Publish(ProfileCreationPresenter.ButtonType.AvatarsClose));

            nickNameInputField.onValueChanged.AddListener(value =>
                Event2().Publish(ProfileCreationPresenter.InputType.NickName, value));
            bioInputField.onValueChanged.AddListener(value =>
                Event2().Publish(ProfileCreationPresenter.InputType.Bio, value));
            
        }

        public override void OnPush()
        {
            createButton.onClick.RemoveAllListeners();
            avatarButton.onClick.RemoveAllListeners();
            avatarsCloseButton.onClick.RemoveAllListeners();
            nickNameInputField.onValueChanged.RemoveAllListeners();
            bioInputField.onValueChanged.RemoveAllListeners();
            ClearAvatars();
        }

        public void ShowAvatarsScroll(AvatarData[] avatars)
        {
            avatarsCloseButton.gameObject.SetActive(true);
            avatarsScroll.gameObject.SetActive(true);
            
            FillAvatars(avatars);
        }

        public void HideAvatarsScroll()
        {
            avatarsCloseButton.gameObject.SetActive(false);
            avatarsScroll.gameObject.SetActive(false);
            UnSubscribeAvatarButtons();
            ClearAvatars();

        }

        public void SetAvatarSprite(Sprite avatarSprite)
        {
            avatarButton.GetComponent<Image>().sprite = avatarSprite;
        }

        private void FillAvatars(AvatarData[] avatars)
        {
            ClearAvatars();
            _avatarVariants = new List<Button>(avatars.Length);
            for (var i = 0; i < avatars.Length; i++)
            {
                var variantButton = Instantiate(avatarButtonPrefab, avatarsScrollContent);
                variantButton.GetComponent<Image>().sprite = avatars[i].SpriteValue;
                var index = i;
                variantButton.onClick.AddListener(() =>
                {
                    Event3().Publish(ProfileCreationPresenter.ButtonType.AvatarVariant, avatars[index].ImageUrl);
                    HideAvatarsScroll();
                });
                
                _avatarVariants.Add(variantButton);
            }
        }

        private void ClearAvatars()
        {
            if(_avatarVariants == null)
                return;
            
            var avatarCount = _avatarVariants.Count;
            for (int i = 0; i < avatarCount; i++)
            {
                Destroy(_avatarVariants[i].gameObject); //ToDo: make pool
            }
            _avatarVariants.Clear();
        }

        private void UnSubscribeAvatarButtons()
        {
            if(_avatarVariants == null)
                return;
            
            foreach (var variant in _avatarVariants)
            {
                variant.onClick.RemoveAllListeners();
            }
        }

    }
}