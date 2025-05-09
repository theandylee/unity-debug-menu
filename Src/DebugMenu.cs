using System;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UIElements;

namespace HungryCouch.DebugMenu
{
    public class DebugMenu : MonoBehaviour
    {
        private static DebugMenu _instance;

        public static DebugMenu Instance
        {
            get
            {
                if (_instance != null) return _instance;

                var prefab = Resources.Load<GameObject>("DebugMenu/DebugMenu");
                var instanceGameObject = Instantiate(prefab);
                _instance = instanceGameObject.GetComponent<DebugMenu>();
                _instance.Init();
                
                return _instance;
            }
        }

        private UIDocument _document;
        private VisualTreeAsset _itemAsset;
        private VisualElement _listRoot;
        private Label _pathLabel;
        private MenuPage _rootPage;
        private MenuPage _currentPage;
        private readonly StringBuilder _stringBuilder = new(1024);

        private bool Active
        {
            get => _document.enabled;
            set
            {
                _document.enabled = value;
                if (_document.enabled)
                    UpdateDocumentLinks();
            }
        }

        private void AddMenuItem(string path, MenuItem menuItem)
        {
            path = path.ToLower();
            var splitPath = path.Split('/');

            var page = _rootPage;

            foreach (var pathLevel in splitPath)
            {
                if (pathLevel == string.Empty) continue;

                var nextPage = page.Items.FirstOrDefault(item => item is MenuPage && item.Title == pathLevel) as MenuPage;
                if (nextPage == null)
                {
                    nextPage = new MenuPage(pathLevel)
                    {
                        Parent = page
                    };
                    page.AddItem(nextPage);
                }

                page = nextPage;
            }

            if (menuItem is MenuPage addedPage)
                addedPage.Parent = page;
            page.AddItem(menuItem);
        }

        private void Init()
        {
            _rootPage = new MenuPage("");
            _currentPage = _rootPage;
            _document = _instance.GetComponent<UIDocument>();
        }
        
        private void UpdateDocumentLinks()
        {
            _listRoot = _document.rootVisualElement.Q("Items");
            _pathLabel = _document.rootVisualElement.Q<Label>("Path");
            _itemAsset = Resources.Load<VisualTreeAsset>("DebugMenu/Item");
        }

        public void Toggle()
        {
            _instance.Active = !_instance.Active;

            if (_instance.Active)
                SetPage(_rootPage);
        }

        private void RenderCurrentPage()
        {
            _listRoot.Clear();

            foreach (var item in _currentPage.Items)
            {
                if (_itemAsset.Instantiate().Children().First() is not Button button) continue;
                
                button.RegisterCallback<NavigationMoveEvent>(
                    evt =>
                    {
                        switch (evt.direction)
                        {
                            case NavigationMoveEvent.Direction.Left:
                                item.Left();
                                break;
                            case NavigationMoveEvent.Direction.Right:
                                item.Right();
                                break;
                        }
                    });

                button.RegisterCallback<FocusEvent>(
                    _ => { _currentPage.LastSelectedItem = item; });

                item.SetupButton(button);
                _listRoot.Add(button);

                if (item == _currentPage.LastSelectedItem)
                    button.Focus();
            }

            if (_currentPage.LastSelectedItem == null && _currentPage.Items.Any())
                _listRoot.Children().First().Focus();
        }

        internal void SetPage(MenuPage page)
        {
            _currentPage = page;
            RenderCurrentPage();
            _pathLabel.text = GetPath(page);
        }

        private string GetPath(MenuPage page)
        {
            _stringBuilder.Clear();
            while (page.Parent != null)
            {
                _stringBuilder.Insert(0, page.Title);
                _stringBuilder.Insert(page.Title.Length, "/");
                page = page.Parent;
            }

            _stringBuilder.Insert(0, "/");

            return _stringBuilder.ToString();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (_currentPage.Parent != null)
                    SetPage(_currentPage.Parent);
            }
        }

        public void AddValueSwitcher(string fullPath, Func<object> valueGetter, Action nextValueAction, Action previousValueAction = null)
        {
            SplitPathAndItemName(fullPath, out var path, out var itemName);
            AddMenuItem(path, new MenuValueSwitcher(itemName, valueGetter, nextValueAction, previousValueAction));
        }

        public void AddAction(string fullPath, Action action)
        {
            SplitPathAndItemName(fullPath, out var path, out var itemName);
            AddMenuItem(path, new MenuAction(itemName, action));
        }

        private void SplitPathAndItemName(string fullPath, out string path, out string itemName)
        {
            var separatorIndex = fullPath.LastIndexOf("/", StringComparison.InvariantCulture);

            if (separatorIndex == -1)
            {
                path = string.Empty;
                itemName = fullPath;
            }
            else
            {
                path = fullPath.Substring(0, separatorIndex);
                itemName = fullPath.Substring(separatorIndex + 1, fullPath.Length - separatorIndex - 1);
            }
        }
    }
}