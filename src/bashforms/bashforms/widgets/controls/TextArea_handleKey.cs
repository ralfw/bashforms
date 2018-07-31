﻿using System;
using System.Linq;
using EventArgs = bashforms.data.eventargs.EventArgs;

namespace bashforms.widgets.controls
{
    partial class TextArea {
        public override bool HandleKey(ConsoleKeyInfo key)
        {
            if (Handle_insertion(key) || 
                Handle_deletions(key) ||
                Handle_arrows(key)) return true;
            return false;
        }
        
        
        bool Handle_insertion(ConsoleKeyInfo key) {
            if (key.KeyChar < ' ') return false;
            if (this.Length == _maxTextLength) return true;

            _insertionPoint = _text.Insert(_insertionPoint.row, _insertionPoint.index, key.KeyChar.ToString());

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
                case ConsoleKey.LeftArrow:
                    var position = _text.GetSoftPosition(_insertionPoint.row, _insertionPoint.index);
                    position.softCol--;
                    if (position.softCol < 0) {
                        position.softRow--;
                        if (position.softRow < 0) return true;
                        position.softCol = _text.SoftLines[position.softRow].Length;
                    }
                    _insertionPoint = _text.GetIndex(position.softRow, position.softCol);
                    return true;
                case ConsoleKey.RightArrow:
                    return true;
                case ConsoleKey.UpArrow:
                    return true;
                case ConsoleKey.DownArrow:
                    return true;
                default:
                    return false;
            }
        }
    }
}