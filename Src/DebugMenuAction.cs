using System;
using UnityEngine.UIElements;

namespace Utils
{
    public class DebugMenuAction : DebugMenuItem
    {
        private Action _action;

        public DebugMenuAction(string title, Action action) : base(title)
        {
            _action = action;
        }

        public override void SetupButton(Button button)
        {
            button.text = Title;
            button.clicked += OnSubmit;
        }

        private void OnSubmit()
        {
            _action?.Invoke();
        }
    }
}