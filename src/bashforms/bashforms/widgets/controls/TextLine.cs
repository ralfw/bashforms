﻿using System;
using System.Diagnostics;
using EventArgs = bashforms.data.eventargs.EventArgs;

namespace bashforms.widgets.controls
{
    public partial class TextLine : CursorControl
    {
        protected int _insertionPoint;
        protected string _text;
        protected int _maxTextLength;
        protected string _label;
        protected ConsoleColor _labelForegroundColor;
        
        
        public TextLine(int left, int top, int width) : base(left, top, width, 1) {
            _text = "";
            _label = "";
            _insertionPoint = 0;
            _maxTextLength = width;
            _focusBackgroundColor = ConsoleColor.Blue;
            _focusForegroundColor = ConsoleColor.White;
            _labelForegroundColor = ConsoleColor.DarkGray;
        }

        
        public string Text {
            get => _text;
            set { 
                _text = value;
                _insertionPoint = _text.Length;
                OnChanged(this, new EventArgs());
            }
        }
        
        
        public int MaxTextLength {
            get => _maxTextLength;
            set { 
                _maxTextLength = value;
                if (_text.Length > _maxTextLength) {
                    _text = _text.Substring(0, _maxTextLength);
                    _insertionPoint = Math.Min(_insertionPoint, _text.Length);
                }
                OnChanged(this, new EventArgs());
            }
        }

        
        public string Label {
            get => _label;
            set {
                _label = value;
                OnChanged(this, new EventArgs());
            }
        }
        
        public ConsoleColor LabelForegroundColor {
            get => _labelForegroundColor;
            set { _labelForegroundColor = value; OnChanged(this,new EventArgs()); }
        }

        
        public override bool HandleKey(ConsoleKeyInfo key) {
            if (key.KeyChar >= ' ') {
                if (_text.Length == _maxTextLength) return true;
                
                _text = _text.Insert(_insertionPoint, key.KeyChar.ToString());
                _insertionPoint++;
                return true;
            }
            
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
                case ConsoleKey.LeftArrow:
                    if (_insertionPoint > 0) _insertionPoint--;
                    return true;
                case ConsoleKey.RightArrow:
                    if (_insertionPoint < _text.Length) _insertionPoint++;
                    return true;
                default:
                    return false;
            }
        }

        
        public override (int x, int y) CursorPosition => (_insertionPoint < _width ? _insertionPoint : _width-1, 0);
    }
}