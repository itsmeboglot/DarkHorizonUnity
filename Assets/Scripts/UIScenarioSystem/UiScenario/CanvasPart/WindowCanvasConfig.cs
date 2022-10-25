using System;
using UnityEngine;
using ScreenMatchMode = UnityEngine.UI.CanvasScaler.ScreenMatchMode;

namespace UiScenario
{
    [Serializable]
    public class WindowCanvasConfig
    {
        private const int DefaultOrderRange = 1;
        private const int DefaultPixelsPerUnit = 100;
        private const RenderMode DefaultRenderMode = RenderMode.ScreenSpaceOverlay;
        private const ScreenMatchMode DefaultScreenMatchMode = ScreenMatchMode.MatchWidthOrHeight;
        private const float DefaultMatch = 1f;
        private const int DefaultScreenWidth = 1920;
        private const int DefaultScreenHeight = 1080;
        private const CanvasAlignmentType DefaultAlignmentType = CanvasAlignmentType.PreferHeight;
        private const CanvasCameraType DefaultCameraMode = CanvasCameraType.Orthogonal;
        private const float DefaultCameraFieldOfView = 60f;
        private const bool DefaultInheritParentCameraSize = false;
        private const float DefaultMainCameraSize = 5;

        [SerializeField] private Vector2 _referenceResolution = new Vector2(DefaultScreenWidth, DefaultScreenHeight);
        [SerializeField] private RenderMode _renderMode = DefaultRenderMode;
        [SerializeField] private ScreenMatchMode _screenMatchMode = DefaultScreenMatchMode;
        [SerializeField] private float _match = DefaultMatch;
        [SerializeField] private int _orderRange = DefaultOrderRange;
        [SerializeField] private int _pixelsPerUnit = DefaultPixelsPerUnit;
        [SerializeField] private CanvasAlignmentType _alignmentType = DefaultAlignmentType;
        [SerializeField] private CanvasCameraType _cameraType = DefaultCameraMode;
        [SerializeField] private float _cameraFieldOfView = DefaultCameraFieldOfView;
        [SerializeField] private bool _inheritParentCameraSize = DefaultInheritParentCameraSize;
        [SerializeField] private float _mainCameraSize = DefaultMainCameraSize;

        public Vector2 ReferenceResolution => _referenceResolution;
        public RenderMode RenderMode => _renderMode;
        public ScreenMatchMode ScreenMatchMode => _screenMatchMode;
        public float Match => _match;
        public CanvasAlignmentType AlignmentType => _alignmentType;
        public int OrderRange => _orderRange;
        public int PixelsPerUnit => _pixelsPerUnit;
        public CanvasCameraType CameraType => _cameraType;
        public float CameraFieldOfView => _cameraFieldOfView;
        public bool InheritParentCameraSize => _inheritParentCameraSize;
        public float MainCameraSize => _mainCameraSize;
    }
}
