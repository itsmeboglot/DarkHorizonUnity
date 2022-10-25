using System;
using System.Collections.Generic;
using System.Linq;
using Core.UiScenario.Concrete;
using UiScenario;
using Zenject;

namespace Core.UiScenario.Pool
{
    public class ScenarioViewPool
    {
        private readonly Dictionary<WindowType, List<ScenarioViewPoolObject>> _objects =
            new Dictionary<WindowType, List<ScenarioViewPoolObject>>();

        private readonly Factory _factory;

        public ScenarioViewPool(IEnumerable<ScenarioViewPoolObject> prefabs, DiContainer container)
        {
            _factory = new Factory(prefabs, container);
        }

        public void Push(ScenarioViewPoolObject value)
        {
            value.Canvas.gameObject.SetActive(false);
            value.OnPush();
            List<ScenarioViewPoolObject> list;
            if (!_objects.TryGetValue(value.Type, out list))
                list = new List<ScenarioViewPoolObject>();

            if (!list.Contains(value))
                list.Add(value);
            _objects[value.Type] = list;
        }

        public ScenarioViewPoolObject Pop(WindowType type)
        {
            List<ScenarioViewPoolObject> list;
            if (!_objects.TryGetValue(type, out list))
                list = new List<ScenarioViewPoolObject>();

            ScenarioViewPoolObject component;
            if (list.Count > 0)
            {
                component = list[0];
                list.RemoveAt(0);
            }
            else
            {
                component = _factory.Create(type);
                component.UnityPoolManager = this;
            }

            return component;
        }

        private class Factory
        {
            private readonly DiContainer _container;
            private readonly Dictionary<WindowType, ScenarioViewPoolObject> _prefabs;

            public Factory(IEnumerable<ScenarioViewPoolObject> prefabs, DiContainer container)
            {
                _container = container;
                _prefabs = prefabs.ToDictionary(o => o.Type);
            }

            public ScenarioViewPoolObject Create(WindowType type)
            {
                if (!_prefabs.ContainsKey(type))
                    throw new Exception($"You do not add prefab view {type}.");

                var component = _container.InstantiatePrefab(_prefabs[type]).GetComponent<ScenarioViewPoolObject>();
                return component;
            }
        }
    }
}