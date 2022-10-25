using System;
using System.Collections.Generic;
using System.Linq;
using Core.UiScenario.Concrete;
using Core.UiScenario.Data;
using Core.UiScenario.Interface;
using UiScenario;

namespace Core.UiScenario
{
    public class WindowHandler : IWindowHandler, IDisposable
    {
        private readonly WindowBuilder _windowBuilder;
        protected readonly List<IWindowController> WindowCtrls = new List<IWindowController>();
        private Action _currentAction;
        private readonly Queue<Action> _actions = new Queue<Action>();
        public event Action<WindowType> OpenWindowEvent;
        public event Action<WindowType> CloseWindowEvent;

        public WindowHandler(WindowBuilder windowBuilder)
        {
            _windowBuilder = windowBuilder;
        }

        public virtual void OpenWindow(WindowType type, object openData = null)
        {
            RunAction(() =>
            {
                var ignoredSorted = WindowCtrls.Where(c => !c.View.Attributes.Contains(WindowAttribute.IgnoreSort));
                var windowControllers = ignoredSorted as IWindowController[] ?? ignoredSorted.ToArray();
                var topWindowOrder = !windowControllers.Any()
                    ? 0
                    : windowControllers.Max(window => window.View.Canvas.sortingOrder);
                var controller = _windowBuilder.Create(type, topWindowOrder);
                WindowCtrls.Add(controller);
                controller.Closed += RemoveWindowInternal;
                if (WindowCtrls.Count > 0 && controller.View.Attributes.Contains(WindowAttribute.Modal))
                {
                    var i = WindowCtrls.Count - 1;
                    do
                    {
                        WindowCtrls[i].Block();
                        WindowCtrls[i].View.Block();
                        i--;
                    } while (i >= 0 && !WindowCtrls[i + 1].View.Attributes.Contains(WindowAttribute.Modal));
                }

                controller.Open(openData);
                OpenWindowEvent?.Invoke(controller.Type);
                ActionComplete();
            });
        }

        public IWindowController GetTopWindow()
        {
            if (WindowCtrls.Count == 0)
                return null;

            var topWindow = WindowCtrls[WindowCtrls.Count - 1];
            return topWindow;
        }

        public bool IsOpen(WindowType type)
        {
            return WindowCtrls.Any(ctrl => ctrl.Type == type);
        }

        public void CloseWindow(WindowType type)
        {
            RemoveWindowInternal(type);
        }

        private void RunAction(Action action)
        {
            if (_currentAction == null)
            {
                _currentAction = action;
                action.Invoke();
            }
            else
                _actions.Enqueue(action);
        }

        private void ActionComplete()
        {
            _currentAction = null;

            if (_actions.Count > 0)
                RunAction(_actions.Dequeue());
        }

        private void RemoveWindowInternal(WindowType type)
        {
            var windowForRemoving = WindowCtrls.LastOrDefault(c => c.Type == type);
            windowForRemoving?.Close();
        }

        private void RemoveWindowInternal(IWindowController controller)
        {
            WindowCtrls.Remove(controller);
            controller.Closed -= RemoveWindowInternal;
            controller.OnClose();
            CloseWindowEvent?.Invoke(controller.Type);
            if (!controller.View.Attributes.Contains(WindowAttribute.Modal) || WindowCtrls.Count == 0)
                return;

            var i = WindowCtrls.Count - 1;
            do
            {
                WindowCtrls[i].Unblock();
                WindowCtrls[i].View.Unblock();
                i--;
            } while (i >= 0 && !WindowCtrls[i + 1].View.Attributes.Contains(WindowAttribute.Modal));
        }

        public void Dispose()
        {
            foreach (var windowController in WindowCtrls)
            {
                windowController.View.OnPush();
                windowController.OnClose();
                if(windowController is IDisposable disposable)
                    disposable.Dispose();
            }
        }
    }
}