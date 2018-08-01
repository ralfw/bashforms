using System;
using bashforms.data;

namespace bashforms.widgets.controls
{
    public class Checkbox : FocusControl
    {
        private string _text;
        private bool _checked;

        public Action<Widget, EventArgs> OnEdited = (w, a) => { };
        
        
        public Checkbox(int left, int top, int width, string text) : base(left, top, width, 1) {
            _text = text;
            _checked = false;
            _focusBackgroundColor = ConsoleColor.Blue;
            _focusForegroundColor = ConsoleColor.White;
        }
        
        
        public string Text {
            get => _text;
            set {
                _text = value;
                OnUpdated(this, new data.eventargs.EventArgs());
            }
        }
        
        public bool Checked {
            get => _checked;
            set {
                _checked = value;
                OnUpdated(this, new data.eventargs.EventArgs());
            }
        }

        
        public override bool HandleKey(ConsoleKeyInfo key) {
            if (key.Key == ConsoleKey.Enter || key.Key == ConsoleKey.Spacebar) {
                _checked = !_checked;
                OnEdited(this,new EventArgs());
                return true;
            }
            return false;
        }
        
        
        public override Canvas Draw() {
            var value = _checked ? 'X' : ' ';
            var text = $"[{value}] {_text}";
            
            var canvas = new Canvas(text.Length, _height, _backgroundColor, _foregroundColor);
            canvas.Write(0,0,text);
            
            if (this.HasFocus)
                canvas.Colorize(_focusBackgroundColor, _focusForegroundColor);

            return canvas;
        }
    }
}