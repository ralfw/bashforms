using System;

namespace bashforms.widgets.controls
{
    partial class MenuBar
    {
        public override bool HandleKey(ConsoleKeyInfo key)
        {
            switch (key.Key) {
                case ConsoleKey.LeftArrow:
                    _currentMenuItemIndex--;
                    if (_currentMenuItemIndex < 0) _currentMenuItemIndex = _menuItemStack.CurrentMenuItems.Length - 1;
                    break;
                case ConsoleKey.RightArrow:
                    _currentMenuItemIndex++;
                    if (_currentMenuItemIndex >= _menuItemStack.CurrentMenuItems.Length) _currentMenuItemIndex = 0;
                    break;
                case ConsoleKey.Spacebar:
                case ConsoleKey.Enter:
                    var currentMenuItem = _menuItemStack.CurrentMenuItems[_currentMenuItemIndex];
                    
                    this.OnSelected(currentMenuItem, new EventArgs());
                    if (currentMenuItem.Submenu.Items.Length > 0) {
                        _menuItemStack.PushItem(_currentMenuItemIndex);
                        _currentMenuItemIndex = 0;
                    } 
                    break;
                case ConsoleKey.Escape:
                    if (_menuItemStack.PathMenuItems.Length > 0) {
                        _currentMenuItemIndex = _menuItemStack.PopItem();
                    }
                    break;
                default:
                    return false;
            }
            return true;
        }
    }
}