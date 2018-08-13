using System;
using System.Collections.Generic;
using bashforms.data;
using bashforms.widgets.controls.editors;

namespace bashforms.widgets.controls
{
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