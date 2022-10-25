using Core.VBCM.Interfaces;
using UnityEngine.UI;

namespace Core.VBCM.EventSources
{
    public class ButtonEventSource<TEventHub> : EventSource<TEventHub>
        where TEventHub : EventHubBase<TEventHub>
    {
        public ButtonEventSource(Button button)
        {
            button.onClick.AddListener(OnEvent);
        }
    }
}