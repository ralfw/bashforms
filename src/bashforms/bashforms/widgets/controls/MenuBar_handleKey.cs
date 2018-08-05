using System;

namespace bashforms.widgets.controls
{
    partial class MenuBar
    {
        public override bool HandleKey(ConsoleKeyInfo key)
        {
            switch (key.Key) {
                case ConsoleKey.DownArrow:
                    _currentItemIndex++;
                    if (_currentItemIndex >= _menu.Items.Length) _currentItemIndex = _menu.Items.Length - 1;
                    break;
                case ConsoleKey.UpArrow:
                    _currentItemIndex--;
                    if (_currentItemIndex < 0) _currentItemIndex = 0;
                    break;
                case ConsoleKey.LeftArrow:
                    break;
                case ConsoleKey.RightArrow:
                    break;
                case ConsoleKey.Spacebar:
                case ConsoleKey.Enter:
                    break;
                default:
                    return false;
            }

            return true;
        }
    }
}