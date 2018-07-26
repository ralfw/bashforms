using System;

namespace bashforms_tests
{
    class LineEditor
    {
        private readonly int _maxLineLength;
        private string _line;

        public LineEditor(int maxLineLength) : this("", maxLineLength) {}
        public LineEditor(string line, int maxLineLength) {
            if (line.IndexOf("\n") >= 0) throw new InvalidOperationException("Text may not include line breaks!");
            
            _maxLineLength = maxLineLength;
            _line = line;
        }

        public string Line => _line;
        public string[] SoftLines => _line.Break(_maxLineLength);

        public int Insert(int index, string textToInsert, Action<string> onLinebreakInserted) {
            if (index >= _line.Length) index = _line.Length;
            
            _line = _line.Insert(index, textToInsert);
            
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
            return (index < _line.Length) ? index : index - 1;
        }
        
        public int Backspace(int index) {
            if (index < 1 || index > _line.Length) throw new IndexOutOfRangeException("Index for backspace outside of text!");

            index -= 1;
            _line = _line.Remove(index, 1);
            return index;
        }
    }
}