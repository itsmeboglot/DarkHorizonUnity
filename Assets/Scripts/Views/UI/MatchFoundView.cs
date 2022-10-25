using Core.UiScenario;
using Presenters.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Views.UI
{
    public class MatchFoundView : Presenter.View<MatchFoundPresenter>
    {
        [SerializeField] private Image yourAvatar;
        [SerializeField] private Image opponentAvatar;
        [SerializeField] private TMP_Text yourNickname;
        [SerializeField] private TMP_Text opponentNickname;

        public void SetYourVisualData(string nickName, Sprite sprite)
        {
            yourAvatar.sprite = sprite;
            yourNickname.text = nickName;
        }
        
        public void SetOpponentVisualData(string nickName, Sprite sprite)
        {
            opponentAvatar.sprite = sprite;
            opponentNickname.text = nickName;
        }
    }
}