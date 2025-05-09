using System;
using UnityEngine.UIElements;

namespace HungryCouch.DebugMenu
{
    public class MenuValueSwitcher : MenuItem
    {
        private Action _nextValueAction;
        private Action _previousValueAction;
        private Button _button;
        private Func<object> _valueGetter;

        public MenuValueSwitcher(string title, Func<object> valueGetter, Action nextValueAction, Action previousValueAction = null) : base(title)
        {
            _previousValueAction = previousValueAction;
            _nextValueAction = nextValueAction;
            _valueGetter = valueGetter;
        }

        public override void SetupButton(Button button)
        {
            _button = button;
            button.clicked += OnSubmit;
            UpdateText();
        }

        private void OnSubmit()
        {
            _nextValueAction.Invoke();
            UpdateText();
        }

        public override void Left()
        {
            _previousValueAction?.Invoke();
            UpdateText();
        }

        public override void Right()
        {
            _nextValueAction.Invoke();
            UpdateText();
        }

        private void UpdateText()
        {
            _button.text = $"{Title} : {(_previousValueAction == null ? "" : "<")} {_valueGetter.Invoke()} >";
        }
    }
}