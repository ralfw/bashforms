using System;
using System.Linq;
using bashforms.data;

namespace bashforms.widgets.controls
{
    partial class MenuBar
    {
        public override Canvas Draw()
        {
            if (this.HasFocus != _hadFocus) {
                _currentItemIndexPath.Clear();
                _currentItemIndexPath.Add(0);
            }
            _hadFocus = this.HasFocus;

            var bgColor = this.HasFocus ? _focusBackgroundColor : _backgroundColor;
            var fgColor = this.HasFocus ? _focusForegroundColor : _foregroundColor;
            var canvas = new Canvas(_width, 1, bgColor, fgColor);
            
            var formattedItems = _menu.Items.Select(Format_item).ToArray();
            var itemLeft = 0;
            for (var i = 0; i < formattedItems.Length; i++) {
                canvas.Write(itemLeft, 0, formattedItems[i]);
                if (i == _currentItemIndexPath[0] && this.HasFocus)
                    canvas.Colorize(itemLeft,0,formattedItems[i].Length,1,ConsoleColor.DarkMagenta,ConsoleColor.White);
                itemLeft += formattedItems[i].Length;
            }

            return canvas;
            

            string Format_item(Item item) => $"[{(item.Checked ? '√' : ' ')}{item.Text} ]";

            // the height depens on if a submenu is opened; a path needs to be remembered

            // horizontal + vertical
            // [ item1 ][√item2 ][ another item3 ]
            //                   [ some subitem ]
            //                   [ subitem 2    ][ subsubitem2.1 ]
            //                   [√sitem 3      ][ ssb2.2        ]
            //                                   [ subsubtitem   ]
            //                                   [ ssb2.4        ]

            /*
             * - draw horizontal menu bar
             * - draw selected menu item
             *   - highlight item
             *   - draw vertical menu bar for item
             *   - draw selected menu item
             *     - highlight item
             *     - draw vertical menu bar for item
             * vertical menus have all items with equal width!
             */
        }
    }
}