using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace UiScenario
{
    public class SetCanvasBounds : ITickable
    {
        public SetCanvasBounds(TickableManager tickableManager)
        {
            if (Math.Abs(Screen.safeArea.width - Screen.width) >= float.Epsilon)
            {
                tickableManager.Add(this);
                _deviceOrientation = Screen.orientation;            
            }
            _panels = new List<RectTransform>();
        }

        private readonly List<RectTransform> _panels;
        private ScreenOrientation _deviceOrientation;

        private void ApplySafeArea(Rect area)
        {
            var anchorMin = area.position;
            var anchorMax = area.position + area.size;
            SetNewBounds(ref anchorMin, ref anchorMax);
            foreach (var panel in _panels)
            {
                panel.anchorMin = anchorMin;
                panel.anchorMax = anchorMax;
            }
        }

        private void ApplySafeArea(Rect area, RectTransform panel)
        {
            var anchorMin = area.position;
            var anchorMax = area.position + area.size;
            SetNewBounds(ref anchorMin, ref anchorMax);
            panel.anchorMin = anchorMin;
            panel.anchorMax = anchorMax;
        }

        private void SetNewBounds(ref Vector2 anchorMin, ref Vector2 anchorMax)
        {
            if (_deviceOrientation == ScreenOrientation.LandscapeLeft)
            {
                anchorMin.x /= Screen.width;
                anchorMax.x = 1;
            }
            else
            {
                anchorMax.x /= Screen.width;
                anchorMin.x = 0;
            }

            anchorMin.y = 0;
            anchorMax.y = 1;
        }

        public void Tick()
        {
            if (_deviceOrientation != Screen.orientation)
            {
                _deviceOrientation = Screen.orientation;
                ApplySafeArea(Screen.safeArea);
            }
        }


        public void Add(RectTransform transform)
        {
            if (!_panels.Contains(transform))
            {
                _panels.Add(transform);
                ApplySafeArea(Screen.safeArea, transform);
            }
        }
    }
}