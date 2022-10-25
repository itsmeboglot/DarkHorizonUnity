using UnityEngine;
using Object = UnityEngine.Object;

namespace UiScenario
{    
    public class WindowCanvasBuilder
    {
        private readonly WindowCanvasConfigurator _canvasConfigurator;
        private readonly Canvas _defaultCanvas;

        public WindowCanvasBuilder(WindowCanvasConfigurator canvasConfigurator, Canvas defaultCanvas)
        {
            _canvasConfigurator = canvasConfigurator;
            _defaultCanvas = defaultCanvas;       
        }

        public Canvas Create(int sortingOrder, WindowCanvasConfig config)
        {
            var canvas = Object.Instantiate(_defaultCanvas);
            canvas.transform.localPosition = Vector3.zero;
            canvas.transform.localScale = Vector3.one;
            canvas.sortingOrder = sortingOrder;
            _canvasConfigurator.UpdateCanvasSettings(canvas, config);
            return canvas;
        }
    }
}
