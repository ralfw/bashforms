using System;

namespace bashforms.widgets.controls
{
    partial class MenuBar
    {
        public override bool HandleKey(ConsoleKeyInfo key)
        {
            switch (key.Key) {
                case ConsoleKey.DownArrow:
                    break;
                case ConsoleKey.UpArrow:
                    break;
                case ConsoleKey.LeftArrow:
                    if (_currentItemIndexPath.Count == 1) {
                        _currentItemIndexPath[0]--;
                        if (_currentItemIndexPath[0] < 0) _currentItemIndexPath[0] = 0;
                    }
                    break;
                case ConsoleKey.RightArrow:
                    if (_currentItemIndexPath.Count == 1) {
                        _currentItemIndexPath[0]++;
                        if (_currentItemIndexPath[0] >= _menu.Items.Length) _currentItemIndexPath[0] = _menu.Items.Length - 1;
                    }
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