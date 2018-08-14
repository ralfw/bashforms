using System.Collections.Generic;

namespace bashforms.data
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
}