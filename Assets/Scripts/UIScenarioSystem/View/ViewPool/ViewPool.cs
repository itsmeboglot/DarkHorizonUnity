using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Assertions;
using View.ViewPool;
using Zenject;

namespace Core.View.ViewPool
{
    public class ViewPool
    {
        private readonly Dictionary<string, List<ViewPoolable>> _pushedObjects =
            new Dictionary<string, List<ViewPoolable>>();

        private readonly Dictionary<string, List<ViewPoolable>> _openObjects =
            new Dictionary<string, List<ViewPoolable>>();

        private readonly Factory _factory;
        private readonly Transform _root;

        public ViewPool([CanBeNull] ViewPoolable[] prefabs, [CanBeNull] ViewPoolConfig[] viewPoolConfigs,
            DiContainer container)
        {
            if (prefabs == null)
                prefabs = new ViewPoolable[0];

            if (viewPoolConfigs != null)
            {
                foreach (var viewPoolConfig in viewPoolConfigs)
                {
                    int originalLength = prefabs.Length;
                    Array.Resize(ref prefabs, originalLength + viewPoolConfig.prefabs.Length);
                    Array.Copy(viewPoolConfig.prefabs, 0, prefabs, originalLength, viewPoolConfig.prefabs.Length);
                }
            }

            _factory = new Factory(prefabs, container);
            _root = new GameObject("ViewPool").transform;
        }

        //On scene load need to dispose
        private void Dispose()
        {
            foreach (var view in _openObjects.Values.SelectMany(openList => openList))
                view.OnPush();
        }

        public void Push(ViewPoolable view, Transform parent = null)
        {
            var openedList = GetList(view.Name, _openObjects);
            openedList.Remove(view);
            view.OnPush();
            view.Transform.SetParent(parent ? parent : _root, false);
            List<ViewPoolable> list;
            if (!_pushedObjects.TryGetValue(view.Name, out list))
                list = new List<ViewPoolable>();

            if (!list.Contains(view))
                list.Add(view);
            _pushedObjects[view.Name] = list;
        }

        public TComponent Pop<TComponent>(string name, Transform parentTransform = null)
            where TComponent : ViewPoolable
        {
            return Pop<TComponent>(name, Vector3.zero, Quaternion.identity, parentTransform);
        }

        public TComponent Pop<TComponent>(string name, Vector3 position, Quaternion rotation,
            Transform parentTransform = null) where TComponent : ViewPoolable
        {
            Assert.IsNotNull(name, "Name of view can't be null");
            var pushedList = GetList(name, _pushedObjects);
            ViewPoolable view;
            if (pushedList.Count > 0)
            {
                view = pushedList[0];
                pushedList.RemoveAt(0);
                view.Transform.SetParent(parentTransform != null ? parentTransform : _root, false);
                view.Transform.SetPositionAndRotation(position, rotation);
            }
            else
            {
                view = _factory.Create(name, position, rotation, parentTransform ? parentTransform : _root);
                view.UnityPoolManager = this;
            }

            view.OnPop();
            var openedList = GetList(name, _openObjects);
            openedList.Add(view);
            return view as TComponent;
        }

        private List<ViewPoolable> GetList(string name, Dictionary<string, List<ViewPoolable>> pool)
        {
            List<ViewPoolable> list;
            if (!pool.TryGetValue(name, out list))
                list = new List<ViewPoolable>();

            return list;
        }

        public TComponent GetFromPrefab<TComponent>(string name) where TComponent : ViewPoolable
        {
            return _factory.GetPrefab(name) as TComponent;
        }

        private class Factory
        {
            private readonly DiContainer _container;
            private readonly Dictionary<string, ViewPoolable> _prefabs;

            public Factory(ViewPoolable[] prefabs, DiContainer container)
            {
                _container = container;
                _prefabs = prefabs.ToDictionary(o => o.Name);
            }

            public ViewPoolable GetPrefab(string name)
            {
                if (!_prefabs.ContainsKey(name))
                    throw new Exception($"You have not added View: {name}");

                return _prefabs[name];
            }

            public ViewPoolable Create(string name, Vector3 position, Quaternion rotation,
                Transform parentTransform = null)
            {
                var prefab = GetPrefab(name);
                var component = _container.InstantiatePrefab(prefab, position, rotation, parentTransform)
                    .GetComponent<ViewPoolable>();
                return component;
            }
        }
    }
}