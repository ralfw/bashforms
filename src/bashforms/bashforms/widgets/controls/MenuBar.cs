using System;
using System.Collections.Generic;
using bashforms.data;
using bashforms.widgets.controls.baseclasses;
using bashforms.widgets.controls.editors;

namespace bashforms.widgets.controls
{
    public class MenuItem
    {
        public MenuItem(string text, string name = "") {
            Text = text;
            Name = name;
            Shortcut = '\0';
            Enabled = true;
            Checked = false;
            Submenu = new MenuItemList();
        }
        
        public string Text { get; }
        public char Shortcut { get; set; }
        public string Name { get; }
        public bool Enabled { get; set; }
        public bool Checked { get; set; }
        public object Tag { get; set; }
        
        public MenuItemList Submenu { get; }
    }
    
    
    public class MenuItemList
    {
        private readonly List<MenuItem> _items = new List<MenuItem>();
            
            
        public MenuItem[] Items => _items.ToArray();

        public MenuItem AddItem(string text, string name = "") => AddItem(new MenuItem(text, name));

        public MenuItem AddItem(MenuItem menuItem) {
            _items.Add(menuItem);
            return menuItem;
        }

        public void AddItems(IEnumerable<MenuItem> items) {
            foreach (var item in items) AddItem(item);
        }
    }
    
    
    public partial class MenuBar : FocusControl
    {
        private readonly MenuItemStack _menuItemStack;
        private int _currentMenuItemIndex;
        private bool _hadFocus = false;
        
        public event Action<MenuItem, EventArgs> OnSelected = (s, e) => { };

        
        public MenuBar(int left, int top, int width) : base(left, top, width, 1) {
            _menuItemStack = new MenuItemStack(new MenuItemList());
            _currentMenuItemIndex = 0;
            
            _focusBackgroundColor = ConsoleColor.Gray;
            _focusForegroundColor = ConsoleColor.Black;
        }

        
        public MenuItemList Menu => _menuItemStack.RootMenu;
    }
}