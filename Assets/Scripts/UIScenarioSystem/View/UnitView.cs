using System;
using Core.View.ViewPool;
using UnityEngine;
using View.ViewPool;

namespace View
{
    public class UnitView : ViewPoolable
    {
        [HideInInspector] public int Id;
        public event Action<UnitView> TriggerEvent;

        protected void OnTriggerEvent(UnitView obj)
        {
            TriggerEvent?.Invoke(obj);
        }

        protected override void Clear()
        {
            base.Clear();
            TriggerEvent = null;
        }
    }
}