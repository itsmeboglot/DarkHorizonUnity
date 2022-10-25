using System.Collections;
using Core.View.ViewPool;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Views.UI
{
    public class StatusNotification : ViewPoolable, IPointerClickHandler
    {
        [SerializeField] private TMP_Text messageText;
        
        private ViewPool _viewPool;
        private WaitForSeconds _waiter;
        private Coroutine _lifeCycleCoroutine;

        public void Initialize(string message, int showTime, ViewPool viewPool)
        {
            messageText.text = message;
            _viewPool = viewPool;
            _waiter = new WaitForSeconds(showTime);
            _lifeCycleCoroutine = StartCoroutine(StartLifecycle());
        }

        private IEnumerator StartLifecycle()
        {
            yield return _waiter;
            _viewPool.Push(this);
        }
    
        public void OnPointerClick(PointerEventData eventData)
        {
            if(_lifeCycleCoroutine != null)
                StopCoroutine(_lifeCycleCoroutine);
            
            _viewPool.Push(this);
        }
    }
}