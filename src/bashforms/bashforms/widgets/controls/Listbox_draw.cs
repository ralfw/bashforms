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
            Render_items(canvas, _items.ToArray());
            return canvas;
        }
        
        
        Canvas Create_canvas() {
            var canvas = new Canvas(_width, _height, _backgroundColor, _foregroundColor);
            if (this.HasFocus)
                canvas.Colorize(_focusBackgroundColor, _focusForegroundColor);
            return canvas;
        }


        string Format_item(Item item) {
            if (_columns == null || _columns.Length == 0) return item.Text;
            
            var row = item.Text.Split('\t').ToArray();
            for (var c = 0; c < _columns.Length; c++) {
                if (c >= row.Length) continue;

                var cellText = row[c].Substring(0, Math.Min(row[c].Length, _columns[c]));
                cellText = cellText.PadRight(_columns[c], ' ');
                row[c] = cellText;
            }
            return string.Join("|", row);
        }

        
        void Render_items(Canvas canvas, Item[] items) {
            for (var row = 0; row < _height; row++) {
                var i = _firstItemToDisplayIndex + row;
                if (i >= _items.Count) break;
                
                canvas.Write(_selectionMode == SelectionModes.NoSelections?0:1, row, Format_item(items[i]));
                canvas.Colorize(0, row, _width, 1, items[i].BackgroundColor, items[i].ForegroundColor);

                if (_selectedItemIndexes.Contains(i)) {
                    canvas.Write(0, row, "√");
                    canvas.Colorize(0, row, _width, 1, ConsoleColor.DarkYellow, ConsoleColor.White);
                }

                if (this.HasFocus && i == _currentItemIndex)
                    canvas.Colorize(0, row, _width,1, ConsoleColor.Gray, ConsoleColor.Black);
            }
        }
    }
}