using Core.View.ViewPool;
using JetBrains.Annotations;
using UnityEngine;

namespace Core.UiScenario.Concrete
{
    public class WindowInfrastructure
    {
        public Camera Camera { get; }
        public ViewPool ViewPool { get; }

        public WindowInfrastructure(Camera camera, [CanBeNull] ViewPool viewPool)
        {
            Camera = camera;
            ViewPool = viewPool;
        }
    }
}