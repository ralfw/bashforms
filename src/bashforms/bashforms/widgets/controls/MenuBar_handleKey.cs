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
                    // select current item and oben new breadcrumb
                    break;
                case ConsoleKey.Escape:
                    // move up one menu
                    break;
                default:
                    return false;
            }

            return true;
        }
    }
}