using Core.UiScenario.Concrete;
using Core.VBCM.Helper;
using UnityEngine;

namespace Core.UiScenario.Pool
{
    public abstract class ScenarioView : MonoBehaviour
    {
        [SerializeField] private WindowType _type;
        public WindowType Type => _type;
        
        private RectTransform _transform;

        public RectTransform Transform => !_transform.IsNull()
            ? _transform
            : _transform = gameObject.GetComponent<RectTransform>();
        public Canvas Canvas { get; protected set; }
    }
}