using System;
using System.Collections.Generic;
using System.Linq;
using bashforms.data;

namespace bashforms.widgets.controls
{
    partial class Listbox
    {
        public override Canvas Draw() {
            var canvas = Create_canvas();
            var formattedItems = Format_items();
            Render_items(canvas, formattedItems);
            return canvas;
        }
        
        
        Canvas Create_canvas() {
            var canvas = new Canvas(_width, _height, _backgroundColor, _foregroundColor);
            if (this.HasFocus)
                canvas.Colorize(_focusBackgroundColor, _focusForegroundColor);
            return canvas;
        }

        
        string[] Format_items() {
            var formattedItems = _items.Select(item => item.Text).ToArray();
            if (_columns?.Length > 0) {
                var table = formattedItems.Select(l => l.Split('\t')).ToArray();
                for (var c = 0; c < _columns.Length; c++)
                for (var r = 0; r < table.Length; r++) {
                    var row = table[r];
                    if (c >= row.Length) continue;

                    var cellText = row[c].Substring(0,Math.Min(row[c].Length, _columns[c]));
                    cellText = cellText.PadRight(_columns[c], ' ');
                    row[c] = cellText;
                }
                formattedItems = table.Select(row => string.Join("|", row)).ToArray();
            }
            return formattedItems;
        }

        
        void Render_items(Canvas canvas, string[] formattedItems) {
            for (var row = 0; row < _height; row++) {
                var i = _firstItemToDisplayIndex + row;
                if (i >= _items.Count) break;
                
                canvas.Write(_selectionMode == SelectionModes.NoSelections ? 0 : 1,row, formattedItems[i]);

                if (_selectedItemIndexes.Contains(i)) {
                    canvas.Write(0, row, "√");
                    canvas.Colorize(0, row, _width, 1, ConsoleColor.DarkYellow, ConsoleColor.White);
                }

                if (this.HasFocus && i == _currentItemIndex)
                    canvas.Colorize(0,row,_width,1, ConsoleColor.Gray, ConsoleColor.Black);
            }
        }
    }
}