using System;
using System.Linq;
using EventArgs = bashforms.data.eventargs.EventArgs;

namespace bashforms.widgets.controls
{
    partial class TextArea {
        public override bool HandleKey(ConsoleKeyInfo key) {
            if (Handle_insertion(key) ||
                Handle_newline(key) ||
                Handle_deletions(key) ||
                Handle_arrows(key))
            {
                Scroll();
                return true;
            }
            return false;
        }


        bool Handle_insertion(ConsoleKeyInfo key) {
            if (key.KeyChar < ' ') return false;
            if (this.Length == _maxTextLength) return true;

            _insertionPoint = _text.Insert(_insertionPoint.row, _insertionPoint.index, key.KeyChar.ToString());

            OnEdited(this,new EventArgs());
            return true;
        }


        bool Handle_newline(ConsoleKeyInfo key) {
            if (key.Key != ConsoleKey.Enter) return false;
            
            _insertionPoint = _text.Insert(_insertionPoint.row, _insertionPoint.index, "\n");

            OnEdited(this,new EventArgs());
            return true;
        }
        
        
        bool Handle_deletions(ConsoleKeyInfo key) {
            switch (key.Key) {
                case ConsoleKey.Backspace:
                    if (_insertionPoint.row == 0 && _insertionPoint.index == 0) 
                        return true;

                    _insertionPoint = _text.Backspace(_insertionPoint.row, _insertionPoint.index);
                    
                    OnEdited(this,new EventArgs());
                    return true;
                case ConsoleKey.Delete:
                    if (_insertionPoint.row == _text.Lines.Length && _insertionPoint.index > _text.Lines.Last().Length)
                        return true;
                    
                    _insertionPoint = _text.Delete(_insertionPoint.row, _insertionPoint.index);
                    
                    OnEdited(this,new EventArgs());
                    return true;
                default:
                    return false;
            }
        }
        
        
        bool Handle_arrows(ConsoleKeyInfo key) {
            switch (key.Key) {
                case ConsoleKey.LeftArrow: {
                    var position = _text.GetSoftPosition(_insertionPoint.row, _insertionPoint.index);
                    position.softCol--;
                    if (position.softCol < 0) {
                        position.softRow--;
                        if (position.softRow < 0) return true;
                        position.softCol = Math.Max(0,_text.SoftLines[position.softRow].Length-1);
                    }
                    _insertionPoint = _text.GetIndex(position.softRow, position.softCol);
                    return true;
                }
                case ConsoleKey.RightArrow: {
                    var position = _text.GetSoftPosition(_insertionPoint.row, _insertionPoint.index);
                    position.softCol++;
                    if (position.softCol > _text.SoftLines[position.softRow].Length) {
                        position.softRow++;
                        if (position.softRow >= _text.SoftLines.Length) return true;
                        position.softCol = 0;
                    }
                    _insertionPoint = _text.GetIndex(position.softRow, position.softCol);
                    return true;
                }
                case ConsoleKey.UpArrow: {
                    var position = _text.GetSoftPosition(_insertionPoint.row, _insertionPoint.index);
                    position.softRow--;
                    if (position.softRow < 0) return true;
                    position.softCol = Math.Min(position.softCol, Math.Max(0,_text.SoftLines[position.softRow].Length - 1));
                    _insertionPoint = _text.GetIndex(position.softRow, position.softCol);
                    return true;
                }
                case ConsoleKey.DownArrow: {
                    var position = _text.GetSoftPosition(_insertionPoint.row, _insertionPoint.index);
                    position.softRow++;
                    if (position.softRow >= _text.SoftLines.Length) return true;
                    position.softCol = Math.Min(position.softCol, Math.Max(0,_text.SoftLines[position.softRow].Length - 1));
                    _insertionPoint = _text.GetIndex(position.softRow, position.softCol);
                    return true;
                }
                default:
                    return false;
            }
        }
        
        
        void Scroll() {
            var position = _text.GetSoftPosition(_insertionPoint.row, _insertionPoint.index);
            if (position.softRow < _displayFromSoftRow)
                _displayFromSoftRow = position.softRow;

            if (position.softRow > _displayFromSoftRow + _height - 1)
                _displayFromSoftRow = position.softRow - _height + 1;
        }
    }
}