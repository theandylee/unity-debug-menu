using System.Collections.Generic;
using UnityEngine.UIElements;

namespace Utils
{
    internal class DebugMenuPage : DebugMenuItem
    {
        public DebugMenuPage Parent { get; set; }
        public DebugMenuItem LastSelectedItem{ get; set; }
        
        public DebugMenuPage(string title) : base(title)
        {

        }

        public IEnumerable<DebugMenuItem> Items => _items;

        private readonly List<DebugMenuItem> _items = new();

        public void AddItem(DebugMenuItem item)
        {
            _items.Add(item);
        }


        public override void SetupButton(Button button)
        {
            button.text = Title + "..";

            button.clicked += OnSubmit;
        }

        private void OnSubmit()
        {
            DebugMenu.Instance.SetPage(this);
        }

        public override void Right()
        {
            DebugMenu.Instance.SetPage(this);
        }
    }
}
