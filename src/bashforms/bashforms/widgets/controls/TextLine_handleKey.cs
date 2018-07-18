using System;

namespace bashforms.widgets.controls
{
    partial class TextLine {
        public override bool HandleKey(ConsoleKeyInfo key) {
            if (Handle_insertion(key) ||
                Handle_arrows(key) ||
                Handle_deletions(key)) { Scroll(); return true; }
            return false;


            void Scroll() {
                _displayFromIndex = _insertionPoint - _width + 1;
                if (_displayFromIndex < 0) _displayFromIndex = 0;
            }
        }


        bool Handle_insertion(ConsoleKeyInfo key) {
            if (key.KeyChar < ' ') return false;
            if (_text.Length == _maxTextLength) return true;
            
            _text = _text.Insert(_insertionPoint, key.KeyChar.ToString());
            _insertionPoint++;
            return true;
        }

        bool Handle_arrows(ConsoleKeyInfo key) {
            switch (key.Key) {
                case ConsoleKey.LeftArrow:
                    if (_insertionPoint > 0) {
                        _insertionPoint--;
                    }
                    return true;
                case ConsoleKey.RightArrow:
                    if (_insertionPoint < _text.Length) {
                        _insertionPoint++;
                    }
                    return true;
                default:
                    return false;
            }
        }

        bool Handle_deletions(ConsoleKeyInfo key) {
            switch (key.Key) {
                case ConsoleKey.Backspace:
                    if (_insertionPoint > 0) {
                        _text = _text.Remove(_insertionPoint - 1, 1);
                        _insertionPoint--;
                    }
                    return true;
                case ConsoleKey.Delete:
                    if (_insertionPoint < _text.Length) {
                        _text = _text.Remove(_insertionPoint, 1);
                    }
                    return true;
                default:
                    return false;
            }
        }
    }
}