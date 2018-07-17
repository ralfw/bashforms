using System;
using EventArgs = bashforms.data.eventargs.EventArgs;

namespace bashforms.widgets.controls
{
    public class TextLine : CursorControl
    {
        private int _insertionPoint;
        private string _text;
        
        public TextLine(int left, int top, int width) : base(left, top, width, 1) {
            _text = "";
            _insertionPoint = 0;
            _focusBackgroundColor = ConsoleColor.Blue;
            _focusForegroundColor = ConsoleColor.White;
        }

        
        public string Text {
            get => _text;
            set { 
                _text = value;
                _insertionPoint = _text.Length;
                OnChanged(this, new EventArgs());
            }
        }

        
        public override void HandleKey(ConsoleKeyInfo key) {
            if (key.KeyChar >= ' ')
                this.Text += key.KeyChar;
            else
                switch (key.Key) {
                    case ConsoleKey.Backspace:
                        if (_insertionPoint > 0) {
                            _text = _text.Remove(_insertionPoint - 1, 1);
                            _insertionPoint--;
                        }
                        break;
                    case ConsoleKey.Delete:
                        if (_insertionPoint < _text.Length) {
                            _text = _text.Remove(_insertionPoint, 1);
                        }
                        break;
                    case ConsoleKey.LeftArrow:
                        if (_insertionPoint > 0) _insertionPoint--;
                        break;
                    case ConsoleKey.RightArrow:
                        if (_insertionPoint < _text.Length) _insertionPoint++;
                        break;
                }
        }

        
        public override (int x, int y) CursorPosition => (_insertionPoint, 0);
    }
}