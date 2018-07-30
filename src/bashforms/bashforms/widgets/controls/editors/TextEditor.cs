using System.Collections.Generic;
using System.Linq;
using bashforms.widgets.controls.formatting;

namespace bashforms.widgets.controls.editors
{
    public class TextEditor
    {
        private readonly int _maxLineLength;
        private readonly List<LineEditor> _lines;
        
        public TextEditor(int maxLineLength) : this ("", maxLineLength) {}
        public TextEditor(string text, int maxLineLength) {
            _maxLineLength = maxLineLength;
            _lines = text.ToLines().Select(l => new LineEditor(l, maxLineLength)).ToList();
        }

        
        public string Text => string.Join("\n", _lines.Select(l => l.Line));
        public string[] Lines => _lines.Select(l => l.Line).ToArray();
        public string[] SoftLines => _lines.SelectMany(l => l.SoftLines).ToArray();

        
        public (int row, int index) Insert(int row, int index, string text)
        {
            var textLines = text.ToLines();

            if (textLines.Length == 1) {
                index = _lines[row].Insert(index, textLines[0], null);
                return (row, index);
            }
            
            _lines[row].Insert(index, textLines[0] + "\n",
                remainder => {
                    index = textLines[textLines.Length - 1].Length;
                    textLines[textLines.Length - 1] += remainder;
                });
            
            _lines.InsertRange(row+1, textLines.Skip(1).Select(l => new LineEditor(l, _maxLineLength)));
            return (row+textLines.Length-1, index);
        }

        
        public (int row, int index) Delete(int row, int index)
        {
            if (index < _lines[row].Line.Length) {
                index = _lines[row].Delete(index);
                return (row, index);
            }

            if (row >= _lines.Count-1) return (row, index);

            _lines[row].Insert(_lines[row].Line.Length, _lines[row + 1].Line, null);
            _lines.RemoveAt(row+1);
            return (row, index);
        }

        
        public (int row, int index) Backspace(int row, int index) {
            if (index > 0) {
                index = _lines[row].Backspace(index);
                return (row, index);
            }

            if (row < 1) return (row, index);

            index = _lines[row - 1].Line.Length;
            _lines[row-1].Insert(_lines[row-1].Line.Length, _lines[row].Line, null);
            _lines.RemoveAt(row);
            return (row-1, index);
        }

        
        public (int softRow, int softCol) GetSoftPosition(int row, int index) {
            var position = _lines[row].GetSoftPosition(index);

            var numberOfPrecedingSoftRows = 0;
            for (var r = 0; r < row; r++)
                numberOfPrecedingSoftRows += _lines[r].SoftLines.Length;

            return (numberOfPrecedingSoftRows + position.softRow, position.softCol);
        }

        
        public (int row, int index) GetIndex(int softRow, int softCol) {
            var row = 0;
            var totalSoftRows = 0;
            foreach (var line in _lines) {
                var newTotalSoftRows = totalSoftRows + line.SoftLines.Length;
                if (newTotalSoftRows > softRow) break;
                totalSoftRows = newTotalSoftRows;
                row++;
            }
            var index = _lines[row].GetIndex(softRow - totalSoftRows, softCol);
            return (row, index);
        }
    }
}