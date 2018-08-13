using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using bashforms.data;

namespace bashforms.widgets.controls
{
    partial class MenuBar
    {
        /*
         * Menu structure:
         *     item1
         *         item1.1
         *         item1.2
         *     item2
         *         item2.1, shortcut: s
         *         item2.2
         *             item2.2.1
         *             item2.2.2
         *         item2.3
         *     item3
         *
         * Menu drawing:
         *
         * At start:
         * >[ item1 ][ item2 ][ item3 ]
         *
         * After selecting item2:
         * item2>[ item2.1 (s) ][ item2.2 ][ item2.3 ]
         *
         * After selecting item2.2
         * item2/item2.2>[ item2.2.1 ][ item2.2.2 ]
         */
        public override Canvas Draw()
        {
            if (this.HasFocus != _hadFocus) {_selectedItems.Clear(); _selectedItems.Push(0);}
            _hadFocus = this.HasFocus;

            var bgColor = this.HasFocus ? _focusBackgroundColor : _backgroundColor;
            var fgColor = this.HasFocus ? _focusForegroundColor : _foregroundColor;
            var canvas = new Canvas(_width, 1, bgColor, fgColor);

            var breadcrumbPath = Path_from_breadcrumbs();
            canvas.Write(0,0,breadcrumbPath);
            
            var formattedItems = _menu.Items.Select(Format_item).ToArray();
            var itemLeft = breadcrumbPath.Length;
            for (var i = 0; i < formattedItems.Length; i++) {
                canvas.Write(itemLeft, 0, formattedItems[i]);
                if (i == _selectedItems.Peek() && this.HasFocus)
                    canvas.Colorize(itemLeft,0,formattedItems[i].Length,1,ConsoleColor.DarkMagenta,ConsoleColor.White);
                itemLeft += formattedItems[i].Length;
            }

            return canvas;
            

            string Format_item(Item item) => $"[{(item.Checked ? '√' : ' ')}{item.Text} ]";
        }


        string Path_from_breadcrumbs() {
            return Path_from_breadcrumbs("", _menu, _selectedItems.ToArray());

            string Path_from_breadcrumbs(string breadcrumbs, MenuItems items, int[] path) {
                if (path.Length<=1) return breadcrumbs + ">";

                var item = items.Items[path.First()];
                breadcrumbs += (breadcrumbs.Length > 0 ? "/" : "") + item.Text;
                return Path_from_breadcrumbs(breadcrumbs, item.Submenu, path.Skip(1).ToArray());
            }
        }
    }
}