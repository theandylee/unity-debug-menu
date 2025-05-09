using System.Collections.Generic;
using UnityEngine.UIElements;

namespace HungryCouch.DebugMenu
{
    internal class MenuPage : MenuItem
    {
        public MenuPage Parent { get; set; }
        public MenuItem LastSelectedItem{ get; set; }
        
        public MenuPage(string title) : base(title)
        {

        }

        public IEnumerable<MenuItem> Items => _items;

        private readonly List<MenuItem> _items = new();

        public void AddItem(MenuItem item)
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
