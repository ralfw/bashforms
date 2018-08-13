using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using bashforms.data;

namespace bashforms.widgets.controls
{
    partial class MenuBar
    {
        public override Canvas Draw()
        {
            if (this.HasFocus != _hadFocus) { _menuItemStack.Clear(); }
            _hadFocus = this.HasFocus;

            var bgColor = this.HasFocus ? _focusBackgroundColor : _backgroundColor;
            var fgColor = this.HasFocus ? _focusForegroundColor : _foregroundColor;
            var canvas = new Canvas(_width, 1, bgColor, fgColor);

            var breadcrumbs = Breadcrumbs();
            canvas.Write(0,0,breadcrumbs);
            
            Draw_current_menu_items(breadcrumbs.Length);            
            return canvas;

            
            void Draw_current_menu_items(int leftCurrentItems) {
                var formattedItems = _menuItemStack.CurrentMenuItems.Select(Format_item).ToArray();
                for (var i = 0; i < formattedItems.Length; i++) {
                    canvas.Write(leftCurrentItems, 0, formattedItems[i]);
                    if (this.HasFocus && i == _currentMenuItemIndex)
                        canvas.Colorize(leftCurrentItems,0,formattedItems[i].Length,1,ConsoleColor.DarkMagenta,ConsoleColor.White);
                    leftCurrentItems += formattedItems[i].Length;
                }
            }
            
            string Format_item(MenuItem item) => $"[{(item.Checked ? '√' : ' ')}{item.Text} ]";
        }


        string Breadcrumbs() {
            return string.Join("/", _menuItemStack.PathMenuItems.Select(i => i.Text)) + ">";
        }
    }
}