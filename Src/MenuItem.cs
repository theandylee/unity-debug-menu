using UnityEngine.UIElements;

namespace HungryCouch.DebugMenu
{
    public abstract class MenuItem
    {
        public MenuItem(string title)
        {
            _title = title;
        }
    
        protected string _title;
        public string Title => _title;

        public virtual void SetupButton(Button button)
        {
        
        }

        public virtual void Left()
        {
        
        }

        public virtual void Right()
        {
        
        }
    }
}

