using System;

namespace bashforms.widgets.controls
{
    partial class MenuBar
    {
        public override bool HandleKey(ConsoleKeyInfo key)
        {
            switch (key.Key) {
                case ConsoleKey.LeftArrow: {
                        var index = _selectedItems.Pop() - 1;
                        if (index < 0) index = 0;
                        _selectedItems.Push(index);
                    }
                    break;
                case ConsoleKey.RightArrow: {
                        var index = _selectedItems.Pop() + 1;
                        if (index >= _menu.Items.Length) index = _menu.Items.Length - 1;
                        _selectedItems.Push(index);
                    }
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