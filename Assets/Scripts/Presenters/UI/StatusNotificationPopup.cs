using Core.UiScenario;
using Core.UiScenario.Interface;
using Core.View.ViewPool;
using Unity.Settings;
using UseCases.Common;
using Views.UI;

namespace Presenters.UI
{
    public class StatusNotificationPopup : Presenter
    {
        private class Controller : Controller<StatusNotificationView>
        {
            private readonly NotificationListenerUseCase _notificationListenerUseCase;
            private readonly ViewPool _viewPool;

            public Controller(StatusNotificationView view, IWindowHandler windowHandler, 
                NotificationListenerUseCase notificationListenerUseCase, ViewPool viewPool) : base(view, windowHandler)
            {
                _notificationListenerUseCase = notificationListenerUseCase;
                _viewPool = viewPool;
            }

            public override void Open(object openData, bool animate = true)
            {
                base.Open(openData, animate);
                _notificationListenerUseCase.OnNotification += HandleNotificationMessage;
            }

            public override void OnClose()
            {
                _notificationListenerUseCase.OnNotification -= HandleNotificationMessage;
            }

            private void HandleNotificationMessage(string message)
            {
                var notification = _viewPool.Pop<StatusNotification>(Const.Poolables.StatusNotification);
                ConcreteView.PopNotification(notification, message, _viewPool);
            }
        }

        protected override void InstallBindings()
        {
            BindController<Controller>();
        }
    }
}