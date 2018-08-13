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
            Check_if_menu_got_focus();

            var colors = Select_colors();
            var canvas = new Canvas(_width, 1, colors.bgColor, colors.fgColor);

            var breadcrumbsWidth = Draw_breadcrumbs();
            Draw_current_menu_items(breadcrumbsWidth);
            
            return canvas;


            void Check_if_menu_got_focus() {
                if (this.HasFocus != _hadFocus) { 
                    _menuItemStack.Clear();
                    _currentMenuItemIndex = 0;
                }
                _hadFocus = this.HasFocus;
            }

            
            (ConsoleColor bgColor, ConsoleColor fgColor) Select_colors() {
                return (this.HasFocus ? _focusBackgroundColor : _backgroundColor,
                        this.HasFocus ? _focusForegroundColor : _foregroundColor);
            }
            
            
            int Draw_breadcrumbs() {
                var breadcrumbs = string.Join("/", _menuItemStack.PathMenuItems.Select(i => i.Text)) + ">";
                canvas.Write(0,0,breadcrumbs);
                return breadcrumbs.Length;
            }
            
            
            void Draw_current_menu_items(int left) {
                var formattedItems = _menuItemStack.CurrentMenuItems.Select(Format_item).ToArray();
                for (var i = 0; i < formattedItems.Length; i++) {
                    canvas.Write(left, 0, formattedItems[i]);
                    if (this.HasFocus && i == _currentMenuItemIndex)
                        canvas.Colorize(left,0,formattedItems[i].Length,1,ConsoleColor.DarkMagenta,ConsoleColor.White);
                    left += formattedItems[i].Length;
                }
                
                
                string Format_item(MenuItem item) => $"[{(item.Checked ? '√' : ' ')}{item.Text} ]";
            }
        }
    }
}