using System;
using System.Collections.Generic;

namespace bashforms.widgets.controls
{
    public partial class MenuBar : FocusControl
    {
        public class Item
        {
            public Item(string text, string name) {
                Text = text;
                Name = name;
                Shortcut = '\0';
                Enabled = true;
                Checked = false;
                Submenu = new MenuItems();
            }
        
            public string Text { get; }
            public char Shortcut { get; set; }
            public string Name { get; }
            public bool Enabled { get; set; }
            public bool Checked { get; set; }
            public object Tag { get; set; }
        
            public MenuItems Submenu { get; }
        }

        
        public class MenuItems
        {
            private readonly List<Item> _items = new List<Item>();
            
            
            public Item[] Items => _items.ToArray();

            public Item AddItem(string text) => AddItem(new Item(text, ""));

            public Item AddItem(Item item) {
                _items.Add(item);
                return item;
            }

            public void AddItems(IEnumerable<Item> items) {
                foreach (var item in items) AddItem(item);
            }
        }


        private readonly MenuItems _menu;
        private readonly Stack<int> _selectedItems;
        private bool _hadFocus = false;
        
        public event Action<Item, EventArgs> OnSelected = (s, e) => { };

        
        public MenuBar(int left, int top, int width) : base(left, top, width, 1) {
            _menu = new MenuItems();
            _selectedItems = new Stack<int>(new[]{0});
            
            _focusBackgroundColor = ConsoleColor.Gray;
            _focusForegroundColor = ConsoleColor.Black;
        }

        public MenuItems Menu => _menu;
    }
}