using System;
using bashforms.widgets.controls.formatting;

namespace bashforms.widgets.controls.editors
{
    class LineEditor
    {
        private readonly int _maxLineLength;
        private string _line;
        private bool _breakingChanged;
        private string[] _softLinesCache;

        public LineEditor(int maxLineLength) : this("", maxLineLength) {}
        public LineEditor(string line, int maxLineLength) {
            if (line.IndexOf("\n") >= 0) throw new InvalidOperationException("Text may not include line breaks!");
            
            _maxLineLength = maxLineLength;
            _line = line;
            _breakingChanged = true;
        }

        public string Line => _line;

        public string[] SoftLines {
            get {
                if (_breakingChanged) {
                    _softLinesCache = _line.Break(_maxLineLength);
                    _breakingChanged = false;
                }
                return _softLinesCache;
            }
        }

        public int Insert(int index, string textToInsert, Action<string> onLinebreakInserted) {
            if (index >= _line.Length) index = _line.Length;
            
            _line = _line.Insert(index, textToInsert);
            _breakingChanged = true;
            
            var iEOL = _line.IndexOf('\n');
            if (iEOL < 0) return index + textToInsert.Length;

            var excessText = _line.Substring(iEOL + 1);
            _line = _line.Substring(0, iEOL);
            onLinebreakInserted(excessText);
            
            return _line.Length;
        }
                
        public int Delete(int index) {
            if (index < 0 || index >= _line.Length) throw new IndexOutOfRangeException("Index for deletion outside of text!");

            _line = _line.Remove(index, 1);
            _breakingChanged = true;
            return Math.Max(0, (index < _line.Length) ? index : index - 1);
        }
        
        public int Backspace(int index) {
            if (index < 1 || index > _line.Length) throw new IndexOutOfRangeException("Index for backspace outside of text!");

            index -= 1;
            _line = _line.Remove(index, 1);
            _breakingChanged = true;
            return index;
        }

        
        public (int softRow, int softCol) GetSoftPosition(int index) {
            var softRow = 0;
            var eolIndex = 0;
            foreach (var sl in this.SoftLines) {
                if (eolIndex + sl.Length > index) return (softRow, index - eolIndex);
                softRow++; eolIndex += sl.Length;
            }
            return (softRow-1, this.SoftLines[this.SoftLines.Length - 1].Length);
        }

        
        public int GetIndex(int softRow, int softCol) {
            var index = 0;
            for (var r = 0; r < Math.Min(softRow, this.SoftLines.Length); r++)
                index += this.SoftLines[r].Length;
            index += softCol;
            if (index > _line.Length) index = _line.Length;
            return index;
        }
    }
}