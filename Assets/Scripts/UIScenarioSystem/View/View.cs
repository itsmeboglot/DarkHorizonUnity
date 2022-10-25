using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace View
{
    public class View : MonoBehaviour
    {
        [NonSerialized]
        private string _name;
        private Transform _myTransform;
        public Transform Transform => _myTransform ? _myTransform : _myTransform = transform;
        public string Name => _name ?? (_name = name.Replace("(Clone)", string.Empty));
        public event Action<string, object> ViewEvent;

        [SerializeField] private ComponentContainer[] components = new ComponentContainer[0];
        private Dictionary<string, Component[]> Components { get; set; }

        public IEnumerable<TComponent> GetViewComponents<TComponent>(string componentId) where TComponent : Component
        {
            if (!Components.ContainsKey(componentId))
                return null;

            return Components[componentId].Cast<TComponent>();
        }

        public TComponent GetViewComponent<TComponent>(string componentId) where TComponent : Component
        {
            if (!Components.ContainsKey(componentId))
                return null;

            return Components[componentId].Cast<TComponent>().First();
        }

        protected virtual void Awake()
        {
            Components = components.GroupBy(c => c.ComponentId, c => c.Component)
                .ToDictionary(c => c.Key, c => c.ToArray());
        }

        public void OnViewEvent(string eventName)
        {
            OnViewEvent(eventName, this);
        }

        protected void OnViewEvent(string eventName, object obj)
        {
            ViewEvent?.Invoke(eventName, obj ?? this);
        }

        protected virtual void Clear()
        {
            ViewEvent = null;
        }
    }
}