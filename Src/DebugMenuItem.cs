using UnityEngine.UIElements;

namespace Utils
{
    public abstract class DebugMenuItem
    {
        public DebugMenuItem(string title)
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

