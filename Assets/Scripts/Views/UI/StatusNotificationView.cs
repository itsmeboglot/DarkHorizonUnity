using Core.UiScenario;
using Core.View.ViewPool;
using Presenters.UI;
using UnityEngine;

namespace Views.UI
{
    public class StatusNotificationView : Presenter.View<StatusNotificationPopup>
    {
        [SerializeField] private int showTime;
        [SerializeField] private Transform parent;

        public void PopNotification(StatusNotification notification, string message, ViewPool viewPool)
        {
            notification.transform.SetParent(parent);
            notification.transform.localPosition = Vector3.zero;
            notification.Initialize(message, showTime, viewPool);
        }
    }
}