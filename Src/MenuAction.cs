using System;
using UnityEngine.UIElements;

namespace Utils
{
    public class MenuAction : MenuItem
    {
        private Action _action;

        public MenuAction(string title, Action action) : base(title)
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