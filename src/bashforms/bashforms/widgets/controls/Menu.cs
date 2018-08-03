using System;
using System.Collections.Generic;
using bashforms.data;

namespace bashforms.widgets.controls
{
    public class Menu : FocusControl
    {
        public class Item
        {
            private List<Item> _items = new List<Item>();
        
            public Item(string text, char shortcut, string name) {
                Text = text;
                Shortcut = shortcut;
                Name = name;
                Menu = new Menu();
            }
        
            public string Text { get; }
            public char Shortcut { get; }
            public string Name { get; }
            public bool Enabled { get; set; }
            public bool Checked { get; set; }
        
            public Menu Menu { get; }
        }
        
        
        private List<Item> _items = new List<Item>();
        private List<int> _currentItemPath = new List<int>();


        public event Action<Item, EventArgs> OnSelected = (s, e) => { };

        
        private Menu() : base(-1, -1, -1, -1) {}
        public Menu(int width) : base(1, 1, width, 1) {}


        public Item[] Items => _items.ToArray();

        public Item AddItem(string text) => AddItem(new Item(text, '', ""));

        public Item AddItem(Item item) {
            _items.Add(item);
            return item;
        }

        public void AddItems(IEnumerable<Item> items) {
            _items.AddRange(items);
        }
        
        
        public override bool HandleKey(ConsoleKeyInfo key)
        {
            throw new NotImplementedException();
            /*
             * arrow keys
             * space/ENTER
             */
        }

        
        public override Canvas Draw()
        {
            throw new NotImplementedException();
            // the height depens on if a submenu is opened; a path needs to be remembered
            
            /*
             * - draw horizontal menu bar
             * - draw selected menu item
             *   - highlight item
             *   - draw vertical menu bar for item
             *   - draw selected menu item
             *     - highlight item
             *     - draw vertical menu bar for item
             * vertical menus have all items with equal width!
             */
        }
    }
}