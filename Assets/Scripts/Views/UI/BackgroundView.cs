using Core.UiScenario;
using Presenters.UI;
using TMPro;
using UnityEngine;

namespace Views.UI
{
    public class BackgroundView : Presenter.View<BackgroundPresenter>
    {
        [SerializeField] private TMP_Text versionText;
        [SerializeField] private TMP_Text serverTypeText;

        public void SetData(string version, string serverType)
        {
            versionText.text = version;
            serverTypeText.text = serverType;
        }
    }
}