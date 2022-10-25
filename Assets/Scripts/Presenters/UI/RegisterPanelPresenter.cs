using Core.EventAggregator;
using Core.UiScenario;
using Core.UiScenario.Interface;
using Utils.Logger;
using Utils.Tweens;
using Views.UI;

namespace Presenters.UI
{
    public class RegisterPanelPresenter : Presenter
    {
        private class Controller : Controller<RegisterPanelView>, ButtonClick.ISubscribed, TextInput.ISubscribed
        {
            private string _enteredLogin;
            private string _enteredPassword;
            private string _enteredConfirmPassword;
            
            public Controller(RegisterPanelView view, IWindowHandler windowHandler) : base(view, windowHandler)
            {
            }

            public override void Open(object openData, bool animate = true)
            {
                base.Open(openData, animate);
                ConcreteView.EnableBackBlocker();
                AnimationHelper.ShowPopup_Scale(ConcreteView.transform);
            }

            void ButtonClick.ISubscribed.OnEvent(ButtonType buttonType)
            {
                switch (buttonType)
                {
                    case ButtonType.Submit:
                        CustomLogger.Log(LogSource.Unity, $"login: {_enteredLogin}, pass: {_enteredPassword}");
                        break;
                    case ButtonType.Close:
                        ConcreteView.DisableBackBlocker();
                        AnimationHelper.HidePopup_Scale(ConcreteView.transform, doNext: Close);
                        break;
                }
            }

            void TextInput.ISubscribed.OnEvent(InputType inputType, string value)
            {
                switch (inputType)
                {
                    case InputType.Login:
                        _enteredLogin = value;
                        break;
                    case InputType.Password:
                        _enteredPassword = value;
                        ConcreteView.TranslateCorrectPasswordState(_enteredPassword == _enteredConfirmPassword);
                        break;
                    case InputType.ConfirmPass:
                        _enteredConfirmPassword = value;
                        ConcreteView.TranslateCorrectPasswordState(_enteredPassword == _enteredConfirmPassword);
                        break;
                }
            }
        }
        
        protected override void InstallBindings()
        {
            BindController<Controller>()
                .BindEvent<ButtonClick>()
                .BindEvent<TextInput>();
        }

        public class ButtonClick : EventHub<ButtonClick, ButtonType>
        {
            
        }
        
        public class TextInput : EventHub<TextInput, InputType, string>
        {
            
        }
        
        public enum ButtonType
        {
            Submit,
            Close
        }
        
        public enum InputType
        {
            Login,
            Password,
            ConfirmPass
        }
    }
}